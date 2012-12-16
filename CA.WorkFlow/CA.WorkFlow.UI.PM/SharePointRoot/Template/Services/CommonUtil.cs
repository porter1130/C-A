using Microsoft.SharePoint;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CA.WorkFlow.UI
{
    public static class CommonUtil
    {
        public static void logError(string error)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string sourceName = @"C&A(Workflow)";
                string logName = @"EWF";

                if (EventLog.SourceExists(sourceName))
                {

                    string oldLogName = EventLog.LogNameFromSourceName(sourceName, System.Environment.MachineName);
                    if (!oldLogName.Equals(logName))
                    {
                        EventLog.Delete(oldLogName);
                    }
                }

                if (!EventLog.Exists(logName))
                {
                    EventLog.CreateEventSource(sourceName, logName);
                }

                EventLog myLog = new EventLog();
                myLog.Source = sourceName;
                myLog.Log = logName;

                myLog.WriteEntry(error, EventLogEntryType.Error);
            });
        }

        public static void logInfo(string info)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string sourceName = @"C&A(Workflow)";
                string logName = @"EWF";

                if (EventLog.SourceExists(sourceName))
                {

                    string oldLogName = EventLog.LogNameFromSourceName(sourceName, System.Environment.MachineName);
                    if (!oldLogName.Equals(logName))
                    {
                        EventLog.Delete(oldLogName);
                    }
                }

                if (!EventLog.Exists(logName))
                {
                    EventLog.CreateEventSource(sourceName, logName);
                }

                EventLog myLog = new EventLog();
                myLog.Source = sourceName;
                myLog.Log = logName;

                myLog.WriteEntry(info, EventLogEntryType.Information);
            });
        }

        public static void logMsg(string msg, EventLogEntryType logLevel)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string sourceName = @"C&A(Workflow)";
                string logName = @"EWF";

                if (EventLog.SourceExists(sourceName))
                {

                    string oldLogName = EventLog.LogNameFromSourceName(sourceName, System.Environment.MachineName);
                    if (!oldLogName.Equals(logName))
                    {
                        EventLog.Delete(oldLogName);
                    }
                }

                if (!EventLog.Exists(logName))
                {
                    EventLog.CreateEventSource(sourceName, logName);
                }

                EventLog myLog = new EventLog();
                myLog.Source = sourceName;
                myLog.Log = logName;
                              

                myLog.WriteEntry(msg, logLevel);
            });
        }

        public static string GetJson<T>(T obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                json.WriteObject(ms, obj);
                string szJson = Encoding.UTF8.GetString(ms.ToArray());
                return szJson;
            }
        }

        /**
         * 获取当前根站点
         */
        public static string GetRootURL(string url)
        {
            return url.EndsWith("/") ? url : url + "/";
        }

        public static DataTable AsDataTable<T>(this IEnumerable<T> enumerable)
        {
            DataTable dt = new DataTable("Generated");

            T first = enumerable.FirstOrDefault();
            if (first == null)
            {
                return dt;
            }

            PropertyInfo[] properties = first.GetType().GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }

            foreach (T t in enumerable)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo pi in properties)
                {
                    row[pi.Name] = t.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, t, null);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}