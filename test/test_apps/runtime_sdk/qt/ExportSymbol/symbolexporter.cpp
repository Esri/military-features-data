/* Copyright 2014-2016 Esri
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#include "symbolexporter.h"

#include <QImage>
#include <QSignalSpy>
#include <QEventLoop>
#include <QTimer>
#include <QFile>
#include <QDir>

#include <iostream>

#include "SymbolDictionary.h"
#include "ArcGISRuntimeEnvironment.h"
#include "PointBuilder.h"
#include "PolylineBuilder.h"
#include "PointBuilder.h"
#include "PolygonBuilder.h"

using namespace Esri::ArcGISRuntime;
using namespace std;

SymbolExporter::SymbolExporter(QObject *parent) : QObject(parent)
{
  setDefaults();
}

SymbolExporter::SymbolExporter(QString dictionaryName, QString dictionaryPathAndFile,
               QString outputFolder, int exportSizeInPixels)
{
  setDefaults();

  setSymbolDictionaryName(dictionaryName);
  setSymbolDictionaryLocation(dictionaryPathAndFile);
  setOutputFolder(outputFolder);
  setImageExportSize(exportSizeInPixels);
}

void SymbolExporter::setDefaults()
{
  m_export_size_in_pixels = 128;
  m_symbol_dictionary_initialized = false;
}

void SymbolExporter::setSymbolDictionaryName(QString dictionaryName)
{
    m_dictionary_name = dictionaryName;
}

void SymbolExporter::setSymbolDictionaryLocation(QString dictionaryPathAndFile)
{
  QFile file(dictionaryPathAndFile);

  if (file.exists())
    m_dictionary_path_and_file = dictionaryPathAndFile;
  else
    cout << "Dictionary/Style File does not exist: " << qPrintable(dictionaryPathAndFile) << endl;
}

void SymbolExporter::setOutputFolder(QString outputFolder)
{
  QDir outputDir(outputFolder);

  if (outputDir.exists())
  {
    m_output_folder = outputFolder;
  }
  else
  {
    cout << "Image output folder does not exist" << endl;
    // TODO:
    // create if !exists ?
    // outputDir.mkpath(".");
  }
}

void SymbolExporter::setImageExportSize(int exportSizeInPixels)
{
  if (exportSizeInPixels > 10)
    m_export_size_in_pixels = exportSizeInPixels;
}

bool SymbolExporter::waitForLoadableInit(Loadable* loadable)
{
  const int timeoutInMs = 10000;

  if (!loadable)
    return false;

  if (loadable->loadStatus() == LoadStatus::Loaded)
    return true;

  QEventLoop loop;
  QTimer timer;
  timer.setInterval(timeoutInMs);
  timer.setSingleShot(true);

  loop.connect(static_cast<SymbolDictionary*>(loadable), SIGNAL(loadStatusChanged(Esri::ArcGISRuntime::LoadStatus)), SLOT(quit()));

  loop.connect(&timer, SIGNAL(timeout()), SLOT(quit()));

  if (loadable->loadStatus() == LoadStatus::NotLoaded)
    loadable->load();

  timer.start();
  while (timer.isActive())
  {
    loop.exec();

    if (loadable->loadStatus() == LoadStatus::Loaded ||
        loadable->loadStatus() == LoadStatus::FailedToLoad)
      break;

    if (!timer.isActive())
     return false;
  }
  return loadable->loadStatus() == LoadStatus::Loaded;
}

QString SymbolExporter::getValidFilename(QString name)
{
  QString validFilename = name;

  // Replace any invalid file character:
  validFilename.replace('*','-');
  validFilename.replace(' ','_');
  validFilename.replace('\\', '-');
  validFilename.replace(':', '-');
  validFilename.replace('?', '-');
  validFilename.replace('<', '-');
  validFilename.replace('>', '-');
  validFilename.replace('|', '-');
  validFilename.replace('\"', '-');
  validFilename.replace(' ', '-');

  return validFilename;
}

Esri::ArcGISRuntime::SymbolDictionary* SymbolExporter::getSymbolDictionary()
{
  if (m_dictionary_name.isEmpty() || m_dictionary_path_and_file.isEmpty())
  {
    cout << "Symbol Dictionary Name/Location not set" << endl;
    return nullptr;
  }

  // Note: not thread-safe
  if (m_symbol_dictionary_initialized)
    return m_symbol_dictionary;

  m_symbol_dictionary = new SymbolDictionary(m_dictionary_name, m_dictionary_path_and_file);
  waitForLoadableInit(m_symbol_dictionary);

  return m_symbol_dictionary;
}

void SymbolExporter::exportSymbol(QString symbolId)
{
  QVariantMap searchKeys;
  searchKeys["sidc"] = symbolId;

  auto sd = getSymbolDictionary();

  if (sd == nullptr)
  {
    cout << "Can't get/create SymbolDictionary" << endl;
    return;
  }

  AutoDisconnector d(connect(sd, &SymbolDictionary::findSymbolCompleted, this,
    [this](CIMSymbol* symbol)
    {
      m_cim_symbol = symbol;
      emit findDone();
    }));

  QSignalSpy spy(this, SIGNAL(findDone()));

  sd->findSymbol(searchKeys);
  spy.wait();

  // Check the type of symbol returned
  SymbolType st = m_cim_symbol->symbolType();

  Geometry geo = getSampleGeometry(st);

  // generate the swatch
  QImage swatch;
  AutoDisconnector d2(connect(m_cim_symbol, &Symbol::createSwatchCompleted, this,
    [this, &swatch](QUuid, QImage image)
    {
      swatch = image;
      emit swatchDone();
    }));

  QSignalSpy swatchSpy(this, SIGNAL(swatchDone()));

  int dpi = m_export_size_in_pixels;
  if (st != SymbolType::CIMPointSymbol)
    dpi = m_export_size_in_pixels / 2;
  m_cim_symbol->createSwatch(geo, m_export_size_in_pixels, m_export_size_in_pixels, dpi);

  swatchSpy.wait();

  if (swatch.byteCount() <= 0)
  {
    cout << "No Image Created" << endl;
    return;
  }

  QString fileName = getValidFilename(symbolId); // Just in case SIDC has "*" or special chars
  QString imageFileName = m_output_folder + QDir::separator() + fileName + ".png";

  QFile imageFilePath(imageFileName); // using QFile to convert to correct file separator
  QString imagePathAndFile = imageFilePath.fileName();

  swatch.save(imagePathAndFile);

  cout << "Exported: " << qPrintable(symbolId) << endl;
}

void SymbolExporter::exportFromCsvFile(QString csvFileName)
{
  QFile csvFile(csvFileName);

  if (!csvFile.open(QIODevice::ReadOnly | QIODevice::Text))
  {
    cout << "****----> CSV File open FAILED: " << qPrintable(csvFileName) << endl;
    return;
  }

  while (!csvFile.atEnd())
  {
    QString csvLine = csvFile.readLine();

    if (csvLine.startsWith("#")) // Skip lines with "#"
      continue;

    // assumes SIDC is first column
    QStringList columns = csvLine.split(",");
    if (columns.length() > 0)
    {
      QString sidc = columns.value(0);
      exportSymbol(sidc);
    }
  }

  csvFile.close();
}

void SymbolExporter::testSymbolDictionaryExport(Esri::ArcGISRuntime::SymbolType testSymbolType, bool useSimpleExport)
{
  QVariantMap searchKeys;

  if (testSymbolType == SymbolType::CIMPointSymbol)
  {
    // Point Symbol
    searchKeys["identity"] = 3;
    searchKeys["symbolset"] = 10;
    searchKeys["echelon"] = 14;
    searchKeys["symbolentity"] = 121105;
    // searchKeys["modifier1"] = 10;
    // searchKeys["modifier2"] = 5;
  }
  else if (testSymbolType == SymbolType::CIMLineSymbol)
  {
    // Line Symbol
    searchKeys["identity"] = 3;
    searchKeys["symbolset"] = 25;
    searchKeys["symbolentity"] = 290204;
  }
  else if (testSymbolType == SymbolType::CIMPolygonSymbol)
  {
    // Polygon Symbol
    searchKeys["identity"] = 3;
    searchKeys["symbolset"] = 25;
    searchKeys["symbolentity"] = 120200;
  }
  else
  {
     cout << "Invalid Symbol Type" << endl;
  }

  auto sd = getSymbolDictionary();

  if (sd == nullptr)
  {
    cout << "Can't get/create SymbolDictionary" << endl;
    return;
  }

  AutoDisconnector d(connect(sd, &SymbolDictionary::findSymbolCompleted, this,
    [this](CIMSymbol* symbol)
    {
      m_cim_symbol = symbol;
      emit findDone();
    }));

  QSignalSpy spy(this, SIGNAL(findDone()));

  sd->findSymbol(searchKeys);
  spy.wait();

  // Check the type of symbol returned
  SymbolType st = m_cim_symbol->symbolType();

  Geometry geo = getSampleGeometry(st);

  // generate the swatch
  QImage swatch;
  AutoDisconnector d2(connect(m_cim_symbol, &Symbol::createSwatchCompleted, this,
    [this, &swatch](QUuid, QImage image)
    {
      swatch = image;
      emit swatchDone();
    }));

  QSignalSpy swatchSpy(this, SIGNAL(swatchDone()));

  if (useSimpleExport)
  {
    m_cim_symbol->createSwatch();
  }
  else
  {
    int dpi = m_export_size_in_pixels;
    if (st != SymbolType::CIMPointSymbol)
      dpi = m_export_size_in_pixels / 2;
    m_cim_symbol->createSwatch(geo, m_export_size_in_pixels, m_export_size_in_pixels, dpi);
  }

  swatchSpy.wait();

  if (swatch.byteCount() <= 0)
    cout << "No Image Created" << endl;

  QString imageBaseName = "QtSDK_Swatch_Export";

  switch (st)
  {
    case SymbolType::CIMPointSymbol : imageBaseName += "_Point"; break;
    case SymbolType::CIMLineSymbol : imageBaseName += "_Line"; break;
    case SymbolType::CIMPolygonSymbol : imageBaseName += "_Polygon"; break;
  }

  if (useSimpleExport)
    imageBaseName += "_Simple";

  QString imageFileName = m_output_folder + QDir::separator() + imageBaseName + ".png";

  QFile imageFilePath(imageFileName); // using QFile to convert to correct file separator
  QString imagePathAndFile = imageFilePath.fileName();

  swatch.save(imagePathAndFile);

}

Esri::ArcGISRuntime::Geometry SymbolExporter::getSampleGeometry(Esri::ArcGISRuntime::SymbolType symbolType)
{
    Geometry geo;

    if (symbolType == SymbolType::CIMPointSymbol)
    {
      PointBuilder* ptBuilder = new PointBuilder(SpatialReference(4326), this);
      // This works, but *only* if point is (0, 0)
      ptBuilder->setXY(0.0, 0.0);

      Point point = ptBuilder->toPoint();
      geo = point;
    }
    else if (symbolType == SymbolType::CIMLineSymbol)
    {
        PolylineBuilder* plBuilder = new PolylineBuilder(SpatialReference(4326), this);
        plBuilder->addPoint(-50.0, 0.0);
        plBuilder->addPoint(50.0, 0.0);

        Polyline polyline = plBuilder->toPolyline();
        geo = polyline;
    }
    else if (symbolType == SymbolType::CIMPolygonSymbol)
    {
        PolygonBuilder* pgBuilder = new PolygonBuilder(SpatialReference(4326), this);
        pgBuilder->addPoint(-50.0, -50.0);
        pgBuilder->addPoint(-50.0, 50.0);
        pgBuilder->addPoint(50.0, 50.0);
        pgBuilder->addPoint(50.0, -50.0);
        pgBuilder->addPoint(-50.0, -50.0);

        Polygon pg = pgBuilder->toPolyline();
        geo = pg;
    }
    else
    {
        cout << "Invalid Symbol Type" << endl;
    }

    return geo;
}
