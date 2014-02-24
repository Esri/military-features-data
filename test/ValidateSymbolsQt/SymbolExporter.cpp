/* Copyright 2014 Esri
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
#include <QtCore/QCoreApplication>
#include <QtSql>

#include "SymbolExporter.h"
#include "ArcGISRuntime.h"
#include "SymbolDictionary.h"
#include <iostream>

using namespace std;

// TODO: this path will need changed to match the current version
QString dictionaryPathAndFileApp6B = "/ArcGISRuntime10.2.2/resources/symbols/app6b/app6b.dat";
QString dictionaryPathAndFile2525C = "/ArcGISRuntime10.2.2/resources/symbols/mil2525c/mil2525c.dat";

SymbolExporter::SymbolExporter()
{  
  dictionaryType     = SymbolDictionaryType::Mil2525C;
  outputFolder       = "ExportedImages";
  exportSizeInPixels = 256;
  dictionaryInitialized = false;
  enableVerbose = true;
}

SymbolExporter::~SymbolExporter()
{

}

void SymbolExporter::SetSymbolDictionaryType(SymbolDictionaryType dictionaryTypeIn)
{
  dictionaryType = dictionaryTypeIn;
}

void SymbolExporter::SetOutputFolder(QString outputFolderIn)
{
  outputFolder = outputFolderIn;

  QDir outputDir(outputFolder);

  if (!outputDir.exists())  // create if !exists
    outputDir.mkpath(".");
  else
  {
    // TODO: decide if we want to empty this folder before we run
    // so the folder only has output from this run
  }

}

void SymbolExporter::SetImageExportSize(int exportSizeInPixelsIn)
{
  exportSizeInPixels = exportSizeInPixelsIn;
}

void SymbolExporter::getDictionary()
{
  if (!dictionaryInitialized)
  {
    dictionary = SymbolDictionary(dictionaryType);

    QString runtimePath = ArcGISRuntime::installDirectory();

    // Ex: Install Dir: /runtime_sdks/qt10.2.2
    //   + ArcGISRuntime10.2.2/resources/symbols
    //   + Mil2525C/Mil2525C.dat

    if (dictionaryType == SymbolDictionaryType::App6B)
    {
      runtimePath.append(dictionaryPathAndFileApp6B);
    }
    else
    {
      runtimePath.append(dictionaryPathAndFile2525C);
    }

    QFile dataPath(runtimePath); // using QFile to convert to correct file separator
    dictionaryPathAndFile = dataPath.fileName();

    if (!dataPath.exists())
      qDebug() << "Dictionary file does not exist: " << dictionaryPathAndFile;

    dictionaryInitialized = true;
  }
}

QString SymbolExporter::getValidFilename(QString name)
{
  QString validFilename = name;
  validFilename.replace('*','-');
  validFilename.replace(' ','_');

  QDir outputDir(outputFolder);
  QString filename = outputDir.path() + QDir::separator() + validFilename;

  return filename;
}

void SymbolExporter::ExportSingleSymbol(QString symbolNameOrId)
{
  getDictionary();

  if (symbolNameOrId.length() != 15)
  {
    // This prevents the modfier entries (non SIDC ones) from being exported
    cout << "--> Invalid SIDC: " << qPrintable(symbolNameOrId) << ", skipping export" << endl;
    return;
  }

  QImage image = dictionary.symbolImage(symbolNameOrId, exportSizeInPixels, exportSizeInPixels);

  if (image.isNull() || (image.size().height() == 0))
  {
    cout << "--> Export Failed: " << qPrintable(symbolNameOrId) << endl;
    return;
  }

  QString fname = getValidFilename(symbolNameOrId + ".png");

  // If printout of each is desired:
  if (enableVerbose)
    cout << "Exporting: " << qPrintable(fname) << std::endl;

  image.save(fname);
}

void SymbolExporter::ExportFromFile(QString fileName)
{
  getDictionary();

  QFile csvFile(fileName);

  if (!csvFile.open(QIODevice::ReadOnly | QIODevice::Text))
  {
    cout << "CSV File open FAILED: " << qPrintable(fileName);
    return;
  }

  while (!csvFile.atEnd())
  {
    QString csvLine = csvFile.readLine();

    if (csvLine.startsWith("#")) // Skip lines with "#"
      continue;

    QStringList columns = csvLine.split(",");
    if (columns.length() > 0)
    {
      QString nameOrSidc  = columns.value(0);
      ExportSingleSymbol(nameOrSidc);
    }
  }

  csvFile.close();

}

void SymbolExporter::ExportAll()
{
  getDictionary();

  // IMPORTANT: Requires either QtSDK to be installed or the data provider deployed with app
  QSqlDatabase db = QSqlDatabase::addDatabase("QSQLITE");
  db.setDatabaseName(dictionaryPathAndFile);
  bool opened = db.open();

  cout << "databaseName = " << qPrintable(dictionaryPathAndFile) << ", Opened = " << opened << endl;

  QSqlQuery query(QString("SELECT Name,SymbolId FROM SymbolInfo"), db);
  QHash<QString, QString> sidc2Name;
  while (query.next())
  {
    QString name = query.value(0).toString();
    QString sidc = query.value(1).toString();

    if (sidc.length() > 10)
      sidc2Name.insert(sidc, name);
  }
  query.clear();
  db.close();

  int exportCount = 0;
  enableVerbose = false;  // disable verbose when we are exporting all(but change here if you want)

  QHash<QString, QString>::iterator i;
  for (i = sidc2Name.begin(); i != sidc2Name.end(); ++i)
  {
      QString name = i.value();
      QString sidc = i.key();
      ExportSingleSymbol(sidc);

      // If printout of each is desired:
      if (enableVerbose)
        cout << "Name=" << qPrintable(name) << ", SIDC=" << qPrintable(sidc) << endl;

      if ((exportCount++ % 100) == 0)
        cout << "Exported Image #" << exportCount << endl;
  }
}

