namespace CA.WorkFlow.UI.TravelRequest3
{
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using System.Text;
    using System.Collections.Generic;
    using QuickFlow;
    using System.Diagnostics;

    class TravelRequest3Common
    {
        public static void SaveDetails(DataForm dataForm, string workflowNumber)
        {
            BatchInsertItems("Travel Details2", workflowNumber, dataForm.TravelTable);
            BatchInsertItems("Travel Hotel Info2", workflowNumber, dataForm.HotelTable);
            BatchInsertItems("Travel Vehicle Info2", workflowNumber, dataForm.VehicleTable);
            //SPList list = WorkFlowUtil.GetWorkflowList("Travel Details2");
            //SPListItem item = null;
            //foreach (DataRow dr in dataForm.TravelTable.Rows)
            //{
            //    item = list.Items.Add();
            //    item["RequestID"] = workflowNumber;
            //    if (dr["TravelDateFrom"].AsString().IsNotNullOrWhitespace())
            //    {
            //        item["TravelDateFrom"] = dr["TravelDateFrom"];
            //    }
            //    if (dr["TravelDateTo"].AsString().IsNotNullOrWhitespace())
            //    {
            //        item["TravelDateTo"] = dr["TravelDateTo"];
            //    }
            //    item["Area"] = dr["Area"];
            //    item["TravelLocationFrom"] = dr["TravelLocationFrom"];
            //    item["TravelLocationTo"] = dr["TravelLocationTo"];
            //    item["VehicleCostItem"] = dr["VehicleCostItem"];
            //    item["OthersCostItem"] = dr["OthersCostItem"];
            //    item["ReturnVehicleCostItem"] = dr["ReturnVehicleCostItem"];
            //    item["VehicleEstimatedCost"] = dr["VehicleEstimatedCost"];
            //    item["HotelEstimatedCost"] = dr["HotelEstimatedCost"];
            //    item["MealEstimatedCost"] = dr["MealEstimatedCost"];
            //    item["LocalTransportationEstimatedCost"] = dr["LocalTransportationEstimatedCost"];
            //    item["SamplePurchaseCost"] = dr["SamplePurchaseCost"];
            //    item["OtherEstimatedCost"] = dr["OtherEstimatedCost"];
            //    item["TotalEstimatedCost"] = dr["TotalEstimatedCost"];
            //    item["CostCenter"] = dr["CostCenter"];
            //    item["HidCost"] = dr["HidCost"];

            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //}

            //list = WorkFlowUtil.GetWorkflowList("Travel Hotel Info2");
            //foreach (DataRow dr in dataForm.HotelTable.Rows)
            //{
            //    item = list.Items.Add();
            //    item["RequestID"] = workflowNumber;
            //    item["City"] = dr["City"];
            //    item["HotelName"] = dr["HotelName"];
            //    if (dr["CheckInDate"].AsString().IsNotNullOrWhitespace())
            //    {
            //        item["CheckInDate"] = dr["CheckInDate"];
            //    }
            //    if (dr["CheckOutDate"].AsString().IsNotNullOrWhitespace())
            //    {
            //        item["CheckOutDate"] = dr["CheckOutDate"];
            //    }
            //    item["TotalNights"] = dr["TotalNights"];

            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //}

            //list = WorkFlowUtil.GetWorkflowList("Travel Vehicle Info2");
            //foreach (DataRow dr in dataForm.VehicleTable.Rows)
            //{
            //    item = list.Items.Add();
            //    item["RequestID"] = workflowNumber;
            //    item["VehicleCostItem"] = dr["VehicleCostItem"];
            //    if (dr["Date"].AsString().IsNotNullOrWhitespace())
            //    {
            //        item["Date"] = dr["Date"];
            //    }
            //    item["Time"] = dr["Time"];
            //    item["VehicleNumber"] = dr["VehicleNumber"];
            //    item["VehicleFrom"] = dr["VehicleFrom"];
            //    item["VehicleTo"] = dr["VehicleTo"];

            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //}
        }

