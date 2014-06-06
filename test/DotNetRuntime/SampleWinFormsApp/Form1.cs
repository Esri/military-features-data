/* Copyright 2014 Esri
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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

            pictureBoxExport.Image = new Bitmap(exportFilename);
        }

        private Esri.ArcGISRuntime.AdvancedSymbology.SymbolDictionary symbolDictionary = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.Initialize();
            symbolDictionary = new Esri.ArcGISRuntime.AdvancedSymbology.SymbolDictionary(Esri.ArcGISRuntime.Layers.SymbolDictionaryType.Mil2525c);

            string sidc = "SFGPUCI---AA---";
            CreateImageFromSic(sidc, 256);
        }

        private void CreateImageFromSic(string sic, int size)
        {

            System.Windows.Media.ImageSource imageSource = symbolDictionary.GetSymbolImage(sic, 256, 256);

            System.Windows.Media.Imaging.WriteableBitmap bi = imageSource as System.Windows.Media.Imaging.WriteableBitmap;

            SaveAsPng(sic + ".png", bi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sic = this.cbSymbolId.Text;
            if (!this.cbSymbolId.Items.Contains(sic))
                this.cbSymbolId.Items.Add(sic);

            const int size = 256;
            CreateImageFromSic(sic, size);
        }

        private void cbSymbolId_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
