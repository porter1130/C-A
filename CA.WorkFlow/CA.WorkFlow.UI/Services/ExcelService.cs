using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint;
using System.Collections;
using System.Text.RegularExpressions;

namespace CA.WorkFlow.UI
{

    public class ExcelService
    {
        private static int defaultCols = 50;
        private static int defaultRows = 500;

        public static string GetExcelConfigInfo(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static DataTable ParseExcel(SPFile file, string positionKeyValue, string primaryKeyValue, string colsKeyValue, string sheetName)
        {
            //Using workbookPath this way will allow 
            //you to call the workbook remotely.
            string targetWorkbookPath = SPContext.Current.Site.Url + file.ServerRelativeUrl;

            ES.ExcelService es = new ES.ExcelService();
            es.Credentials = System.Net.CredentialCache.DefaultCredentials;
            ES.Status[] outStatus;

            string sessionId = es.OpenWorkbook(targetWorkbookPath, "en-US", "en-US", out outStatus);

            string[] pos = positionKeyValue.Split(',');
            ES.RangeCoordinates rangeCoordinates = GetRangeCoordinates(es, sessionId, pos, primaryKeyValue, sheetName, outStatus);
            //string positionKeyValue = ConfigurationManager.AppSettings[positionKey];

            object[] excelObjctData = es.GetRange(sessionId, sheetName, rangeCoordinates, false, out outStatus);
            DataTable excelDataTable = ConvertToDataTable(excelObjctData, primaryKeyValue, colsKeyValue);

            //Close workbook. This also closes session.
            es.CloseWorkbook(sessionId);

            return excelDataTable;
        }

        private static ES.RangeCoordinates GetRangeCoordinates(ES.ExcelService es, string sessionId, string[] pos, string primaryKeyValue, string sheetName, ES.Status[] outStatus)
        {
            int startCol = ToIndex(pos[0]);
            int startRow = int.Parse(pos[1]) - 1;

            ES.RangeCoordinates range = new ES.RangeCoordinates();

            range.Column = startCol;
            range.Row = startRow;
            range.Width = GetExcelCols(es, sessionId, startRow, sheetName, outStatus);
            range.Height = GetExcelRows(es, sessionId, primaryKeyValue, sheetName, outStatus);

            return range;


        }

        private static int GetExcelRows(ES.ExcelService es, string sessionId,string primaryKeyValue, string sheetName, ES.Status[] outStatus)
        {
            int row = defaultRows;
            int col = ToIndex(primaryKeyValue);

            while (es.GetCell(sessionId, sheetName, row, col, false, out outStatus) != null)
            {
                row += defaultRows;
            }
            return row;
        }

        private static int GetExcelCols(ES.ExcelService es, string sessionId, int startRow, string sheetName, ES.Status[] outStatus)
        {
            int row = startRow;
            int col = defaultCols;

            while (es.GetCell(sessionId, sheetName, row, col, false, out outStatus) != null) {
                col += defaultCols;
            }

            return col;
        }



        private static DataTable ConvertToDataTable(object[] excelObjctData, string primaryKeyValue, string colsKeyValue)
        {
            Hashtable colsHash = ParseCols(colsKeyValue);

            DataTable dt = new DataTable();

            if (excelObjctData.Length > 0)
            {
                for (int i = 0; i < ((object[])excelObjctData[0]).Length; i++)
                {
                    dt.Columns.Add(ToTagName(i));
                }

                foreach (object[] objs in excelObjctData)
                {
                    if (objs[ToIndex(primaryKeyValue)] == null)
                    {
                        break;
                    }
                    dt.LoadDataRow(ParseColsType(objs, colsHash), true);

                }

                for (int i = 0; i < ((object[])excelObjctData[0]).Length; i++)
                {
                    if (!colsHash.ContainsKey(ToTagName(i)))
                    {
                        dt.Columns.Remove(ToTagName(i));
                    }
                }
            }
            return dt;
        }

        private static object[] ParseColsType(object[] objs, Hashtable colsHash)
        {
            string colValue = string.Empty;
            foreach (DictionaryEntry entry in colsHash)
            {
                switch (entry.Value.ToString().ToLower())
                {
                    case "date":
                        colValue = objs[ToIndex(entry.Key.ToString())].ToString();
                        objs[ToIndex(entry.Key.ToString())] = DateTime.FromOADate(Convert.ToDouble(colValue)).ToString("yyyy-MM-dd");
                        break;
                    default:
                        break;
                }
            }
            return objs;
        }

        private static Hashtable ParseCols(string colsKeyValue)
        {
            Hashtable hash = new Hashtable();
            string key, value;
            string[] cols = colsKeyValue.Split(',');

            string pattern = @"(.*)\[(.*)\]|.*$";
            Regex reg = new Regex(pattern, RegexOptions.None);

            foreach (string col in cols)
            {
                if (string.IsNullOrEmpty(reg.Match(col).Groups[1].Value))
                {
                    key = reg.Match(col).Groups[0].Value;
                    value = "Text";
                }
                else
                {
                    key = reg.Match(col).Groups[1].Value;
                    value = reg.Match(col).Groups[2].Value;
                }
                hash.Add(key, value);
            }
            return hash;
        }

        private static int ToIndex(string primaryKeyValue)
        {
            char[] tags = primaryKeyValue.ToCharArray();
            int index = 0;
            if (tags.Length == 1)
            {
                index = (int)tags[0] % (int)'A';
            }
            else
            {
                int position = 0;
                for (int i = 0; i < tags.Length; i++)
                {
                    position += (ToIndex(tags[i].ToString()) + 1) * (int)Math.Pow(26, tags.Length - 1 - i);
                }
                index = position - 1;
            }
            return index;
        }

        private static bool IsNeedColumn(string colName, string[] cols)
        {
            bool isNeedColumn = false;
            foreach (string s in cols)
            {
                if (colName == s)
                {
                    isNeedColumn = true;
                    break;
                }
            }
            return isNeedColumn;
        }

        private static string ToTagName(int index)
        {
            if ((index - index % 26) / 26 == 0)
            {
                return ((char)(index % 26 + (int)'A')).ToString();
            }
            return ToTagName((index - index % 26) / 26 - 1) + ToTagName(index % 26);
        }


    }
}
