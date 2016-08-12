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

#include "imagecomparer.h"
#include "imagehistogram.h"
#include <iostream>
#include <QDir>

using namespace std;

ImageComparer::ImageComparer()
{
}

ImageComparer::ImageComparer(QString generatedImagesFolderIn, QString validatedImagesFolderIn,
              QString mismatchedImagesFolderIn, QString standardIn)
{
  SetGeneratedImagesFolder(generatedImagesFolderIn);
  SetValidatedImagesFolder(validatedImagesFolderIn);
  SetMismatchedImagesFolder(mismatchedImagesFolderIn);
  SetStandard(standardIn);
}

bool ImageComparer::ValidateRequiredFolders()
{
  return (QFileInfo(generatedImagesFolder).exists() && QFileInfo(validatedImagesFolder).exists()
          && QFileInfo(mismatchedImagesFolder).exists());
}

void ImageComparer::SetStandard(QString standardIn)
{
  standard = standardIn;
}

void ImageComparer::SetGeneratedImagesFolder(QString folderIn)
{
  generatedImagesFolder  = folderIn;
}

void ImageComparer::SetValidatedImagesFolder(QString folderIn)
{
  validatedImagesFolder  = folderIn;
}

void ImageComparer::SetMismatchedImagesFolder(QString folderIn)
{
  mismatchedImagesFolder = folderIn;

  QDir outputDir(mismatchedImagesFolder);

  if (!outputDir.exists()) // create if !exists
    outputDir.mkpath(".");
}

void ImageComparer::CompareImageFolders()
{
  cout.precision(3); // set the output precision for the percent confidence

  QDir validatedImagesDir(validatedImagesFolder);

  QDir generatedImagesDir(generatedImagesFolder);
  generatedImagesDir.setNameFilters(QStringList() << "*.png"); // <-- only does ".png"
  QStringList generatedImagesFiles = generatedImagesDir.entryList(QDir::Files);

  foreach (QString imageFile, generatedImagesFiles)
  {
    // get the corresponding file name in the validated images folder
    // IMPORTANT: assumes it will have the same/exact file name as the one we want to compare
    QString validatedImage = validatedImagesDir.path() + QDir::separator() + imageFile;

    QString compareImage   = generatedImagesDir.path() + QDir::separator() + imageFile;

    compareImages(validatedImage, compareImage);

    // TODO: maybe add counter here, so you can see that it is doing something...
  }

}

/**
* @brief ImageComparer::compareImages compares 2 images to determine if there is a match
*        Just outputs the results to standard out
* @param validatedImage the truth/validated image
* @param compareImage the image to compare
*/
void ImageComparer::compareImages(QString validatedImage,
                                  QString compareImage)
{
  QFileInfo fileInfoValidatedFile(validatedImage);
  QFileInfo fileInfoCompareFile(compareImage);

  // TODO: we may want to change this output to a better-formatted .csv output

  if (!fileInfoValidatedFile.exists())
  {
    cout << "MATCH FAILED : Validated Image does not exist: " << qPrintable(validatedImage) << endl;
    return;
  }
  else if (!fileInfoCompareFile.exists())
  {
    cout << "MATCH FAILED : Compare Image does not exist: " << qPrintable(compareImage) << endl;
    return;
  }

  // they both exist so compare with ImageHistogram
  QImage qimageValidated(validatedImage);
  ImageHistogram ih(qimageValidated);

  QImage qimageCompare(compareImage);
  ImageHistogram ih2(qimageCompare);

  double confidence;
  ImageHistogram::MATCH match = ih.CompareTo(&ih2, &confidence);

  double confidenceAsPercent = confidence * 100.0;

  if (match == ImageHistogram::EXACT_MATCH)
  {
    // TODO: after testing, comment out the match lines, so we only see the fails
    // cout << "EXACT_MATCH : " << qPrintable(compareImage) << " : Confidence : "
    //     << confidenceAsPercent  << "%" << endl;
    return;
  }

  if (match == ImageHistogram::PROBABLE_MATCH)
  {
    // TODO: If you want to see the non-exact matches, then uncomment out this:
    // cout << "PROBABLE_MATCH : " << qPrintable(compareImage) << " : Confidence : "
    //     << confidenceAsPercent  << "%" << endl;
    return;
  }

  // else if we are here the match definitely failed...

  cout << "MATCH FAILED : " << qPrintable(compareImage) << " : ";

  if (match == ImageHistogram::POSSIBLE_MATCH)
    cout << "POSSIBLE MATCH";
  else
    cout << "NO MATCH";

  cout << " : Confidence : " << confidenceAsPercent << "%";

  cout << " : Standard : " << qPrintable(standard);

  //////////////////////////////////////////////////////////
  // Copy the mismatched files to the "mismatched folder"
  QString image1FileNameNoExtOnly(fileInfoValidatedFile.baseName());

  QDir mismatchedImagesDir(mismatchedImagesFolder);

  // change the image name to add "_VALIDATED." ex. "ImageName_VALIDATED.png"
  QString newValidatedImageFileName = mismatchedImagesDir.path() + QDir::separator()
      + image1FileNameNoExtOnly + "_VALIDATED." + fileInfoValidatedFile.completeSuffix();

  QString image2FileNameOnly(fileInfoCompareFile.fileName());
  QString mismatchedImageFileName = mismatchedImagesDir.path() + QDir::separator() + image2FileNameOnly;

  if (QFile::exists(mismatchedImageFileName))
      QFile::remove(mismatchedImageFileName);

  if (QFile::exists(newValidatedImageFileName))
      QFile::remove(newValidatedImageFileName);

  // Copy both files to "mismatched folder"
  QFile::copy(validatedImage, newValidatedImageFileName); // validated image with fname + _VALIDATED
  QFile::copy(compareImage, mismatchedImageFileName);   // mismatched image
  //
  //////////////////////////////////////////////////////////

  cout << endl;
}
