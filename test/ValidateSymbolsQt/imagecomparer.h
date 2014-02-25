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
#ifndef IMAGECOMPARER_H
#define IMAGECOMPARER_H

#include <QString>

class ImageComparer
{
public:
  ImageComparer();
  ImageComparer(QString generatedImagesFolderIn, QString validatedImagesFolderIn,
                QString mismatchedImagesFolderIn, QString standardIn);

  void SetGeneratedImagesFolder(QString folderIn);
  void SetValidatedImagesFolder(QString folderIn);
  void SetMismatchedImagesFolder(QString folderIn);

  void SetStandard(QString standardIn);

  bool ValidateRequiredFolders();

  void CompareImageFolders();

private:

  QString generatedImagesFolder;
  QString validatedImagesFolder;
  QString mismatchedImagesFolder;
  QString standard;

  void compareImages(QString image1, QString image2);

};

#endif // IMAGECOMPARER_H
