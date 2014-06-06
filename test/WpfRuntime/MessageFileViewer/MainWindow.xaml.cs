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
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.AdvancedSymbology;
using ESRI.ArcGIS.Client.Geometry;

using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Win32;

namespace MessageFileViewer
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBusy = true;

        private List<string> addedIds = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            // Disable the UI button until the MessageProcessor is initialized
            ProcessMessagesButton.IsEnabled = false;
            _map.Layers.LayersInitialized += HandleLayersInitialized;
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;

                if (PropertyChanged != null)
                {

                    PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
                }
            }
        }

        private void HandleLayersInitialized(object sender, System.EventArgs e)
        {
            // The MessageProcessor is initialized - enable the UI button
            ProcessMessagesButton.IsEnabled = true;
            DataContext = this;
            IsBusy = false;
        }

        private void ProcessMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // This function simulates real time message processing by processing a static set of messages from an XML document.

            /* 
            * |== Example Message ==|
            * 
            * <geomessage>
            *      <_type>position_report</_type>
            *      <_action>update</_action>
            *      <_id>16986029-8295-48d1-aa6a-478f400a53c0</_id>
            *      <_wkid>3857</_wkid>
            *      <sic>GFGPOLKGS-----X</sic>
            *      <_control_points>-226906.99878,6679149.88998;-228500.51759,6677576.8009;-232194.67644,6675625.78198</_control_points>
            *      <uniquedesignation>DIRECTION OF ATTACK</uniquedesignation>
            * </geomessage>
            */

            // Old code:
            // string messagesXmlFilePath = Path.Combine(ESRI.ArcGIS.Client.ArcGISRuntime.InstallPath, @"SDK\Samples\Data\Symbology\Mil2525CMessages.xml");                     

            OpenFileDialog openDialog = new OpenFileDialog();

            if (!openDialog.ShowDialog().Value)
                return;

            var xmlFilePath = openDialog.FileName;

            // Load the XML document
            XDocument xmlDocument = XDocument.Load(xmlFilePath, LoadOptions.None);

            // Create a collection of messages - may contain both tags: geomessage -or- message
            IEnumerable<XElement> messagesXml = from n in xmlDocument.Root.Elements() where n.Name == "geomessage" select n;

            if (messagesXml.Count() <= 0)
                messagesXml = from n in xmlDocument.Root.Elements() where n.Name == "message" select n;

            if (messagesXml.Count() <= 0)
                MessageBox.Show("No messages found", "No messages found");

            // Iterate through the messages passing each to the ProcessMessage method on the MessageProcessor.
            // The MessageGroupLayer associated with this MessageProcessor will handle the creation of any 
            // GraphicsLayers and Graphic objects necessary to display the message.
            ESRI.ArcGIS.Client.Geometry.MapPoint zoomPoint = null;

            bool success = false;
            foreach (XElement messageXml in messagesXml)
            {
                Message message = new Message(from n in messageXml.Elements() select new KeyValuePair<string, string>(n.Name.ToString(), n.Value));

                success = false;
                try
                {
                    success = _messageLayer.ProcessMessage(message);
                }
                catch (System.Exception ex)
                {
                    if (message.ContainsKey("_type"))
                        System.Diagnostics.Trace.WriteLine("Exception processing message type: " + message["_type"]);
                }

                if (success)
                    addedIds.Add(message.Id);  

                try
                {
                    // Zoom to a symbol point if SRs match
                    if (zoomPoint == null)
                    {
                        string controlPointsString = message["_control_points"];
                        string wkidString = message["_wkid"];

                        string firstCoord = controlPointsString.Split(';')[0]; // in case > 1
                        string xString = firstCoord.Split(',')[0];
                        string yString = firstCoord.Split(',')[1];

                        double x = double.Parse(xString);
                        double y = double.Parse(yString);

                        int wkid = int.Parse(wkidString);

                        SpatialReference sr = new SpatialReference(wkid);
                        zoomPoint = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y, sr);

                        // TODO: need to spin up a local service with a GeometryService Task
                        // to do arbitrary conversions from any WKID
                    }
                }
                catch (System.Exception ex)
                {
                    // probably a parse exception
                }

            }

            if (zoomPoint != null)
            {
                // Zoom to the first point found (if we are able)
                if (zoomPoint.SpatialReference.WKID == 3857)
                {
                    Envelope env = new ESRI.ArcGIS.Client.Geometry.Envelope(
                    zoomPoint.X - 10000,
                    zoomPoint.Y - 10000,
                    zoomPoint.X + 10000,
                    zoomPoint.Y + 10000);
                    _map.ZoomTo(env);
                }
            }
        }

        private void ClearSymbolsButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var id in addedIds)
            {
                Message message = new Message();
                message.Id = id;
                message["_action"] = "remove";
                bool success = _messageLayer.ProcessMessage(message);
                if (!success)
                    System.Diagnostics.Trace.WriteLine("Could not remove Id: " + id.ToString());
            }

            addedIds.Clear();
        }

    }

}
