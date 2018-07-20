/* 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateSVGStyle
{
    /// <summary>
    /// Takes a CSV file and converts to a Data Table
    /// IMPORTANT: the CSV *must* have a first/header row that has the desired 
    ///            column names to be used
    /// CSV lines starting with "#" will be ignored (as comments)
    /// </summary>
    class CsvToTableMaker
    {
        public CsvToTableMaker()
        {

        }

        // Somewhat hack-ish - needed check for when file is locked or can't be opened
        // (e.g. because of open/being viewed somewhere else-like Excel)
        public bool IsFileOpenable(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException ex)
            {
                System.Diagnostics.Trace.WriteLine("ERROR: " + " Message: " + ex.Message + 
                    " Could not open file : " + filePath);
                return false;
            }

            return true;
        }

        public bool LoadTable(string csvFile)
        {
            bool success = false;

            if (!File.Exists(csvFile) || !(IsFileOpenable(csvFile)))
                return false;
           
            if (table != null)
            {
                System.Diagnostics.Trace.WriteLine(
                    "Warning: Unexpected: Table has been initialized previously");
                return false;
            }

            table = new DataTable();

            bool firstRow = true;
            int maxHeadingColumn = 0;

            foreach (string line in File.ReadLines(csvFile))
            {
                if (line.StartsWith("#")) // allow "#" comment character
                    continue;

                string[] values = line.Split(',');

                if (firstRow)
                {
                    // IMPORTANT: creates column names from the first row
                    foreach (string columnName in values)
                    {
                        table.Columns.Add(columnName);
                        maxHeadingColumn++;
                    }
                    firstRow = false;
                    continue;
                }

                DataRow row = table.NewRow();

                int index = 0;
                foreach (string value in values)
                {
                    if (index < maxHeadingColumn) // just in case 
                        row[index++] = value;
                }

                table.Rows.Add(row);
            }

            success = true;

            return success;
        }

        public DataTable Table
        {
            get { return table; }
        }
        private DataTable table = null;

        public bool IsValid
        {
            get
            {
                if (table == null)
                    return false;

                if ((table.Rows != null) && (table.Columns != null) &&
                    (table.Rows.Count > 0) && (table.Columns.Count > 0))
                {
                    // Any other check needed? column names?
                    return true;
                }

                return false;
            }
        }

        public void DebugOutput()
        {
            if (!IsValid)
            {
                System.Diagnostics.Trace.WriteLine("Table not initialized! Can't Output!");
                return;
            }

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    System.Diagnostics.Trace.Write(" | " + item);
                }
                System.Diagnostics.Trace.WriteLine(" |");
            }
        }

    }

}
