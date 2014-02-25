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

#include "imagehistogram.h"

ImageHistogram::ImageHistogram(const QImage imageIn)
{
  image = imageIn;
}

ImageHistogram::MATCH ImageHistogram::CompareTo(ImageHistogram* other, double* confidence)
{
  MATCH match = MATCH_FAILED;

  this->calculateHistogram();
  this->calculateHistogramEqualizationTable();
  other->calculateHistogram();
  other->calculateHistogramEqualizationTable();

  int difference = this->compareHistogram(other);

  int maxPossibleError = 256*256;

  if (difference < (maxPossibleError * 0.001))      // < 60 - exact
  {
    match = EXACT_MATCH;
  }
  else if (difference < (maxPossibleError * 0.025))      // < ~1200 - same
  {
    match = PROBABLE_MATCH;
  }
  else if (difference < (maxPossibleError * 0.045)) // 1200-2400 - possibly different
  {
    match = POSSIBLE_MATCH;
  }
  else // > 2400 - different
  {
    match = MATCH_FAILED;
  }

  *confidence = 1.0 - ((double)difference / (double)maxPossibleError);

  return match;
}

void ImageHistogram::calculateHistogram()
{
  QRgb value;
  QRgb valueR;
  QRgb valueG;
  QRgb valueB;
  QRgb valueAlpha;

  mapR = QVector<int>(256);
  mapG = QVector<int>(256);
  mapB = QVector<int>(256);

  for( int y = 0; y < image.height(); y++ )
  {
    for( int x = 0; x < image.width(); x++ )
    {
      value  = image.pixel(x,y);
      valueR = qRed(value);
      valueG = qGreen(value);
      valueB = qBlue(value);
      valueAlpha = qAlpha(value);

      if ((valueR == 255) && (valueG == 255) && (valueB == 255) ||
          (valueAlpha == 0)) // we may also need to play with this alpha value check, e.g. < 128
        // TICKY: Don't count whites or transparents (background colors in our case)
        // NOTE: this will affect the correctness if an image really has white in it
        // since whites are ignored (a few of the mil symbols do have some white center icons)
        continue;

      mapR[valueR] = mapR.value(valueR) + 1;
      mapG[valueG] = mapG.value(valueG) + 1;
      mapB[valueB] = mapB.value(valueB) + 1;
    }
  }

  // Normalize the histogram
  double size = (double)(image.width() * image.height());

  mapNormalR = QVector<double>(256);
  mapNormalG = QVector<double>(256);
  mapNormalB = QVector<double>(256);

  for (int i = 0; i < 256; i++ )
  {
    mapNormalR[i] = mapR[i] / size;
    mapNormalG[i] = mapG[i] / size;
    mapNormalB[i] = mapB[i] / size;
  }
}

void ImageHistogram::calculateHistogramEqualizationTable()
{
  histogramEqualizationTable = QVector<QRgb>(256);

  double sumR = 0.0;
  double sumG = 0.0;
  double sumB = 0.0;

  // Compute the CDF
  for(int i = 0; i < 256; i++ )
  {
    sumR += mapNormalR[i];
    sumG += mapNormalG[i];
    sumB += mapNormalB[i];

    histogramEqualizationTable[i] =
     qRgb((int)(sumR * 255.0 + 0.5 ),
          (int)(sumG * 255.0 + 0.5 ),
          (int)(sumB * 255.0 + 0.5 ));
  }
}

int ImageHistogram::compareHistogram(ImageHistogram* other)
{
  QRgb value;
  QRgb valueR;
  QRgb valueG;
  QRgb valueB;

  QRgb valueOther;
  QRgb valueROther;
  QRgb valueGOther;
  QRgb valueBOther;

  int totalDistance = 0;

  for(int i = 0; i < 256; i++ )
  {
    value  = histogramEqualizationTable[i];
    valueR = qRed(value);
    valueG = qGreen(value);
    valueB = qBlue(value);

    valueOther  = other->histogramEqualizationTable[i];
    valueROther = qRed(valueOther);
    valueGOther = qGreen(valueOther);
    valueBOther = qBlue(valueOther);

    int deltaR = abs((int)(valueR - valueROther));
    int deltaG = abs((int)(valueG - valueGOther));
    int deltaB = abs((int)(valueB - valueBOther));

    totalDistance += deltaR;
    totalDistance += deltaG;
    totalDistance += deltaB;
  }

  return totalDistance;
}

