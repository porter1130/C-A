using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using System.Configuration.Provider;
using System.Data;

namespace CA.WorkFlow.DeleteTask
{
    class Program
    {
        static string sSiteURL = System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString();
        static string sWebName = System.Configuration.ConfigurationManager.AppSettings["WebName"].ToString();
        static string sRestoreTitle = "[This is a restore task from RestoreTask]";
        static void Main(string[] args)
        {
            Console.WriteLine("Start Restore task......");
            Test();
            Console.WriteLine("Completed");
        }

        static void Test()
        {
            using (SPSite site = new SPSite(sSiteURL))
            {
                using (SPWeb web = site.OpenWeb(sWebName))
                {
                    //ResoreTask(web.Lists["CompletedTasks"].Items, web.Lists["Tasks"]);
                    SPQuery query = new SPQuery();
                    query.Query = string.Format(@"
                                                    <Where>
                                                            <Contains>
                                                                <FieldRef Name='Title' />
                                                                <Value Type='Text'>{0}</Value>
                                                            </Contains>
                                                    </Where>", sRestoreTitle);
                    BatchDeleteItems(web.Lists["Tasks"], query,web);
                }
            }
        }


        public static void BatchDeleteItems(SPList splTask, SPQuery query,SPWeb web)
        {
            // Set up the variables to be used.
            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";

            string methodFormat = "<Method ID=\"{0}\">" +
                "<SetList Scope=\"Request\">{1}</SetList>" +
                "<SetVar Name=\"ID\">{2}</SetVar>" +
                "<SetVar Name=\"Cmd\">Delete</SetVar>" +
                "</Method>";

            // Get the list containing the items to update.
            //PList list = WorkFlowUtil.GetWorkflowList(listName);

            // Query to get the unprocessed items.

            SPListItemCollection unprocessedItems = splTask.GetItems(query);

            // Build the CAML delete commands.
            foreach (SPListItem item in unprocessedItems)
            {
                methodBuilder.AppendFormat(methodFormat, "1", item.ParentList.ID, item.ID.ToString());
            }

            // Put the pieces together.
            batch = string.Format(batchFormat, methodBuilder.ToString());

            // Process the batch of commands.
            string batchReturn = web.ProcessBatchData(batch.ToString());
        }

    }
}