        public static void DeleteAllDraftItems(string workflowNumber)
        {
            BatchDeleteItems("Travel Details2", workflowNumber);
            BatchDeleteItems("Travel Hotel Info2", workflowNumber);
            BatchDeleteItems("Travel Vehicle Info2", workflowNumber);
        }
        private static void BatchInsertItems(string listName, string workflowNumber, DataTable dt)
        {
            // Set up the variables to be used.
            SPList list = WorkFlowUtil.GetWorkflowList(listName);
            string listGuid = list.ID.ToString();

            StringBuilder methodBuilder = new StringBuilder();
            string batch = string.Empty;
            string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                "<Batch onError=\"Return\">{0}</Batch>";
            string methodFormat;
            switch (listName)
            {
                case "Travel Details2":
                    methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TravelDateFrom\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TravelDateTo\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Area\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TravelLocationFrom\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TravelLocationTo\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VehicleCostItem\">{9}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OthersCostItem\">{10}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ReturnVehicleCostItem\">{11}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VehicleEstimatedCost\">{12}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#HotelEstimatedCost\">{13}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#MealEstimatedCost\">{14}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#LocalTransportationEstimatedCost\">{15}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#SamplePurchaseCost\">{16}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#OtherEstimatedCost\">{17}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalEstimatedCost\">{18}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CostCenter\">{19}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#HidCost\">{20}</SetVar>" +
               "</Method>";

                    // Build the CAML update commands.
                    foreach (DataRow dr in dt.Rows)
                    {
                        methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                            workflowNumber,
                            dr["TravelDateFrom"],
                            dr["TravelDateTo"],
                            dr["Area"],
                            dr["TravelLocationFrom"],
                            dr["TravelLocationTo"],
                            dr["VehicleCostItem"],
                            dr["OthersCostItem"],
                            dr["ReturnVehicleCostItem"],
                            dr["VehicleEstimatedCost"],
                            dr["HotelEstimatedCost"],
                            dr["MealEstimatedCost"],
                            dr["LocalTransportationEstimatedCost"],
                            dr["SamplePurchaseCost"],
                            dr["OtherEstimatedCost"],
                            dr["TotalEstimatedCost"],
                            dr["CostCenter"],
                            dr["HidCost"]
                            );
                    }
                    break;
                case "Travel Hotel Info2":
                    methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#City\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#HotelName\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CheckInDate\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#CheckOutDate\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#TotalNights\">{8}</SetVar>" +
               "</Method>";

                    // Build the CAML update commands.
                    foreach (DataRow dr in dt.Rows)
                    {
                        methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                            workflowNumber,
                            dr["City"],
                            dr["HotelName"],
                            dr["CheckInDate"],
                            dr["CheckOutDate"],
                            dr["TotalNights"]
                            );
                    }
                    break;
                case "Travel Vehicle Info2":
                    methodFormat = "<Method ID=\"{0}\">" +
               "<SetList Scope=\"Request\">{1}</SetList>" +
               "<SetVar Name=\"ID\">{2}</SetVar>" +
               "<SetVar Name=\"Cmd\">Save</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Title\">{3}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VehicleCostItem\">{4}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Date\">{5}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#Time\">{6}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VehicleNumber\">{7}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VehicleFrom\">{8}</SetVar>" +
               "<SetVar Name=\"urn:schemas-microsoft-com:office:office#VehicleTo\">{9}</SetVar>" +
               "</Method>";

                    // Build the CAML update commands.
                    foreach (DataRow dr in dt.Rows)
                    {
                        methodBuilder.AppendFormat(methodFormat, 0, listGuid, "New",
                            workflowNumber,
                            dr["VehicleCostItem"],
                            dr["Date"],
                            dr["Time"],
                            dr["VehicleNumber"],
                            dr["VehicleFrom"],
                            dr["VehicleTo"]
                           );
                    }
                    break;
                default:
                    break;
            }

            if (methodBuilder.ToString().IsNotNullOrWhitespace())
            {
                // Put the pieces together.
                batch = string.Format(batchFormat, methodBuilder.ToString());

                // Process the batch of commands.
                string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);

                //SPSecurity.RunWithElevatedPrivileges(delegate {
                //    if (!EventLog.SourceExists("C&A"))
                //    {
                //        EventLog.CreateEventSource("C&A", "Mail");
                //    }

                //    // Create an EventLog instance and assign its source.
                //    EventLog myLog = new EventLog();
                //    myLog.Source = "C&A";

