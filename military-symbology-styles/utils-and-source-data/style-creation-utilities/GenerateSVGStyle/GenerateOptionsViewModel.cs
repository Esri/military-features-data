/*******************************************************************************
 * Copyright 2018 Esri
 * 
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 ******************************************************************************/
 
 using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Controls;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Internal.Mapping;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using ArcGIS.Desktop.Catalog;

namespace GenerateSVGStyle
{
    public class GenerateOptionsViewModel : ViewModelBase
    {
        public bool IsGenerateEnabled
        {
            get
            {
                return !(string.IsNullOrEmpty(CsvPath) || string.IsNullOrEmpty(StyleFolder)
                  || string.IsNullOrEmpty(StyleFileName)); 
            }
        }

        private string svgRootPath = "";
        public string SvgRootPath
        {
            get
            { return svgRootPath; }
            set
            {
                SetProperty(ref svgRootPath, value, () => SvgRootPath);
            }
        }

        private string csvPath = "";
        public string CsvPath
        {
            get
            { return csvPath; }
            set
            {
                SetProperty(ref csvPath, value, () => CsvPath);
                NotifyPropertyChanged(() => IsGenerateEnabled);
            }
        }

        private string styleFolder = "";
        public string StyleFolder
        {
            get
            { return styleFolder; }
            set
            {
                SetProperty(ref styleFolder, value, () => StyleFolder);
                NotifyPropertyChanged(() => IsGenerateEnabled);
            }
        }

        private string styleFileName = "StyleFile.stylx";
        public string StyleFileName
        {
            get
            { return styleFileName; }
            set
            {
                SetProperty(ref styleFileName, value, () => StyleFileName);
                NotifyPropertyChanged(() => IsGenerateEnabled);
            }
        }

        public string StylePath
        {
            get
            {
                if (string.IsNullOrEmpty(StyleFolder)
                  || string.IsNullOrEmpty(StyleFileName))
                    return string.Empty;

                string styleFile = System.IO.Path.Combine(StyleFolder, StyleFileName);
                // ensure it ends with ".stylx"

                if (!styleFile.ToLower().EndsWith(".stylx"))
                    styleFile += ".stylx";

                return styleFile;
            }
        }

        private ICommand _browseFileCmd = null;
        public ICommand BrowseFileCommand
        {
            get { return _browseFileCmd ?? (_browseFileCmd = new RelayCommand(() => this.BrowseFileName())); }
        }

        private ICommand _browseSvgFolderCmd = null;
        public ICommand BrowseSvgFolderCmd
        {
            get { return _browseSvgFolderCmd ?? (_browseSvgFolderCmd = new RelayCommand(() => this.BrowseSvgFolder())); }
        }

        private ICommand _browseStyleFolderCmd = null;
        public ICommand BrowseStyleFolderCommand
        {
            get { return _browseStyleFolderCmd ?? (_browseStyleFolderCmd = new RelayCommand(() => this.BrowseStyleFolder())); }
        }

        private ICommand _mergeStylesCmd = null;
        public ICommand MergeStylesCommand
        {
            get { return _mergeStylesCmd ?? (_mergeStylesCmd = new RelayCommand(() => this.MergeStyles())); }
        }

        private ICommand _browseStyle1Cmd = null;
        public ICommand BrowseStyle1Command
        {
            get { return _browseStyle1Cmd ?? (_browseStyle1Cmd = new RelayCommand(() => this.BrowseStyle1())); }
        }

        private ICommand _browseStyle2Cmd = null;
        public ICommand BrowseStyle2Command
        {
            get { return _browseStyle2Cmd ?? (_browseStyle2Cmd = new RelayCommand(() => this.BrowseStyle2())); }
        }

        private ICommand _executeSqlScriptCmd = null;
        public ICommand ExecuteSqlScriptCommand
        {
            get { return _executeSqlScriptCmd ?? (_executeSqlScriptCmd = new RelayCommand(() => this.ExecuteSqlScript())); }
        }

