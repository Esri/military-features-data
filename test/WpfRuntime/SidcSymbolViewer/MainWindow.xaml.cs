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

using System.Windows;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.AdvancedSymbology;
using System;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.Client.Symbols;

namespace SidcSymbolViewer
{

    public partial class MainWindow : Window
    {
        Dictionary<string, SymbolProperties> sidc2SymbolProps = new Dictionary<string, SymbolProperties>();        

        public MainWindow()
        {
            // License setting and ArcGIS Runtime initialization is done in Application.xaml.cs.

            InitializeComponent();
            _symbolDictionary = new SymbolDictionary(SymbolDictionaryType.Mil2525C);

            IEnumerable<SymbolProperties> symbolProperties = _symbolDictionary.FindSymbols(null, null);
            var allSymbols = symbolProperties.ToList();
            foreach (var s in allSymbols)
            {
                string name = s.Name;
                if (string.IsNullOrEmpty(name) || !(s.Values.ContainsKey("SymbolID")))
                    continue;

                string symbolId = s.Values["SymbolID"].Replace('*','-');
                string geoType = s.Values["GeometryType"];

                // Need this table to be able to get the geometry type & other properties later
                sidc2SymbolProps[symbolId] = s;

                // To see symbols list:
                // System.Diagnostics.Trace.WriteLine(name + ":" + symbolId + ":" + geoType);
            }

            ListBox_Swatches.ItemsSource = swatches;
        }

        double x = 0;
        const double INITIAL_Y = 15000000;
        double y = INITIAL_Y;
        ObservableCollection<Image> swatches = new ObservableCollection<Image>();

  
        public static SymbolDictionary _symbolDictionary;

        private void Button_Click_Add_Points(object sender, RoutedEventArgs e)
        {
            AddSic("GHSPPT--------X");
            AddSic("GFMPNDA-------X");
            AddSic("GFSPPAT-------X");
            AddSic("GFMPNEB-------X");
            AddSic("GHGPGPUUB-----X");
            AddSic("GNMPNEC-------X");
            AddSic("GFGPGPPC------X");
            AddSic("GNMPNDB-------X");
            AddSic("GHGPAPD-------X");
            AddSic("GHMPNF--------X");
            AddSic("GHGPGPWG------X");
            AddSic("GFGPGPUUL-----X");
            AddSic("GNGPGPWM------X");
        }

        private void Button_Click_Add_Lines(object sender, RoutedEventArgs e)
        {
            AddSic("GFMPSL--------X");
            AddSic("GFMPOFA-------X");
            AddSic("GFGPPM--------X");
            AddSic("GFGPGLP-------X");
        }

        private void Button_Click_AddOne(object sender, RoutedEventArgs e)
        {
            var sic = TextBox_Sic.Text;

            try
            {
                AddSic(sic);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error occured: " + ee.ToString());
            }

        }

        private void AddSic(string sic)
        {
            const double INCREMENT = 2000000;
            const double HALF_INCREMENT = INCREMENT / 2.0;

            Message msg = new Message();
            msg.Id = Guid.NewGuid().ToString();
            msg.Add("_type", "position_report");
            msg.Add("_action", "update");
            msg.Add("_wkid", "3857");
            msg.Add("sic", sic);
            msg.Add("UniqueDesignation", "1");

            SymbolProperties code = null;

            if (sidc2SymbolProps.ContainsKey(sic))
                code = this.sidc2SymbolProps[sic];

            string geoType = "Point";
            if (code != null)
                geoType = code.Values["GeometryType"];

            if (geoType == "Point")
                msg.Add("_control_points", x.ToString() + "," + y.ToString());
            else if (geoType == "Line")
            {
               double x2 = x + HALF_INCREMENT;

               msg.Add("_control_points", 
                   x.ToString() + "," + y.ToString() + ";" +
                   x2.ToString() + "," + y.ToString());                    
            }
            else if (geoType == "Polygon") 
            {
               double yMinusHalf = y - HALF_INCREMENT;
               double xPlusHalf = x + HALF_INCREMENT;

               msg.Add("_control_points", 
                   x.ToString() + "," + y.ToString() + ";" +
                   xPlusHalf.ToString() + "," + y.ToString() + ";" +
                   xPlusHalf.ToString() + "," + yMinusHalf.ToString() + ";" +
                   x.ToString() + "," + yMinusHalf.ToString() + ";" +
                   x.ToString() + "," + y.ToString());
            }
            
            var imgSource = _symbolDictionary.GetSymbolImage(sic, 48, 48);
            var img = new Image();
            img.Source = imgSource;
            swatches.Add(img);
            ListBox_Swatches.SelectedIndex = swatches.Count - 1;
            _messageLayer.ProcessMessage(msg);

            if (!string.IsNullOrEmpty(geoType))
            {
                // var pt = new MapPoint(0, y);
                var pt = new MapPoint(x, y);

                var text = sic;
                if (code != null)
                    text = sic + " : " + code.Values["Category"] + " : " + code.Name ;

                // The main point of this App 
                // Compare the output of the Exported/Swatch version with the Map Version:
                // (They should be the same except for some exceptions for patterned lines)

                var tx = new TextSymbol() { Text = text };
                _graphicsLayer.Graphics.Add(new Graphic() { Symbol = tx, Geometry = pt });

                var pm = new PictureMarkerSymbol() { Source = imgSource, OffsetX = 128, OffsetY = 64 };
                _graphicsLayer.Graphics.Add(new Graphic() { Symbol = pm, Geometry = pt });
            }
            y -= INCREMENT;

            if (y < -INITIAL_Y)
            {
                y = INITIAL_Y;
                x = x + (3 * INCREMENT);
            }

        }

    }
}
