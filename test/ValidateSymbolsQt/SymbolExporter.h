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
#ifndef SYMBOLEXPORTER_H
#define SYMBOLEXPORTER_H

#include "SymbolDictionary.h"

using namespace EsriRuntimeQt;

/**
 * @brief Works with AGS Runtime to export dictionary images
 *        Options include export all dictionary entries, entries from a file,
 *        or single entry/image
 *
 */
class SymbolExporter
{
public:

  SymbolExporter();
  ~SymbolExporter();

  void SetSymbolDictionaryType(SymbolDictionaryType dictionaryTypeIn);
  void SetOutputFolder(QString outputFolderIn);
  void SetImageExportSize(int exportSizeInPixelsIn);

  void ExportAll();
  void ExportFromFile(QString fileName);
  void ExportSingleSymbol(QString symbolNameOrId);

private:

  QString getValidFilename(QString name);
  void getDictionary();

private:

  QString dictionaryPathAndFile;
  QString outputFolder;
  SymbolDictionaryType dictionaryType;
  SymbolDictionary dictionary;
  int exportSizeInPixels;
  bool dictionaryInitialized;
  bool enableVerbose;
};

#endif // SYMBOLEXPORTER_H

