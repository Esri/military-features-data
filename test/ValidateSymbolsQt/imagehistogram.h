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
#ifndef IMAGEHISTOGRAM_H
#define IMAGEHISTOGRAM_H

#include <QImage>

/*
 * @brief Compares 2 Images using Histogram Equalization
 * http://en.wikipedia.org/wiki/Histogram_equalization
 *
 */
class ImageHistogram
{
public:

  enum MATCH { EXACT_MATCH=0, PROBABLE_MATCH=1, POSSIBLE_MATCH=2, MATCH_FAILED=3 };

  ImageHistogram(const QImage imageIn);

  MATCH CompareTo(ImageHistogram* other, double* confidence);

private:

  void calculateHistogram();
  void calculateHistogramEqualizationTable();
  int  compareHistogram(ImageHistogram* other);

  QImage image;

  QVector<int> mapR;
  QVector<int> mapG;
  QVector<int> mapB;

  QVector<double> mapNormalR;
  QVector<double> mapNormalG;
  QVector<double> mapNormalB;

  QVector<QRgb> histogramEqualizationTable;

};

#endif // IMAGEHISTOGRAM_H