                //    // Write an informational entry to the event log.    
                //    myLog.WriteEntry("Tag Travel Requeset batch insert:"+batchReturn, EventLogEntryType.Information);    
                //});
            }
        }
        private static void BatchDeleteItems(string listName, string workflowNumber)
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
            SPList list = WorkFlowUtil.GetWorkflowList(listName);

            // Query to get the unprocessed items.
            SPQuery query = new SPQuery();
            query.Query = @"<Where>
                                <Eq>
                                    <FieldRef Name='Title' />
                                    <Value Type='Text'>" + workflowNumber + @"</Value>
                                </Eq>
                            </Where>";
            query.ViewAttributes = "Scope='Recursive'";
            SPListItemCollection unprocessedItems = list.GetItems(query);

            // Build the CAML delete commands.
            foreach (SPListItem item in unprocessedItems)
            {
                methodBuilder.AppendFormat(methodFormat, "1", item.ParentList.ID, item.ID.ToString());
            }

            // Put the pieces together.
            batch = string.Format(batchFormat, methodBuilder.ToString());

            // Process the batch of commands.
            string batchReturn = SPContext.Current.Web.ProcessBatchData(batch.ToString());
        }

        /*
         * 
         */
        public static List<string> GetMailMembers(params string[] personTypes)
        {
            List<string> members = new List<string>();

            var qType = new QueryField("PersonType", false);
            CamlExpression exp = null;
            foreach (var type in personTypes)
            {
                exp = WorkFlowUtil.LinkOr(exp, qType.Equal(type));
            }

            SPListItemCollection lc = null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                            .From(web.Lists["MailMember"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });

            foreach (SPListItem item in lc)
            {
                members.Add(item["Account"].AsString());
            }
            return members;
        }

        public static bool isSpecialLevel(string applicantStr)
        {
            var account = WorkFlowUtil.GetApplicantAccount(applicantStr);

            var qType = new QueryField("Title", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkOr(exp, qType.Equal(account));

            SPListItemCollection lc = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                            .From(web.Lists["LevelList"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });

            return lc.Count > 0;
        }

        public static List<string> ConvertToList(NameCollection coll)
        {
            List<string> list = new List<string>();
            foreach (var temp in coll)
            {
                list.Add(temp);
            }
            return list;
        }

        public static bool IsLastTask(string listId, string itemId)
        {
            int count = 0;
            SPListItemCollection coll = null;
            coll = GetAllTasks(listId, itemId);
            foreach (SPListItem item in coll)
            {
                if (item["Status"].AsString().Equals("Not Started", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    count++;
                }
            }
            return count == 1;
        }

        public static SPListItemCollection GetAllTasks(string listId, string itemId)
        {
            var qWorkflowListId = new QueryField("WorkflowListId", false);
            var qWorkflowItemId = new QueryField("WorkflowItemId", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qWorkflowListId.Equal(listId));
            exp = WorkFlowUtil.LinkAnd(exp, qWorkflowItemId.Equal(itemId));
            SPListItemCollection lc = null;

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        lc = ListQuery.Select()
                            .From(web.Lists["Tasks"])
                            .Where(exp)
                            .GetItems();
                    }
                }
            });
            return lc;
        }

        public static bool IsCEO(string userAccount)
        {
            bool isCEO = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPGroup group = web.Groups["wf_CEO"];
                        foreach (SPUser user in group.Users)
                        {
                            if (user.LoginName.Equals(userAccount, System.StringComparison.CurrentCultureIgnoreCase))
                            {
                                isCEO = true;
                                break;
                            }
                        }
                    }
                }
            });
            return isCEO;
        }

        /// <summary>
        /// 返回用户名组
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="mName">模块名</param>
        /// <returns></returns>
        internal static NameCollection GetTaskUsersByModuleWithoutDeleman(string group, string module)
        {
            string moduleID = WorkFlowUtil.GetModuleIdByListName(module);
            return WorkFlowUtil.GetTaskUsersWithoutDeleman(group, moduleID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="managerAccount"></param>
        /// <param name="mName"></param>
        /// <returns></returns>
        internal static NameCollection GetTaskUsersByModule(NameCollection manager, string managerAccount, string module)
        {
            manager.Add(managerAccount);
            string moduleID = WorkFlowUtil.GetModuleIdByListName(module);
            var deleman = WorkFlowUtil.GetDeleman(managerAccount, moduleID);
            if (deleman != null){
                manager.Add(deleman);
            }

            return manager;
        }

       /// <summary>
        /// 根据组和模块返回用户
       /// </summary>
       /// <param name="group">组名<param>
       /// <param name="module">模块名</param>
       /// <returns>用户集合</returns>
        internal static NameCollection GetTaskUsersByModuleAndGroup(string group, string module)
        {
            string moduleID = WorkFlowUtil.GetModuleIdByListName(module);
            return WorkFlowUtil.GetTaskUsers(group, moduleID);
        }
    }
}