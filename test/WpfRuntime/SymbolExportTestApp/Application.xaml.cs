/* Copyright 2013 Esri
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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.AdvancedSymbology;

namespace ArcGISWpfTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void SaveAsPng(string exportFilename, System.Windows.Media.Imaging.BitmapSource exportImage)
        {
            if (exportFilename == string.Empty)
                return;

            using (System.IO.FileStream stream = new System.IO.FileStream(exportFilename, System.IO.FileMode.Create))
            {
                System.Windows.Media.Imaging.PngBitmapEncoder encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(exportImage));
                encoder.Save(stream);
                stream.Close();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Before initializing the ArcGIS Runtime first 
            // set the ArcGIS Runtime license by providing the license string 
            // obtained from the License Viewer tool.

            // ArcGISRuntime.SetLicense("TODO: Place the License String in here");

            // Initialize the ArcGIS Runtime before any components are created.
            try
            {
                ArcGISRuntime.Initialize();

                string sidc = "SFGAUCIL--AA---";
                if (e.Args.Length != 0)
                {
                    sidc = e.Args[0];
                }

                SymbolDictionary sd = new SymbolDictionary(SymbolDictionaryType.Mil2525C);

                const int exportSize = 128;

                System.Windows.Media.ImageSource export = sd.GetSymbolImage(sidc, exportSize, exportSize);

                System.Windows.Media.Imaging.WriteableBitmap bi = export as System.Windows.Media.Imaging.WriteableBitmap;

                SaveAsPng(sidc + ".png", bi);

                // Exit application
                this.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }
    }
}
