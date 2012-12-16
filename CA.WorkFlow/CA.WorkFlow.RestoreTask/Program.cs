using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using System.Configuration.Provider;
using System.Data;
using System.IO;

namespace CA.WorkFlow.RestoreTask
{
    class Program
    {
        static string sSiteURL = System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString();
        static string sWebName = System.Configuration.ConfigurationManager.AppSettings["WebName"].ToString();
        static string sRestoreTitle="[This is a restore task from RestoreTask]";


        static void Main(string[] args)
        {
            Console.WriteLine("Start Restore task......");
            try
            {
                Test();
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured:" + ex.ToString());
                WriteErrorLog("An error occured:" + ex.ToString());
            }
            Console.WriteLine("Completed");
        }
        
            static void Test()
            {
                using (SPSite site = new SPSite(sSiteURL))
                {
                    using (SPWeb web = site.OpenWeb(sWebName))
                    {
                        web.AllowUnsafeUpdates = true;
                        ResoreTask(web.Lists["CompletedTasks"].Items, web.Lists["Tasks"]);
                    }
                }
            }

            static bool ExistTask()
            {
                return true;
            }

            public static void ResoreTask(SPListItemCollection splicCompletedTasks, SPList splTask)
            {
                foreach (SPListItem item in splicCompletedTasks)
                {
                    string sTaskID = item["TaskID"] == null ? string.Empty : item["TaskID"].ToString();

                    string sTitle = item["Title"] == null ? string.Empty : item["Title"].ToString().Replace(sRestoreTitle,"");
                   // string sGUID = item["OriginalGuid"] == null ? string.Empty : item["OriginalGuid"].ToString();
                    if (CheckGUID(splTask, sTaskID))
                 //   if (isUnique(splTask, sTitle, sAssignedTo))
                    {

                        SPListItem TaskItem = splTask.Items.Add();
                        foreach (SPField field in TaskItem.Fields)
                        {
                            try
                            {
                                if ((!field.ReadOnlyField) && (field.InternalName != "Attachments"))
                                {
                                    string sInernalName=field.InternalName;
                                    if (sInernalName == "Title")
                                    {
                                        TaskItem[field.InternalName] = item[field.InternalName].ToString() + sRestoreTitle;
                                    }
                                    else
                                    {
                                        TaskItem[sInernalName] = item[sInernalName];
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                        }

                        TaskItem["Outcome"] = item["Outcome"];
                        TaskItem["Link"] = item["Link"];
                        TaskItem.Update(); //only now you call update! 
                        TaskItem["DueDate"] = item["EndDate"];

                        //item["TaskID"] = TaskItem.ID;//更新CompletedTask的TaskID为Task的ID。

                        string sLog = "Restore task:" + item["Title"].ToString();
                        WriteErrorLog(sLog);
                    }
                }
            }

            /// <summary>
            /// 检测list里是否存在重复的ID记录
            /// </summary>
            /// <param name="splCompletedTasks"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public static bool isUnique(SPList splTask, string id)///sTitle, sAssignedTo
            {
                SPQuery query = new SPQuery();
                query.Query = string.Format(@"
                                                <Where>
                                                     <Eq>
                                                         <FieldRef Name='ID' />
                                                         <Value Type='Counter'>{0}</Value>
                                                      </Eq>
                                                </Where>", id);

                int iCount = splTask.GetItems(query).Count;
                if (iCount > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            static bool CheckGUID(SPList splTask, string id)
            {

                SPQuery query = new SPQuery();
                query.Query = string.Format(@"
                                                <Where>
                                                     <Eq>
                                                         <FieldRef Name='TaskID' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                </Where>", id);
                int iCount = splTask.GetItems(query).Count;
                if (iCount > 0)
                {
                    return false;
                }
                else
                {
                    SPQuery queryID = new SPQuery();
                    queryID.Query = string.Format(@"
                                                <Where>
                                                     <Eq>
                                                         <FieldRef Name='ID' />
                                                         <Value Type='Counter'>{0}</Value>
                                                      </Eq>
                                                </Where>", id);
                    int iCountID = splTask.GetItems(queryID).Count;
                    if (iCountID > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }


            }

            public static void WriteErrorLog(string sErrorInfo)
            {
                if (!Directory.Exists("Log"))
                {
                    Directory.CreateDirectory("Log");
                }
                string sDate = DateTime.Now.ToString("yyyy-MM-dd");
                string sFileNme = string.Concat("Log/", sDate, ".txt");
                StreamWriter sw = File.AppendText(sFileNme);
                string sErrorFormate = string.Format("{0}: {1}\r\n-----------------------------------------------------------\r\n", DateTime.Now.ToString(), sErrorInfo);

                sw.WriteLine(sErrorFormate);
                sw.Flush();
                sw.Dispose();
            }
    }
}
