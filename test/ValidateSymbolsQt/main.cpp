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

#include "SymbolExporter.h"
#include "ImageComparer.h"
#include <QApplication>
#include "ArcGISRuntime.h"
#include <iostream>

using namespace std;

//
// Command line app to create symbol images using the ArcGIS Runtime and dictionary file
//
// Command line parameters explanation:
// Param 1 : Command
//   GENERATE - only generates the images to the GENERATED_IMAGES_FOLDER (Default)
//   VALIDATE - does GENERATE & then validates these against images in VALIDATED_IMAGES_FOLDER
//   VALIDATE_ONLY - validation only (doesn't do GENERATE step)
// Param 2 : Standard
//   2525 (Default)
//   APP6
// Param 3 : Filter
//   ALL - do for all found in the dictionary
//   FILE - do for all from a supplied file (Param 4)
//   SymbolId - do for a single Id supplied (Default)
// Param 4 :
//   FileName - a supplied csv file name, SymbolId must be 1st entry in comma-delimited list
//
// Note: to run this app from the command line,
// You will need the following software:
// 1. QT SDK
// 2. ArcGIS for Runtime Qt SDK (or a deployment)
// And the following in your %path%:
// 1. %QTDIR%\bin (and QTDIR set)
// 2. %ARCGISRUNTIMESDKQt_10_2_2%\arcgisruntime10.2.2\client64
//

void usage();
void removeFilesFromPreviousRun(QString folderToCleanse);

int main(int argc, char *argv[])
{
  // IMPORTANT/TODO: Make sure these folders are what you want to use as output/input
  // for your run, and change them if not:
  const QString GENERATED_IMAGES_FOLDER = "GENERATED_IMAGES";
  const QString VALIDATED_IMAGES_FOLDER_2525 = "VALIDATED_IMAGES_2525";
  const QString VALIDATED_IMAGES_FOLDER_APP6 = "VALIDATED_IMAGES_APP6";
  const QString MISMATCHED_COMPARED_IMAGES_FOLDER = "MISMATCHED_IMAGES";
  QString ValidatedImagesFolder; // this will be set to either 2525 or APP6 one

  QCoreApplication a(argc, argv);

  QString NOT_SET = "NOT SET";

  QString command = "GENERATE"; // "VALIDATE" "VALIDATE_ONLY"
  QString standard = "2525";    // "APP6"
  QString filterBy = "SIDC";    // "ALL" "FILE"
  QString optionalFile;         // "SampleFile.csv"
  QString symbolNameOrId = "SFGPUCI---AAUSG";

  QString arg1 = NOT_SET, arg2 = NOT_SET, arg3 = NOT_SET, arg4 = NOT_SET;
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
      arg1 = args[1].toUpper();
      command = arg1;
  }
  if (args.count() > 2)
  {
      arg2 = args[2].toUpper();
      standard = arg2;
  }
  if (args.count() > 3)
  {
      arg3 = args[3].toUpper();
      filterBy = arg3;

      if (!((filterBy.contains("ALL") || filterBy.contains("FILE"))))
      {
        filterBy = "SIDC";
        symbolNameOrId = arg3;
      }
      else
      {
        symbolNameOrId.clear(); // empty this if we are running with params (vs. standalone/debug)
      }
  }
  if (args.count() > 4)
  {
      arg4 = args[4].toUpper();
      optionalFile = arg4;
  }

  SymbolDictionaryType dictionaryType = SymbolDictionaryType::Mil2525C;

  if (standard.contains("APP6"))
  {
    dictionaryType = SymbolDictionaryType::App6B;
    ValidatedImagesFolder = VALIDATED_IMAGES_FOLDER_APP6;
  }
  else // 2525
  {
    dictionaryType = SymbolDictionaryType::Mil2525C;
    ValidatedImagesFolder = VALIDATED_IMAGES_FOLDER_2525;
  }

  cout << "Running with Parameters: " << endl;
  cout << "        Command: " << qPrintable(command) << endl;
  cout << "       Standard: " << qPrintable(standard) << endl;
  cout << "       FilterBy: " << qPrintable(filterBy) << endl;
  if (optionalFile.length() > 0)
    cout << "           File: " << qPrintable(optionalFile) << endl;
  if (symbolNameOrId.length() > 0)
    cout << "           SIDC: " << qPrintable(symbolNameOrId) << endl;

  if ((command == "GENERATE") || (command == "VALIDATE"))
  {
    removeFilesFromPreviousRun(GENERATED_IMAGES_FOLDER);

    SymbolExporter exporter(dictionaryType, GENERATED_IMAGES_FOLDER);

    if (filterBy == "ALL")
    {
      exporter.ExportAll();
    }
    else if (filterBy == "FILE")
    {
      exporter.ExportFromFile(optionalFile);
    }
    else if (filterBy == "SIDC")
    {
      exporter.ExportSingleSymbol(symbolNameOrId);
    }

  }

  if ((command == "VALIDATE") || (command == "VALIDATE_ONLY"))
  {
    removeFilesFromPreviousRun(MISMATCHED_COMPARED_IMAGES_FOLDER);

    ImageComparer comparer(GENERATED_IMAGES_FOLDER, ValidatedImagesFolder,
                           MISMATCHED_COMPARED_IMAGES_FOLDER, standard);

    if (!comparer.ValidateRequiredFolders())
    {
      cout << "Can't contine, required input folders do not exist, CHECK FOLDERS:" << endl;
      cout << "GENERATED_IMAGES_FOLDER: " << qPrintable(GENERATED_IMAGES_FOLDER) << endl;
      cout << "VALIDATED_IMAGES_FOLDER: " << qPrintable(ValidatedImagesFolder) << endl;
      cout << "MISMATCHED_COMPARED_IMAGES_FOLDER: " << qPrintable(MISMATCHED_COMPARED_IMAGES_FOLDER) << endl;

      exit(-1);
    }

    comparer.CompareImageFolders();
  }

}

void usage()
{
  cout << "USAGE: ValidateSymbols [GENERATE | VALIDATE | VALIDATE_ONLY] [Standard:{2525 | APP6}] "
       << "{ALL | FILE | SymbolIdCode} {Filename}";
}

void removeFilesFromPreviousRun(QString folderToCleanse)
{
  QDir outputDir(folderToCleanse);

  if (!outputDir.exists()) // !exists, nothing to do
    return;

  // TODO: decide if we really want to empty folders/files from previous runs
  // (so the folders only have output from this run)
  // If you would like to change this behaviour (for testing for instance),
  // change the setting below
  const bool REMOVE_FILES_FROM_PREVIOUS_RUN = true;
  const bool VERBOSE_DELETE = true; // print every file deleted

  if (!REMOVE_FILES_FROM_PREVIOUS_RUN)
    return;

  if (VERBOSE_DELETE)
    cout << "Deleting files from previous run from folder: " <<
          qPrintable(folderToCleanse) << endl;

  outputDir.setNameFilters(QStringList() << "*.png"); // <-- only does ".png"
  outputDir.setFilter(QDir::Files);
  QStringList previousRunFilesToDelete = outputDir.entryList();

  foreach (QString deleteFile, previousRunFilesToDelete)
  {
    if (VERBOSE_DELETE)
      cout << "  Deleting file: " << qPrintable(deleteFile) << endl;

    outputDir.remove(deleteFile);
  }
}


