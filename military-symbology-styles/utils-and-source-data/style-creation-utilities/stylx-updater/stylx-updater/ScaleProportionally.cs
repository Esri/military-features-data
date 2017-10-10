/* Copyright 2015 Esri
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
 * limitations under the License. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;

namespace StylxUpdater
{
    /// <summary>
    /// A sample Pro plugin to demostrate batch updating of Pro style symbols
    /// </summary>
    internal class ScaleProportionally : Button
    {
        protected async override void OnClick()
        {
            // To use this utility, you need to add the style that you want to update to your ArcGIS Pro project
            // If necessary and not alreadly listed, update the line below to include the name of the style that
            //    you want to update 
            // Note: no ".stylx" extension 

            List<string> stylxFiles = new List<string>() { "mil2525d", "mil2525c", "app6b" };

            string stylxUpdatedNames = string.Empty;

            foreach (string stylxToUpdateName in stylxFiles)
            {
                StyleProjectItem projectStyleItem = null;
                try
                {
                    projectStyleItem = Project.Current.GetItems<StyleProjectItem>().First(x => x.Name == stylxToUpdateName);
                }
                catch (Exception ex)
                {
                    projectStyleItem = null;
                }

                if (projectStyleItem == null)
                {
                    continue;
                }

                stylxUpdatedNames += stylxToUpdateName + ";";

                //update point symbols
                var ptSymbols = await projectStyleItem.SearchSymbolsAsync(StyleItemType.PointSymbol, "");
                await QueuedTask.Run(() =>
                {
                    foreach (SymbolStyleItem s in ptSymbols)
                    {
                        CIMPointSymbol symbol = (CIMPointSymbol)s.Symbol;

                        if (symbol == null)
                            continue;

                        try 
                        {
                            var lyrs = symbol.SymbolLayers;

                            foreach (CIMVectorMarker x in lyrs)
                            {
                                if (x.ScaleSymbolsProportionally == false)
                                    x.ScaleSymbolsProportionally = true;
                            }
                            symbol.SymbolLayers = lyrs;
                            s.Symbol = symbol;
                            projectStyleItem.UpdateItem(s);
                        }
                        catch (Exception ex2)
                        {
                        }
                    }
                });

                //update line symbols
                var lineSymbols = await projectStyleItem.SearchSymbolsAsync(StyleItemType.LineSymbol, "");
                await QueuedTask.Run(() =>
                {
                    foreach (SymbolStyleItem s in lineSymbols)
                    {
                        try
                        {
                            if (s.Symbol is CIMLineSymbol)
                            {
                                CIMLineSymbol symbol = (CIMLineSymbol)s.Symbol;
                                var lyrs = symbol.SymbolLayers;
                                foreach (CIMSymbolLayer lyr in lyrs)
                                {
                                    CIMVectorMarker x = lyr as CIMVectorMarker;
                                    if (x != null)
                                    {
                                        if (x.ScaleSymbolsProportionally == false)
                                            x.ScaleSymbolsProportionally = true;
                                    }
                                }
                                symbol.SymbolLayers = lyrs;
                                s.Symbol = symbol;
                            }
                            else if (s.Symbol is CIMPointSymbol)
                            {
                                CIMPointSymbol symbol = (CIMPointSymbol)s.Symbol;
                                var lyrs = symbol.SymbolLayers;
                                foreach (CIMSymbolLayer lyr in lyrs)
                                {
                                    CIMVectorMarker x = lyr as CIMVectorMarker;
                                    if (x != null)
                                    {
                                        if (x.ScaleSymbolsProportionally == false)
                                            x.ScaleSymbolsProportionally = true;
                                    }
                                }
                                symbol.SymbolLayers = lyrs;
                                s.Symbol = symbol;
                                s.ItemType = StyleItemType.LineSymbol;
                            }

                            projectStyleItem.UpdateItem(s);
                        }
                        catch (Exception ex3)
                        {

                        }

                    }
                });

                //update polygon symbols
                var polygonSymbols = await projectStyleItem.SearchSymbolsAsync(StyleItemType.PolygonSymbol, "");
                await QueuedTask.Run(() =>
                {
                    foreach (SymbolStyleItem s in polygonSymbols)
                    {
                        try
                        {
                            if (s.Symbol is CIMPolygonSymbol)
                            {
                                CIMPolygonSymbol symbol = (CIMPolygonSymbol)s.Symbol;
                                var lyrs = symbol.SymbolLayers;
                                foreach (CIMSymbolLayer lyr in lyrs)
                                {
                                    CIMVectorMarker x = lyr as CIMVectorMarker;
                                    if (x != null)
                                    {
                                        if (x.ScaleSymbolsProportionally == false)
                                            x.ScaleSymbolsProportionally = true;
                                    }
                                }
                                symbol.SymbolLayers = lyrs;
                                s.Symbol = symbol;
                            }
                            else if (s.Symbol is CIMPointSymbol)
                            {
                                CIMPointSymbol symbol = (CIMPointSymbol)s.Symbol;
                                var lyrs = symbol.SymbolLayers;
                                foreach (CIMSymbolLayer lyr in lyrs)
                                {
                                    CIMVectorMarker x = lyr as CIMVectorMarker;
                                    if (x != null)
                                    {
                                        if (x.ScaleSymbolsProportionally == false)
                                            x.ScaleSymbolsProportionally = true;
                                    }
                                }
                                symbol.SymbolLayers = lyrs;
                                s.Symbol = symbol;
                                s.ItemType = StyleItemType.PolygonSymbol;
                            }

                            projectStyleItem.UpdateItem(s);
                        }
                        catch (Exception ex3)
                        {
                        }

                    }
                });

            } // end foreach

            if (string.IsNullOrEmpty(stylxUpdatedNames))
            {
                string stylxs = string.Join(";", stylxFiles);
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Could not find any styles: " + stylxs + " in Project");
            }
            else
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Stylx Update DONE, styles updated:" + stylxUpdatedNames);
            }

        } // OnClick

    }
}