        private string BrowseItem(string itemFilter, string initialPath = "")
        {
            Module1.Current.GenerateOptionsWindow.Topmost = false;

            string folderPath = "";

            if (string.IsNullOrEmpty(initialPath))
            {
                var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                initialPath = Path.Combine(System.IO.Path.Combine(myDocs, "ArcGIS"));
            }

            OpenItemDialog pathDialog = new OpenItemDialog()
            {
                Title = "Select Folder",
                InitialLocation = initialPath,
                MultiSelect = false,
                Filter = itemFilter,
            };

            bool? ok = pathDialog.ShowDialog();
            if ((ok == true) && (pathDialog.Items.Count() > 0))
            {
                IEnumerable<Item> selectedItems = pathDialog.Items;
                folderPath = selectedItems.First().Path;
            }

            // IMPORTANT/WORKAROUND: must set Addin Form back to topmost
            Module1.Current.GenerateOptionsWindow.Topmost = true;

            return folderPath;
        }

        private void BrowseSvgFolder()
        {
            SvgRootPath = BrowseItem(ItemFilters.folders);

            // Check against a Well Known File:
            string expectedFile = "BoundingOctagon.svg";
            string expectedPath = System.IO.Path.Combine(SvgRootPath, expectedFile);

            if (!File.Exists(expectedPath))
            {
                MessageBoxResult result =
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                    "Are you sure this path is correct? \n" + expectedPath,
                    "Recheck Path", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }           
        }

        private void BrowseStyleFolder()
        {
            StyleFolder = BrowseItem(ItemFilters.folders);
        }

        private ICommand _generateCommand = null;
        public ICommand GenerateCommand
        {
            get
            {
                if (_generateCommand == null)
                {
                    _generateCommand = new RelayCommand(GenerateStyleFromCsvFile, () =>
                    {
                        return CsvPath != null && StylePath != null;
                    });
                }
                return _generateCommand;
            }
        }

        private void BrowseFileName()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() != true)
                return;

            var filename = openFileDialog.FileName;
            CsvPath = filename;

