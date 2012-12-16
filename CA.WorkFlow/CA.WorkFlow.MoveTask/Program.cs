using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using System.Configuration.Provider;
using System.Data;
using System.IO;

namespace CA.WorkFlow.MoveTask
{
    class Program
    {
        static string sSiteURL = System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString();
        static string sWebName = System.Configuration.ConfigurationManager.AppSettings["WebName"].ToString();
        static string sDate = System.Configuration.ConfigurationManager.AppSettings["Date"].ToString();
        static string sRestoreTitle = "[This is a restore task from RestoreTask]";
        static void Main(string[] args)
        {
            Console.WriteLine("Start move completed task......");
            try
            {
                //MoveCompleteTask();
                MoveSpecialTask();
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured:" + ex.ToString());
                WriteErrorLog("An error occured:" + ex.ToString());
            }
            Console.WriteLine("Completed");
        }

        static void MoveCompleteTask()
        {
            using (SPSite site = new SPSite(sSiteURL))
            {
                using (SPWeb web = site.OpenWeb(sWebName))
                {
                    SPQuery query = new SPQuery();
                    query.Query = @"
                                    <Where>
                                         <Eq>
                                             <FieldRef Name='Status' />
                                             <Value Type='Text'>Completed</Value>
                                          </Eq>
                                    </Where>";
                    CopyList(web.Lists["Tasks"].GetItems(query), web.Lists["CompletedTasks"]);
                   

                }
            }
        }

        static void MoveSpecialTask()
        {
            //Created
            using (SPSite site = new SPSite(sSiteURL))
            {
                using (SPWeb web = site.OpenWeb(sWebName))
                {
                    SPQuery query = new SPQuery();
                    query.Query = string.Format(@"
                                                   <Where>
                                                      <And>
                                                         <Neq>
                                                            <FieldRef Name='Status' />
                                                            <Value Type='Choice'>Completed</Value>
                                                         </Neq>
                                                         <Lt>
                                                            <FieldRef Name='Created' />
                                                            <Value Type='DateTime'>{0}</Value>
                                                         </Lt>
                                                      </And>
                                                   </Where>", sDate);

                    CopyList(web.Lists["Tasks"].GetItems(query), web.Lists["CompletedTasks"]);
                }
            }
        }

        static bool ExistTask()
        {
            return true;
        }

        public static void CopyList(SPListItemCollection spliicSoruce, SPList splTarget)
        {
            foreach (SPListItem item in spliicSoruce)
            {
                string sTitle = item["Title"] == null ? string.Empty : item["Title"].ToString();
                if (sTitle.Contains(sRestoreTitle))//是还原过来的Task
                {
                    continue;
                }
               if (isUnique(splTarget,item.ID.ToString()))// sTitle, sAssignedTo))
               {
                    SPListItem newDestItem = splTarget.Items.Add();

                    foreach (SPField field in item.Fields)
                    {
                        try
                        {
                            string sInternalName = field.InternalName;
                            if ((!field.ReadOnlyField) && (sInternalName != "Attachments"))
                            {
                                    newDestItem[sInternalName] = item[sInternalName];
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    }
                    newDestItem["TaskID"] = item.ID;
                    newDestItem["Link"] = item["Link"];
                    newDestItem["EndDate"] = item["Modified"];
                    newDestItem["Outcome"] = item["Outcome"];
                    newDestItem.Update();

                    string sLog = "MoveTask:"+item["Title"].ToString();
                    WriteErrorLog(sLog);
                }
            }
        }

        /// <summary>
        /// 检测list里是否存在重复的task记录
        /// </summary>
        /// <param name="splCompletedTasks"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool isUnique(SPList splCompletedTasks,string id) //string sTitle, string sAssignedTo)//sTitle, sAssignedTo
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format(@"
                                            <Where>
                                                 <Eq>
                                                     <FieldRef Name='TaskID' />
                                                     <Value Type='Text'>{0}</Value>
                                                  </Eq>
                                            </Where>", id);
//            query.Query = string.Format(@" <Where>
//                                              <And>
//                                                 <Eq>
//                                                    <FieldRef Name='AssignedTo' />
//                                                    <Value Type='User'>{0}</Value>
//                                                 </Eq>
//                                                 <Contains>
//                                                    <FieldRef Name='Title' />
//                                                    <Value Type='Text'>{1}</Value>
//                                                 </Contains>
//                                              </And>
//                                           </Where>",sAssignedTo,sTitle);

            int iCount = splCompletedTasks.GetItems(query).Count;
            if (iCount > 0)
            {
                return false;
            }
            else
            {
            return true;
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


        static bool CheckGUID(SPList splTask, string sGUID)
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format(@"
                                                <Where>
                                                     <Eq>
                                                         <FieldRef Name='OriginalGuid' />
                                                         <Value Type='Text'>{0}</Value>
                                                      </Eq>
                                                </Where>", sGUID);

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

    }
}
