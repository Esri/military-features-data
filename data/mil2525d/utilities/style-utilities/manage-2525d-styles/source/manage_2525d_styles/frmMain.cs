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
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace Manage2525DStyles
{
    public partial class frmMain : Form
    {
        private readonly BackgroundWorker _bw = new BackgroundWorker();

        private static string _strMessage;

        private const string _strConfig = "manage2525d.config";
        private const string _strJMSMLSymbols = "\\svg\\MIL_STD_2525D_Symbols";
        private const string _strConvertBatch = "\\data\\mil2525d\\utilities\\style-utilities\\image-conversion-utilities\\ConvertTree-SVGtoEMF.bat";
        private const string _strCreateStylesBatch = "\\CreateAllMilitaryStyles.bat";
        private const string _strCSVFiles = "\\samples\\imagefile_name_category_tags";
        private const string _strScratchConvertBatch = ".\\ConvertTree-SVGtoEMF.bat";
        private const string _strScratchCreateStylesBatch = ".\\CreateAllMilitaryStyles.bat";
        private const string _strScratchCSVs = "\\CSVInputData";
        private const string _strStyleOutput = "\\StyleOutputData";
        private const string _strStylxFile = "\\Military-2525Delta-All-Icons.stylx";
        private const string _strSqlScript = "\\data\\mil2525d\\utilities\\style-utilities\\merge-stylx-utilities\\SqliteMergeStylx.sql";
        private const string _strScratchSqlScript = "\\source\\manage_2525d_styles\\bin\\Debug\\SqliteMergeStylx.sql";
        private const string _strTemplateStylx = "\\data\\mil2525d\\core_data\\stylxfiles\\mil2525d-lines-areas-labels-base-template.stylx";
        private const string _strScratchTemplateStylx = "\\mil2525d.stylx";
        private const string _strMergeBatch = ".\\MergeStylx.bat";

        // Example values for these strings are here for instructionsl purposes only.  The XML config file now handles
        // these values for the user.

        private string _strMyHome; // = "C:\\Users\\andy750\\Documents\\GitHub\\manage-2525d-styles";
        private string _strMyMFD; // = "C:\\Users\\andy750\\Documents\\GitHub\\military-features-data";
        private string _strMyJMSML; // = "C:\\Users\\andy750\\Documents\\GitHub\\joint-military-symbology-xml";
        private string _strMySymbols; // = "C:\\Symbols\\MIL_STD_2525D_Symbols";
        private string _strMyInkscape; // = "C:\\Inkscape\\inkscape.com";
        private string _strMyCSVtoStyle; // = "C:\\Users\\andy750\\Documents\\GitHub\\military-features-data\\data\\mil2525d\\utilities\\style-utilities\\style-file-utilities\\csv2ArcGISStyleDeployment";
        private string _strMySQLite; // = "C:\\SQLite3\\sqlite3.exe";
        private string _strMyStylx; // = "C:\\Users\\andy750\\Documents\\ArcGIS\\Projects\\MyProject\\Military-2525Delta-All-Icons.stylx";
        private string _strMyPro; // = "C:\\Program Files\\ArcGIS\\Pro\\bin\\ArcGISPro.exe";
        private string _strMyProject; // = "C:\\Users\\andy750\\Documents\\ArcGIS\\Projects\\MyProject\\MyProject.aprx";

        private ManageStylesConfig _config;

        public frmMain()
        {
            InitializeComponent();

            this.FormClosing += frmMain_FormClosing;

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
            toolStripProgressBar1.MarqueeAnimationSpeed = 0;

            // Initialize button color to provide some progress feedback

            btnConvertGraphics.BackColor = Color.PaleGreen;
            btnCreateStyle.BackColor = Color.IndianRed;
            btnImportIntoPro .BackColor = Color.IndianRed;
            btnMergeStylx.BackColor = Color.IndianRed;

            //
            // Deserialize the configuration xml to get the location of the needed data and tools
            //

            XmlSerializer serializer = new XmlSerializer(typeof(ManageStylesConfig));

            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            string s = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            s = Path.Combine(s, _strConfig);

            if (File.Exists(s))
            {
                using (FileStream fs = new FileStream(s, FileMode.Open, FileAccess.Read))
                {
                    if (fs.CanRead)
                    {
                        try
                        {
                            _config = (ManageStylesConfig)serializer.Deserialize(fs);

                            _strMyHome = _config.Home;
                            _strMyMFD = _config.MilitaryFeaturesDataHome;
                            _strMyJMSML = _config.JMSMLHome;
                            _strMySymbols = _config.GraphicHome;
                            _strMyInkscape = _config.InkscapeHome;
                            _strMyCSVtoStyle = _config.CSVtoStyleHome;
                            _strMySQLite = _config.SQLiteHome;
                            _strMyStylx = _config.StylxHome;
                            _strMyPro = _config.ProHome;
                            _strMyProject = _config.ProjectHome;

                            txtHome.Text = _strMyHome;
                            txtMFD.Text = _strMyMFD;
                            txtJMSML.Text = _strMyJMSML;
                            txtGraphics.Text = _strMySymbols;
                            txtInkscape.Text = _strMyInkscape;
                            txtCSVtoStyle.Text = _strMyCSVtoStyle;
                            txtSQLite.Text = _strMySQLite;
                            txtStylx.Text = _strMyStylx;
                            txtPro.Text = _strMyPro;
                            txtProject.Text = _strMyProject;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);

                            System.Environment.Exit(0);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Can't read the config file.");

                        System.Environment.Exit(0);
                    }
                }
            }
            else
            {
                MessageBox.Show("Config file is missing.");

                System.Environment.Exit(0);
            }
        }

        private void frmMain_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //
            // Serialize the configuration xml to save the locations of stuff
            //

            XmlSerializer serializer = new XmlSerializer(typeof(ManageStylesConfig));

            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            string s = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            s = Path.Combine(s, _strConfig);

            if (File.Exists(s))
            {
                // Deleting the existing config file, to work around a possible corruption issue

                File.Delete(s);
            }

            // Now create a new config file

            using (FileStream fs = new FileStream(s, FileMode.Create, FileAccess.Write))
            {
                if (fs.CanWrite)
                {
                    try
                    {
                        _config.Home = _strMyHome;
                        _config.MilitaryFeaturesDataHome = _strMyMFD;
                        _config.JMSMLHome = _strMyJMSML;
                        _config.GraphicHome = _strMySymbols;
                        _config.InkscapeHome = _strMyInkscape;
                        _config.CSVtoStyleHome = _strMyCSVtoStyle;
                        _config.SQLiteHome = _strMySQLite;
                        _config.StylxHome = _strMyStylx;
                        _config.ProHome = _strMyPro;
                        _config.ProjectHome = _strMyProject;

                        serializer.Serialize(fs, _config);

                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            
            //In case windows is trying to shut down, don't hold the process up
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (this.DialogResult == DialogResult.Cancel)
            {
                // Assume that X has been clicked and act accordingly.
                // Confirm user wants to close
                switch (MessageBox.Show(this, "Are you sure?", "Do you still want ... ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    //Stay on this form
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            // Set the location for working/scratch/output files

            if (!Directory.Exists(_strMyHome))
                _strMyHome = AppDomain.CurrentDomain.BaseDirectory;

            folderBrowserDialog1.SelectedPath = _strMyHome;

            DialogResult result = folderBrowserDialog1.ShowDialog();

            txtHome.Text = folderBrowserDialog1.SelectedPath;
            _strMyHome = txtHome.Text;
        }

        private void btnMFD_Click(object sender, EventArgs e)
        {
            // Set the location of the Military Features Data repo

            if (!Directory.Exists(_strMyMFD))
                _strMyMFD = AppDomain.CurrentDomain.BaseDirectory;

            folderBrowserDialog1.SelectedPath = _strMyMFD;
            
            DialogResult result = folderBrowserDialog1.ShowDialog();

            txtMFD.Text = folderBrowserDialog1.SelectedPath;
            _strMyMFD = txtMFD.Text;
        }

        private void btnJMSML_Click(object sender, EventArgs e)
        {
            // Set the location of the JMSML repo

            if (!Directory.Exists(_strMyJMSML))
                _strMyJMSML = AppDomain.CurrentDomain.BaseDirectory;

            folderBrowserDialog1.SelectedPath = _strMyJMSML;

            DialogResult result = folderBrowserDialog1.ShowDialog();

            txtJMSML.Text = folderBrowserDialog1.SelectedPath;
            _strMyJMSML = txtJMSML.Text;
        }

        private void btnGraphics_Click(object sender, EventArgs e)
        {
            // Set the spaceless path to the place where svg and emf files will be converted

            if (!Directory.Exists(_strMySymbols))
                _strMySymbols = AppDomain.CurrentDomain.BaseDirectory;

            folderBrowserDialog1.SelectedPath = _strMySymbols;
            txtGraphics.Text = folderBrowserDialog1.SelectedPath;

            DialogResult result = folderBrowserDialog1.ShowDialog();

            txtGraphics.Text = folderBrowserDialog1.SelectedPath;
            _strMySymbols = txtGraphics.Text;
        }

        private void btnInkscape_Click(object sender, EventArgs e)
        {
            // Set the path to the Inkscape command line program

            openFileDialog1.FileName = _strMyInkscape;
            txtInkscape.Text = openFileDialog1.FileName;

            DialogResult result = openFileDialog1.ShowDialog();

            txtInkscape.Text = openFileDialog1.FileName;
            _strMyInkscape = txtInkscape.Text;
        }

        private void btnCSVtoStyle_Click(object sender, EventArgs e)
        {
            // Set the path to the CSV to Style creation project

            if (!Directory.Exists(_strMyCSVtoStyle))
                _strMyCSVtoStyle = AppDomain.CurrentDomain.BaseDirectory;

            folderBrowserDialog1.SelectedPath = _strMyCSVtoStyle;
            txtCSVtoStyle.Text = folderBrowserDialog1.SelectedPath;

            DialogResult result = folderBrowserDialog1.ShowDialog();

            txtCSVtoStyle.Text = folderBrowserDialog1.SelectedPath;
            _strMyCSVtoStyle = txtCSVtoStyle.Text;
        }

        private void btnSQLite_Click(object sender, EventArgs e)
        {
            // Set the path to the Sqlite command line app

            openFileDialog1.FileName = _strMySQLite;
            txtSQLite.Text = openFileDialog1.FileName;

            DialogResult result = openFileDialog1.ShowDialog();

            txtSQLite.Text = openFileDialog1.FileName;
            _strMySQLite = txtSQLite.Text;
        }

        private void btnStylx_Click(object sender, EventArgs e)
        {
            // Set the path to the all-symbol stylx file created from within ArcGIS Pro

            openFileDialog1.FileName = _strMyStylx;
            txtStylx.Text = openFileDialog1.FileName;

            DialogResult result = openFileDialog1.ShowDialog();

            txtStylx.Text = openFileDialog1.FileName;
            _strMyStylx = txtStylx.Text;
        }

        private void btnPro_Click(object sender, EventArgs e)
        {
            // Set the path to ArcGIS Pro

            openFileDialog1.FileName = _strMyPro;
            txtPro.Text = openFileDialog1.FileName;

            DialogResult result = openFileDialog1.ShowDialog();

            txtPro.Text = openFileDialog1.FileName;
            _strMyPro = txtPro.Text;
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            // Set the path to the ArcGIS Pro project

            openFileDialog1.FileName = _strMyProject;
            txtProject.Text = openFileDialog1.FileName;

            DialogResult result = openFileDialog1.ShowDialog();

            txtProject.Text = openFileDialog1.FileName;
            _strMyProject = txtProject.Text;
        }

        private static void ConvertGraphics(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // Converts JMSML SVG files to EMF

            List<string> l = (List<string>)doWorkEventArgs.Argument;

            try
            {
                Microsoft.VisualBasic.Devices.Computer comp = new Microsoft.VisualBasic.Devices.Computer();

                // Delete svg and emf directories

                if (comp.FileSystem.DirectoryExists(l[2]))
                {
                    comp.FileSystem.DeleteDirectory(l[2], Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
                }

                if (comp.FileSystem.DirectoryExists(l[2] + "_EMF"))
                {
                    comp.FileSystem.DeleteDirectory(l[2] + "_EMF", Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
                }

                // Copy the svg tree from JMSML to a new working location

                if (comp.FileSystem.DirectoryExists(l[0] + l[1]))
                {
                    comp.FileSystem.CopyDirectory(l[0] + l[1], l[2], true);

                    // Modify the conversion batch file with local pathnames

                    string line;

                    if (File.Exists(l[3] + _strConvertBatch))
                    {
                        System.IO.StreamReader fileIn = new System.IO.StreamReader(l[3] + _strConvertBatch);
                        System.IO.StreamWriter fileOut = new System.IO.StreamWriter(_strScratchConvertBatch, false);

                        while ((line = fileIn.ReadLine()) != null)
                        {
                            if (line.StartsWith("SET converter"))
                            {
                                line = "SET converter=" + l[4];
                            }
                            else if (line.StartsWith("SET source_folder="))
                            {
                                line = "SET source_folder=" + l[2];
                            }
                            else if (line.StartsWith("SET destination_folder="))
                            {
                                line = "SET destination_folder=" + l[2] + "_EMF";
                            }

                            fileOut.WriteLine(line);
                        }

                        fileIn.Close();
                        fileOut.Close();

                        // Now run the batch file

                        System.Diagnostics.ProcessStartInfo pStart = new ProcessStartInfo(_strScratchConvertBatch);
                        pStart.Arguments = "> ConverterOutput.txt 2>&1";
                        pStart.WindowStyle = ProcessWindowStyle.Hidden;
                        pStart.WorkingDirectory = ".";

                        System.Diagnostics.Process proc = new Process();
                        proc.StartInfo = pStart;

                        proc.Start();
                        proc.WaitForExit();
                        proc.Close();

                        // Display conversion results in Notepad

                        System.Diagnostics.ProcessStartInfo pStartNotepad = new ProcessStartInfo("notepad.exe");
                        pStartNotepad.Arguments = "ConverterOutput.txt";
                        pStartNotepad.WorkingDirectory = ".";
                        pStartNotepad.UseShellExecute = false;

                        proc = new Process();
                        proc.StartInfo = pStartNotepad;
                        proc.Start();
                        proc.Close();

                        _strMessage = "Graphic conversion completed, please check the results...";
                    }
                    else
                    {
                        _strMessage = "Batch conversion file is missing.";
                    }
                }
                else
                {
                    _strMessage = "Directory does not exist.";
                }
            }
            catch (Exception e)
            {
                _strMessage = e.Message;
            }
        }

        private static void CreateStyles(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // Creates style files from EMFs and JMSML CSV output.

            string line;
            System.IO.StreamReader fileIn;
            System.IO.StreamWriter fileOut;

            try
            {
                List<string> l = (List<string>)doWorkEventArgs.Argument;

                // Update the JMSML output files with the paths to the actual EMF files.

                Microsoft.VisualBasic.Devices.Computer comp = new Microsoft.VisualBasic.Devices.Computer();

                // Delete scratch csv folder and then create it again

                if (comp.FileSystem.DirectoryExists(l[3] + _strScratchCSVs))
                {
                    comp.FileSystem.DeleteDirectory(l[3] + _strScratchCSVs, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
                }
                comp.FileSystem.CreateDirectory(l[3] + _strScratchCSVs);

                // Same with the style file output folder

                if (comp.FileSystem.DirectoryExists(l[3] + _strStyleOutput))
                {
                    comp.FileSystem.DeleteDirectory(l[3] + _strStyleOutput, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
                }
                comp.FileSystem.CreateDirectory(l[3] + _strStyleOutput);

                // Modify the csv files so they point to the correct graphic (emf) files

                if (comp.FileSystem.DirectoryExists(l[0] + _strCSVFiles))
                {
                    string[] files = Directory.GetFiles(l[0] + _strCSVFiles, "*.csv", SearchOption.TopDirectoryOnly);

                    foreach (string file in files)
                    {
                        fileIn = new System.IO.StreamReader(file);
                        fileOut = new System.IO.StreamWriter(l[3] + _strScratchCSVs + file.Substring(file.LastIndexOf('\\')), false);

                        while ((line = fileIn.ReadLine()) != null)
                        {
                            if (line.StartsWith("{Symbols_Root}"))
                            {
                                line = line.Replace("{Symbols_Root}", l[1] + "_EMF");
                            }

                            fileOut.WriteLine(line);
                        }

                        fileIn.Close();
                        fileOut.Close();
                    }

                    // Update paths in batch file

                    if (File.Exists(l[2] + _strCreateStylesBatch))
                    {
                        fileIn = new System.IO.StreamReader(l[2] + _strCreateStylesBatch);
                        fileOut = new System.IO.StreamWriter(_strScratchCreateStylesBatch, false);

                        while ((line = fileIn.ReadLine()) != null)
                        {
                            if (line.StartsWith("csv2ArcGISStyle.exe"))
                            {
                                line = line.Replace("csv2ArcGISStyle.exe", l[2] + "\\csv2ArcGISStyle.exe");
                                line = line.Replace("C:\\{TODO_NO_SPACES_CSV2STYLEHOME}\\CsvSourceData", l[3] + _strScratchCSVs);
                                line = line.Replace("C:\\{TODO_NO_SPACES_CSV2STYLEHOME}\\StyleOutputData", l[3] + _strStyleOutput);
                            }

                            fileOut.WriteLine(line);
                        }

                        fileIn.Close();
                        fileOut.Close();

                        // Now run the batch file

                        System.Diagnostics.ProcessStartInfo pStart = new ProcessStartInfo(_strScratchCreateStylesBatch);
                        pStart.Arguments = "> CreateStyleOutput.txt 2>&1";
                        pStart.WindowStyle = ProcessWindowStyle.Hidden;
                        pStart.WorkingDirectory = ".";

                        System.Diagnostics.Process proc = new Process();
                        proc.StartInfo = pStart;

                        proc.Start();
                        proc.WaitForExit();
                        proc.Close();

                        // Display conversion results in Notepad

                        System.Diagnostics.ProcessStartInfo pStartNotepad = new ProcessStartInfo("notepad.exe");
                        pStartNotepad.Arguments = "CreateStyleOutput.txt";
                        pStartNotepad.WorkingDirectory = ".";
                        pStartNotepad.UseShellExecute = false;

                        proc = new Process();
                        proc.StartInfo = pStartNotepad;
                        proc.Start();
                        proc.Close();

                        _strMessage = "Style creation completed, please check the results...";
                    }
                    else
                    {
                        _strMessage = "CSV to Style file not found.";
                    }
                }
                else
                {
                    _strMessage = "Directory does not exist.";
                }
            }
            catch (Exception e)
            {
                _strMessage = e.Message;
            }
        }

        private static void MergeStylx(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            // Merges Pro created stylx file with manually managed line/area/label stylx file.

            List<string> l = (List<string>)doWorkEventArgs.Argument;

            try
            {
                // Copy the template merge file locally

                if (File.Exists(l[1] + _strTemplateStylx))
                {
                    File.Copy(l[1] + _strTemplateStylx, l[0] + _strStyleOutput + _strScratchTemplateStylx, true);

                    // Modify the sql script with correct paths - note that the sql script needs forward
                    // slashes on its paths, even in Windows.

                    string line;

                    if (File.Exists(l[1] + _strSqlScript))
                    {
                        System.IO.StreamReader fileIn = new System.IO.StreamReader(l[1] + _strSqlScript);
                        System.IO.StreamWriter fileOut = new System.IO.StreamWriter(l[0] + _strScratchSqlScript, false);

                        while ((line = fileIn.ReadLine()) != null)
                        {
                            if (line.Contains("{Full Path To}/mil2525d-points-only.stylx"))
                            {
                                line = line.Replace("{Full Path To}/mil2525d-points-only.stylx", l[3]);
                                line = line.Replace('\\', '/');
                            }

                            if (line.Contains("{Full Path To}/mil2525d.stylx"))
                            {
                                line = line.Replace("{Full Path To}/mil2525d.stylx", l[0] + _strStyleOutput + _strScratchTemplateStylx);
                                line = line.Replace('\\', '/');
                            }

                            if (line.Contains("{FULL-PATH-TO}/Military-All-Icons.csv"))
                            {
                                line = line.Replace("{FULL-PATH-TO}", l[4] + _strCSVFiles);
                                line = line.Replace('\\', '/');
                            }

                            fileOut.WriteLine(line);
                        }

                        fileIn.Close();
                        fileOut.Close();

                        // Can't seem to run this from a process, so lets create a batch file and try and run 

                        fileOut = new StreamWriter(_strMergeBatch, false);
                        fileOut.WriteLine(l[2] + " <" + l[0] + _strScratchSqlScript + " >MergeStylxOutput.txt 2>&1");
                        fileOut.Close();

                        // Now run the batch file, which runs sqlite3 and loads the sql script

                        System.Diagnostics.ProcessStartInfo pStart = new ProcessStartInfo(_strMergeBatch);
                        pStart.WindowStyle = ProcessWindowStyle.Hidden;
                        pStart.WorkingDirectory = ".";

                        System.Diagnostics.Process proc = new Process();
                        proc.StartInfo = pStart;

                        proc.Start();
                        proc.WaitForExit();
                        proc.Close();

                        // Display conversion results in Notepad

                        System.Diagnostics.ProcessStartInfo pStartNotepad = new ProcessStartInfo("notepad.exe");
                        pStartNotepad.Arguments = "MergeStylxOutput.txt";
                        pStartNotepad.WorkingDirectory = ".";
                        pStartNotepad.UseShellExecute = false;

                        proc = new Process();
                        proc.StartInfo = pStartNotepad;
                        proc.Start();
                        proc.Close();

                        _strMessage = "Stylx files merged, please check the results...";
                    }
                    else
                    {
                        _strMessage = "SQL script file is missing.";
                    }
                }
                else
                {
                    _strMessage = "Managed SQL file is missing.";
                }
            }
            catch (Exception e)
            {
                _strMessage = e.Message;
            }
        }

        private void ConvertGraphicsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Executed when the graphic file conversion process completes

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
            toolStripProgressBar1.MarqueeAnimationSpeed = 0;

            toolStripStatusLabel1.Text = _strMessage;

            _bw.DoWork -= ConvertGraphics;
            _bw.RunWorkerCompleted += ConvertGraphicsCompleted;

            btnConvertGraphics.BackColor = Color.Khaki;
            btnCreateStyle.BackColor = Color.PaleGreen;
        }

        private void CreateStylesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Executed when the style file creation process completes

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
            toolStripProgressBar1.MarqueeAnimationSpeed = 0;

            toolStripStatusLabel1.Text = _strMessage;

            _bw.DoWork -= CreateStyles;
            _bw.RunWorkerCompleted -= CreateStylesCompleted;

            // Rename the resulting style files so their names match what is placed in the military features data repo

            Microsoft.VisualBasic.Devices.Computer comp = new Microsoft.VisualBasic.Devices.Computer();

            string[] files = Directory.GetFiles(_strMyHome + _strStyleOutput, "*.style", SearchOption.TopDirectoryOnly);
            
            foreach (string file in files)
            {
                string renamedFile = file.Substring(file.LastIndexOf('\\') + 1);
                renamedFile = renamedFile.Replace('-', ' ');
                renamedFile = renamedFile.Replace("Source", "");
                renamedFile = renamedFile.Replace("Icons", "");
                renamedFile = renamedFile.Replace("  ", " ");
                renamedFile = renamedFile.Replace(" .", ".");
                renamedFile = renamedFile.Replace("ControlMeasures", "Control Measures");
                renamedFile = renamedFile.Replace("Sigint", "SigInt");
                renamedFile = renamedFile.Replace("Frame And Amplifier", "Frames And Amplifiers");

                comp.FileSystem.RenameFile(file, renamedFile);
            }

            btnCreateStyle.BackColor = Color.Khaki;
            btnImportIntoPro.BackColor = Color.PaleGreen;
        }

        private void MergeStylxCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Executes when the stylx file merge process completes

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
            toolStripProgressBar1.MarqueeAnimationSpeed = 0;

            toolStripStatusLabel1.Text = _strMessage;

            _bw.DoWork -= MergeStylx;
            _bw.RunWorkerCompleted -= MergeStylxCompleted;

            btnMergeStylx.BackColor = Color.Khaki;
        }

        private void btnConvertGraphics_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Converting graphic files...";

            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.MarqueeAnimationSpeed = 30;

            List<string> l = new List<string>();
            l.Add(txtJMSML.Text);
            l.Add(_strJMSMLSymbols);
            l.Add(_strMySymbols);
            l.Add(_strMyMFD);
            l.Add(_strMyInkscape);

            _bw.DoWork += ConvertGraphics;
            _bw.RunWorkerCompleted += ConvertGraphicsCompleted;
            _bw.RunWorkerAsync(l);
        }

        private void btnCreateStyle_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Creating style files...";

            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.MarqueeAnimationSpeed = 30;

            List<string> l = new List<string>();
            l.Add(txtJMSML.Text);
            l.Add(_strMySymbols);
            l.Add(_strMyCSVtoStyle);
            l.Add(_strMyHome);

            _bw.DoWork += CreateStyles;
            _bw.RunWorkerCompleted += CreateStylesCompleted;
            _bw.RunWorkerAsync(l);
        }

        private void btnMergeStylx_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Merging stylx files...";

            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            toolStripProgressBar1.MarqueeAnimationSpeed = 30;

            List<string> l = new List<string>();
            l.Add(_strMyHome);
            l.Add(_strMyMFD);
            l.Add(_strMySQLite);
            l.Add(_strMyStylx);
            l.Add(_strMyJMSML);

            _bw.DoWork += MergeStylx;
            _bw.RunWorkerCompleted += MergeStylxCompleted;
            _bw.RunWorkerAsync(l);
        }

        private void btnImportIntoPro_Click(object sender, EventArgs e)
        {
            // Run ArcGIS Pro with the given project, so the user can import the style file and create a stylx

            System.Diagnostics.ProcessStartInfo pStart = new ProcessStartInfo(_strMyPro);
            pStart.Arguments = _strMyProject;
            pStart.WindowStyle = ProcessWindowStyle.Hidden;
            pStart.WorkingDirectory = ".";

            System.Diagnostics.Process proc = new Process();
            proc.StartInfo = pStart;

            proc.Start();

            btnMergeStylx.BackColor = Color.PaleGreen;
            btnImportIntoPro.BackColor = Color.Khaki;

            MessageBox.Show("Within ArcGIS Pro, use 'Insert>Import Style' to convert the newly created 'Military 2525Delta All.style' file to stylx.");
        }

        private void txtHome_TextChanged(object sender, EventArgs e)
        {
            _strMyHome = txtHome.Text;
        }

        private void txtMFD_TextChanged(object sender, EventArgs e)
        {
            _strMyMFD = txtMFD.Text;
        }

        private void txtJMSML_TextChanged(object sender, EventArgs e)
        {
            _strMyJMSML = txtJMSML.Text;
        }

        private void txtGraphics_TextChanged(object sender, EventArgs e)
        {
            _strMySymbols = txtGraphics.Text;
        }

        private void txtInkscape_TextChanged(object sender, EventArgs e)
        {
            _strMyInkscape = txtInkscape.Text;
        }

        private void txtCSVtoStyle_TextChanged(object sender, EventArgs e)
        {
            _strMyCSVtoStyle = txtCSVtoStyle.Text;
        }

        private void txtPro_TextChanged(object sender, EventArgs e)
        {
            _strMyPro = txtPro.Text;
        }

        private void txtProject_TextChanged(object sender, EventArgs e)
        {
            _strMyProject = txtProject.Text;
        }

        private void txtStylx_TextChanged(object sender, EventArgs e)
        {
            _strMyStylx = txtStylx.Text;
        }

        private void txtSQLite_TextChanged(object sender, EventArgs e)
        {
            _strMySQLite = txtSQLite.Text;
        }
    }
}
