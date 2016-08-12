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
#ifndef SYMBOLEXPORTER_H
#define SYMBOLEXPORTER_H

#include <QObject>

#include "Loadable.h"
#include "CIMSymbol.h"
#include "SymbolDictionary.h"

class SymbolExporter : public QObject
{
    Q_OBJECT
public:
    explicit SymbolExporter(QObject *parent = 0);
    SymbolExporter(QString dictionaryName, QString dictionaryPathAndFile,
                   QString outputFolder, int exportSizeInPixels);

    void exportSymbol(QString symbolId);
    void exportFromCsvFile(QString csvFileName);

    void testSymbolDictionaryExport(Esri::ArcGISRuntime::SymbolType testSymbolType, bool useSimpleExport);

    void setOutputFolder(QString outputFolder);

    void setSymbolDictionaryName(QString dictionaryName);
    void setSymbolDictionaryLocation(QString dictionaryPathAndFile);
    void setImageExportSize(int exportSizeInPixels);

private:

    bool waitForLoadableInit(Esri::ArcGISRuntime::Loadable* loadable);

    QString getValidFilename(QString name);

    Esri::ArcGISRuntime::SymbolDictionary* getSymbolDictionary();

    Esri::ArcGISRuntime::Geometry getSampleGeometry(Esri::ArcGISRuntime::SymbolType symbolType);

    void setDefaults();

    Esri::ArcGISRuntime::SymbolDictionary* m_symbol_dictionary;
    Esri::ArcGISRuntime::CIMSymbol* m_cim_symbol;

    bool m_symbol_dictionary_initialized;

    QString m_dictionary_name;
    QString m_dictionary_path_and_file;
    QString m_output_folder;
    int m_export_size_in_pixels;

    class AutoDisconnector
    {
    public:
      AutoDisconnector::AutoDisconnector(QMetaObject::Connection&& connection) :
        m_conn(std::move(connection))
      {
      }

      AutoDisconnector::~AutoDisconnector()
      {
        QObject::disconnect(m_conn);
      }

    private:
      Q_DISABLE_COPY(AutoDisconnector)
      QMetaObject::Connection m_conn;
    };

signals:

  void findDone();
  void swatchDone();

public slots:


};

#endif // SYMBOLEXPORTER_H