            NotifyPropertyChanged(() => CsvPath);
        }

        public bool IsMergeEnabled
        {
            get
            {
                // Enable if Styles are set and exist
                return !string.IsNullOrEmpty(SourceStyleToMerge) && !string.IsNullOrEmpty(StyleToMergeInto) &&
                    !string.IsNullOrEmpty(MergedStyleName) && (SourceStyleToMerge != StyleToMergeInto) &&
                    File.Exists(SourceStyleToMerge) && File.Exists(StyleToMergeInto);
            }
        }

        public void BrowseStyle1()
        {
            SourceStyleToMerge = BrowseItem(ItemFilters.styleFiles);

            NotifyPropertyChanged(() => SourceStyleToMerge);
        }

        public void BrowseStyle2()
        {
            StyleToMergeInto = BrowseItem(ItemFilters.styleFiles);

            NotifyPropertyChanged(() => StyleToMergeInto);
        }

        private string sourceStyleToMerge = "SourceStyle.stylx";
        public string SourceStyleToMerge
        {
            get
            { return sourceStyleToMerge; }
            set
            {
                SetProperty(ref sourceStyleToMerge, value, () => SourceStyleToMerge);
                NotifyPropertyChanged(() => IsMergeEnabled);
                NotifyPropertyChanged(() => IsExecuteSqlScriptEnabled);
            }
        }

        private string styleToMergeInto = "StyleToMergeInto.stylx";
        public string StyleToMergeInto
        {
            get
            { return styleToMergeInto; }
            set
            {
                SetProperty(ref styleToMergeInto, value, () => StyleToMergeInto);
                NotifyPropertyChanged(() => IsMergeEnabled);
            }
        }
        
        private string mergedStyleName = "MergedStyle.stylx";
        public string MergedStyleName
        {
            get
            { return mergedStyleName; }
            set
            {
                SetProperty(ref mergedStyleName, value, () => MergedStyleName);
                NotifyPropertyChanged(() => IsMergeEnabled);
            }
        }

        private async Task RemoveStyleItem(string styleFileFullPath)
        {
            await QueuedTask.Run(() =>
            {
                var styles = Project.Current.GetItems<StyleProjectItem>();

                //Get the style in the project
                StyleProjectItem style = styles.FirstOrDefault(x => x.Path == styleFileFullPath);

                if (style != null)
                {
                    // remove it, if it was found
                    Project.Current.RemoveStyle(styleFileFullPath);
                }
            });
        }

        private async Task<StyleProjectItem> GetStyleItem(string styleFileFullPath)
        {
            StyleProjectItem style = null;

            if (!File.Exists(styleFileFullPath))
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                    "Style File does not exist: \n" + styleFileFullPath,
                    "Style File Missing", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (Project.Current != null)
                {
                    await QueuedTask.Run(() =>
                    {
                        //Get all styles in the project
                        var styles = Project.Current.GetItems<StyleProjectItem>();

                        //Get the style in the project
                        style = styles.FirstOrDefault(x => x.Path == styleFileFullPath);

                        if (style == null)
                        {
                            // add it, if it wasn't found
                            Project.Current.AddStyle(styleFileFullPath);

                            // then check again for style (just in case)
                            styles = Project.Current.GetItems<StyleProjectItem>();
                            style = styles.FirstOrDefault(x => x.Path == styleFileFullPath);
                        }
                    });
                }
            }

            return style;
        }

        private async void MergeStyles()
        {
            var result = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                "Merging Styles: \n" + SourceStyleToMerge + "\n Into: \n" +
                StyleToMergeInto,
                "Merge Styles", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (result.ToString() != "OK")
                return;

            string mergedStylePath = Path.GetDirectoryName(StyleToMergeInto);
            string mergedStyleFile = System.IO.Path.Combine(mergedStylePath, MergedStyleName);

            if (File.Exists(mergedStyleFile))
            {
                var result2 = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                    "Style already exists: \n" + mergedStyleFile + "\n Overwrite?",
                    "Overwrite Style?", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

                if (result2.ToString() != "OK")
                    return;
            }

            System.IO.File.Copy(StyleToMergeInto, mergedStyleFile, true);
           
            var styleSourceStyleToMerge = await GetStyleItem(SourceStyleToMerge);
            var styleMergedStyleFile = await GetStyleItem(mergedStyleFile);

            int numSymbolsAdded = 0;
            await QueuedTask.Run(() =>
            {
                IList<SymbolStyleItem> sourceSymbols = styleSourceStyleToMerge.SearchSymbols(StyleItemType.PointSymbol, string.Empty);

                foreach (var styleItem in sourceSymbols)
                {
                    styleMergedStyleFile.AddItem(styleItem);

                    System.Diagnostics.Debug.WriteLine("Merging item: " + styleItem.Name);

                    numSymbolsAdded++;
                }
            });

            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                "Merge Complete: \n" + mergedStyleFile +
                "Number of Symbols Added: " + numSymbolsAdded,
                "Merge Styles", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public bool IsExecuteSqlScriptEnabled
        {
            get
            {
                // Enable if Styles are set and exist
                return !string.IsNullOrEmpty(SourceStyleToMerge) && 
                    File.Exists(SourceStyleToMerge);
            }
        }

        public string SqliteCommand
        {
            get
            {
                string addinLocation = AddinAssemblyLocation();

                string exeLocation = Path.Combine(addinLocation, "sqlite", "sqlite3.exe");

                return exeLocation;
            }
        }
     
        public string SqliteCommandScript
        {
            get
            {
                string addinLocation = AddinAssemblyLocation();

                string exeLocation = Path.Combine(addinLocation, "sqlite", "SqliteStyleCommands.sql");

                return exeLocation;
            }
        }

        public async void ExecuteSqlScript()
        {
            string sqliteDb = SourceStyleToMerge;

            var result = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                "Execute Sql Command?",
                "Execute Sql Command?", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

            if (result.ToString() != "OK")
                return;

            if (!File.Exists(SqliteCommand) || !File.Exists(SqliteCommandScript) ||
                !File.Exists(sqliteDb))
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                                "Required File Missing",
                                "Required File Missing", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                // Remove the style from the project if there (to avoid the style being locked)
                await RemoveStyleItem(sqliteDb);

                // Run Sql Command
                // Command format: "%SQLITE_PATH_AND_EXE%" "%SQLITE_DATABASE%" < "%SQLITE_SCRIPT%"

                // WORKAROUND: Command only running from Batch File:
                string addinLocation = AddinAssemblyLocation();
                string batFile = Path.Combine(addinLocation, "sqlite", "SqliteBatch.bat");
                System.IO.StreamWriter fileOut = new System.IO.StreamWriter(batFile, false);
                fileOut.WriteLine(SqliteCommand + " " + sqliteDb + " < " + SqliteCommandScript + " > BatchOutput.txt 2>&1");
                fileOut.Close();

                System.Diagnostics.ProcessStartInfo pStart = new System.Diagnostics.ProcessStartInfo(batFile);
                // Note: WORKAROUND - Setting these didn't seem to work - had to run from batch
                // pStart.Arguments = sqliteDb + " < " + SqliteCommandScript;
                pStart.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pStart.WorkingDirectory = addinLocation;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();

                proc.StartInfo = pStart;
                proc.Start();
                proc.WaitForExit();
                proc.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Unknown exception while updating style",
                    "Update style FAILURE!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                                    "Sqlite Label Command Complete",
                                    "Sqlite Command Complete", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private async void GenerateStyleFromCsvFile(object parameter)
        {
            // CloseWindow();

            int numSymbolsAttempted = 0;
            int numSymbolsLoaded = 0;

            await QueuedTask.Run(() => {
            try
            {
                if (string.IsNullOrEmpty(StylePath))
                {

                }

                if (File.Exists(StylePath))
                {
                    MessageBoxResult result =
                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                            "Style File Already Exists - Do you Want to Overwrite? \n" + StylePath,
                            "Style Exists - Overwrite?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (result.ToString() == "Yes")
                        {
                            File.Delete(StylePath);
                        }
                        else
                        {
                            // Don't delete/stop                       
                            return;
                        }
                }

                string fullStylePath = Path.GetDirectoryName(StylePath);
                if (!Directory.Exists(fullStylePath))
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                        "Style Path Does Not Exist: \n" + fullStylePath,
                        "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!StylePath.ToLower().EndsWith(".stylx"))
                {
                    MessageBoxResult result =
                        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                            "Style File Name does not end with .stylx - Continue? \n" + StylePath,
                            "Continue?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result.ToString() != "Yes")
                        return;
                }

                // Warning: must be mobile style
                // StyleHelper.CreateStyle(Project.Current, StylePath);
                // 
                StyleHelper.CreateMobileStyle(Project.Current, StylePath);
                StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == StylePath);

                if (!File.Exists(CsvPath))
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                        "CSV File Does Not Exist: " + CsvPath, 
                        "Invalid Csv", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (!string.IsNullOrEmpty(SvgRootPath) && !Directory.Exists(SvgRootPath))
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                        "SVG Root Folder Does Not Exist: " + SvgRootPath,
                        "Invalid SVG Root Folder", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                // Test File:
                // string csvTableFullPath = @"C:\MyFiles\Work\Esri\ProSDK\TestImportStyleCsv\App6D-Added-Icons.csv";

                CsvToTableMaker csvTable = new CsvToTableMaker();
                    csvTable.LoadTable(CsvPath);
                    DataTable symbolTable = csvTable.Table;

                // Output input table if needed
                // csvTable.DebugOutput();

                // Make sure initialized properly
                bool tableInitialized = (symbolTable != null) &&
                    (symbolTable.Rows != null) && (symbolTable.Columns != null) &&
                    (symbolTable.Rows.Count > 0) && (symbolTable.Columns.Count > 0);

                ////////////////////////////////////////////////////////
                // Check required columns exist
                Dictionary<string, bool> requiredColumnsExists = 
                    new Dictionary<string, bool>()
                    {
                        { "filePath", false },
                        { "pointSize", false },
                        { "styleItemName", false },
                        { "styleItemCategory", false },
                        { "styleItemTags", false },
                        { "styleItemUniqueId", false },
                    };

                foreach (DataColumn column in symbolTable.Columns)
                {
                    var colName = column.ColumnName;
                    if (requiredColumnsExists.ContainsKey(colName))
                        requiredColumnsExists[colName] = true;
                }

                bool doRequiredColumnsExist = true;
                foreach (KeyValuePair<string, bool> pair in requiredColumnsExists)
                {
                    if (pair.Value == false)
                    {
                        doRequiredColumnsExist = false;
                        break;
                    }
                }

                if (!doRequiredColumnsExist)
                {
                    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                        "CSV File Does Not Have Expected Columns: " + CsvPath,
                        "Invalid Csv", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                ////////////////////////////////////////////////////////

                var results = from row in symbolTable.AsEnumerable()
                                select row;

                foreach (DataRow row in results)
                {
                    // filePath	pointSize, styleItemName, styleItemCategory, styleItemTags	styleItemUniqueId	styleItemGeometryType	notes
                    string svgFile = row["filePath"] as string;
                    int pointSize = int.Parse(row["pointSize"] as string);
                    string styleItemName = row["styleItemName"] as string;
                    string styleItemCategory = row["styleItemCategory"] as string;
                    string styleItemTags = row["styleItemTags"] as string;
                    string styleItemUniqueId = row["styleItemUniqueId"] as string;
                    string styleItemGeometryType = row["styleItemGeometryType"] as string;

                    // Expand this string if present: 
                    string svgPathToReplace = "{Symbols_Root}";
                    if (svgFile.Contains(svgPathToReplace) && !string.IsNullOrEmpty(SvgRootPath))
                    {
                        svgFile = svgFile.Replace(svgPathToReplace, SvgRootPath);
                    }

                    // WORKAROUND: old files contained ".emf" instead of ".svg" 
                    // Replace ".emf" with ".svg"  string if present: 
                    string emfStringToReplace = ".emf";
                    if (svgFile.EndsWith(emfStringToReplace))
                    {
                        svgFile = svgFile.Replace(emfStringToReplace, ".svg");
                    }

                    if (!File.Exists(svgFile))
                    {
                        var result = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Warning: SVG File not found: " + svgFile,
                            "SVG File Not Found!", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

                        if (result.ToString() == "OK")
                            continue;
                        else
                            break;
                    }

                    try
                    {
                        numSymbolsAttempted++;

                        SymbolStyleItem styleItem = new SymbolStyleItem();

                        CIMMarker svgMarker =
                                SymbolFactory.Instance.ConstructMarkerFromFile(svgFile);

                        svgMarker.Size = pointSize;
                        styleItem.Name = styleItemName;
                        styleItem.Category = styleItemCategory;
                        styleItem.Tags = styleItemTags;
                        styleItem.Key = styleItemUniqueId;
                        styleItem.Symbol = SymbolFactory.Instance.ConstructPointSymbol(svgMarker);

                        styleItem.PatchWidth = pointSize;
                        styleItem.PatchHeight = pointSize;

                        // set the style item thumbnail - working?
                        StyleItem.GeneratePreview(styleItem, pointSize, pointSize);

                        style.AddItem(styleItem);

                        numSymbolsLoaded++;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                        MessageBoxResult result = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Failed to create symbol, failed on symbol#: " + numSymbolsAttempted +
                            ", File: " + svgFile,
                            "Symbol Creation FAILURE!", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                        if (result.ToString() == "Cancel")
                            throw new Exception("Operation Cancelled");
                    }
                } // foreach
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Failed to create style, stopped on symbol#: " + numSymbolsAttempted,
                    "Style File Generation FAILURE!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            });

            ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
                "Added: " + numSymbolsLoaded + " Symbols out of " + numSymbolsAttempted +
                " attempted to style: \n" + StylePath,
                "Style File Generation Complete", MessageBoxButton.OK, MessageBoxImage.Exclamation);    
        }

        //    private async void GenerateStyleFromFolder(object parameter)
        //    {
        //      CloseWindow();
        //      await QueuedTask.Run(() =>
        //       {
        //         try
        //         {
        //           if (File.Exists(pathStyle))
        //             File.Delete(pathStyle);

        //           int num_symbols = 0;
        //           const int num_symbols_to_add = 15;

        //           StyleHelper.CreateStyle(Project.Current, pathStyle);
        //           StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == pathStyle);

        //           IEnumerable<string> svgPaths = Directory.EnumerateFiles(pathCsv, "*.svg", SearchOption.AllDirectories);

        //           foreach (string svgPath in svgPaths)
        //           {            
        //             SymbolStyleItem styleItem = new SymbolStyleItem();

        //             CIMMarker svgMarker = 
        //                   SymbolFactory.Instance.ConstructMarkerFromFile(svgPath);

        //             svgMarker.Size = 64;
        //             string nameKeyBase = Path.GetFileNameWithoutExtension(svgPath);
        //             styleItem.Name = nameKeyBase + "_Name";
        //             styleItem.Key = nameKeyBase + "_Key";

        //             styleItem.Symbol = SymbolFactory.Instance.ConstructPointSymbol(svgMarker);

        //             styleItem.PatchWidth = 64;
        //             styleItem.PatchHeight = 64;

        //             // set the style item thumbnail - currently not working?
        //             StyleItem.GeneratePreview(styleItem, 64, 64);

        //             style.AddItem(styleItem);

        //             if (num_symbols_to_add > 0 && ++num_symbols == num_symbols_to_add)
        //               break;
        //           }
        //         }
        //         catch (Exception ex)
        //         {
        //           System.Diagnostics.Trace.WriteLine(ex.Message);
        //         }
        //       });

        //return;

        //        await QueuedTask.Run(() =>
        //        {
        //            try
        //            {
        //                //update point symbols
        //                StyleProjectItem style = Project.Current.GetItems<StyleProjectItem>().First(x => x.Path == pathStyle);
        //                var ptSymbols = style.SearchSymbols(StyleItemType.PointSymbol, "");

        //                foreach (SymbolStyleItem s in ptSymbols)
        //                {
        //                    CIMPointSymbol symbol = (CIMPointSymbol)s.Symbol;

        //                    if (symbol == null)
        //                        continue;

        //                    symbol.SetSize(64.0);

        //                    s.Symbol = symbol;
        //                    style.UpdateItem(s);

        //                    //try
        //                    //{
        //                    //    var lyrs = symbol.SymbolLayers;

        //                    //    foreach (CIMVectorMarker x in lyrs)
        //                    //    {
        //                    //        if (x.ScaleSymbolsProportionally == false)
        //                    //            x.ScaleSymbolsProportionally = true;
        //                    //    }
        //                    //    symbol.SymbolLayers = lyrs;
        //                    //    s.Symbol = symbol;
        //                    //    projectStyleItem.UpdateItem(s);
        //                    //}
        //                    //catch (Exception ex2)
        //                    //{
        //                    //}
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Trace.WriteLine(ex.Message);
        //            }
        //        });
        //        }

    internal static string AddinAssemblyLocation()
    {
        var asm = System.Reflection.Assembly.GetExecutingAssembly();
        return System.IO.Path.GetDirectoryName(
                            Uri.UnescapeDataString(
                                    new Uri(asm.CodeBase).LocalPath));
    }

    internal static void CloseWindow()
    {
      ProWindow window = Application.Current.Windows.OfType<ProWindow>().SingleOrDefault(x => x.IsActive);
      window.Close();
    }
  }
}
