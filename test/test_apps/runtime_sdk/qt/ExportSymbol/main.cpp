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

#include <QCoreApplication>
#include <iostream>

#include "symbolexporter.h"

using namespace std;
using namespace Esri::ArcGISRuntime;

void usage();

//
// Command line app to create symbol images using the ArcGIS Runtime and Symbol Dictionary
//
// Requirements:
//    ArcGIS Runtime SDK for Qt: https://developers.arcgis.com/qt/
//    Version: Quartz Beta or later
//
// Command line parameters explanation:
// Param 1 :
//   SymbolId - do for a single Id supplied (Default)
//   FILE - do for all from a supplied file (Param 2)
//   TEST - generate a few test images
// Param 2 :
//   FileName - a supplied csv file name, SymbolId must be 1st entry/column in comma-delimited file
//
// Note: to run this app from the command line,
// You will need the following software:
// 1. QT SDK
// 2. ArcGIS for Runtime Qt SDK (or a deployment)
// And the following in your %path%:
// 1. Qt Binaries: %QTDIR%\bin (and QTDIR set)
// 2. Runtime SDK: Ex: C:\Program Files (x86)\ArcGIS SDKs\Qt100.0\sdk\windows\x64\bin\release (or debug)

int main(int argc, char *argv[])
{
    ///////////////////////////////////////////////
    // IMPORTANT/TODO: you must change these to the location in your configuration
    //
    QString mil2525dSpec = "mil2525d";
    QString dictionaryPath = "C:/applications/output/resources/symbols/mil2525d/mil2525d.stylx";
    QString outputPath = "C:/applications/output/images";
    ///////////////////////////////////////////////

    int exportSize = 128;

    QCoreApplication a(argc, argv);

    QString sidc = "10033000001401090000", csvFile = "NOT SET";

    QStringList args = a.arguments();
    if (args.count() <= 1)
    {
      usage();

      // TODO: we may want to just exit, but for now run with the above defaults
      // (it will just export 1 SIDC), this will allow an easy sanity check of configuration
      // exit(-1);
    }

    if (args.count() > 1)
    {
        sidc = args[1].toUpper();
    }
    if (args.count() > 2)
    {
        csvFile = args[2];
    }

    SymbolExporter exporter(mil2525dSpec, dictionaryPath, outputPath, exportSize);

    if (sidc == "FILE")
    {
        exporter.exportFromCsvFile(csvFile);
    }
    else if (sidc == "TEST")
    {
        exporter.testSymbolDictionaryExport(SymbolType::CIMPointSymbol, true);
        exporter.testSymbolDictionaryExport(SymbolType::CIMLineSymbol, true);
        exporter.testSymbolDictionaryExport(SymbolType::CIMPolygonSymbol, true);
        exporter.testSymbolDictionaryExport(SymbolType::CIMPointSymbol, false);
        exporter.testSymbolDictionaryExport(SymbolType::CIMLineSymbol, false);
        exporter.testSymbolDictionaryExport(SymbolType::CIMPolygonSymbol, false);
    }
    else
    {
        exporter.exportSymbol(sidc);
    }

    return 0; // a.exec();
}

void usage()
{
    cout << "USAGE: ExportSymbol [Symbol ID/\"TEST\"/\"FILE\"] {CSV Filename(\"FILE\" option)}" << endl;
}

