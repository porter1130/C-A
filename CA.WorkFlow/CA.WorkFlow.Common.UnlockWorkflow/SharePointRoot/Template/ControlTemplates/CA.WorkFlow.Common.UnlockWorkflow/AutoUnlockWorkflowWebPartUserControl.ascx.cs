using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using Microsoft.SharePoint.Workflow;
using CA.WorkFlow.Common.UnlockWorkflow.Utility;
using CA.WorkFlow.Common.UnlockWorkflow.Serialization;
using CA.SharePoint.CamlQuery;
using System.Collections;
using CA.SharePoint.Utilities.Common;
using System.Threading;
using System.Globalization;

namespace CA.WorkFlow.Common.UnlockWorkflow.UI
{
    public partial class AutoUnlockWorkflowWebPartUserControl : UserControl
    {
        protected SPList currList = SPContext.Current.List;
        protected List<int> excutedTaskList = new List<int>();
        protected string lastTaskCompleteDate = string.Empty;
        protected string CancelTask_StartDate = string.Empty;
        protected string CancelTask_EndDate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            if (ddlWFName.Items.Count == 0)
            {
                WFNameBind();
            }
        }

        private void WFNameBind()
        {
            ListItemCollection wfNameItems = new ListItemCollection();
            if (currList != null)
            {
                foreach (SPWorkflowAssociation association in currList.WorkflowAssociations)
                {
                    wfNameItems.Add(new ListItem(currList.WorkflowAssociations[association.Id].Name, association.Id.ToString()));
                }
            }

            if (wfNameItems.Count > 0)
            {
                ddlWFName.DataSource = wfNameItems;
                ddlWFName.DataTextField = "Text";
                ddlWFName.DataValueField = "Value";
                ddlWFName.SelectedIndex = wfNameItems.Count - 1;
                ddlWFName.DataBind();
            }

        }

        protected void btnUnlock_Click(object sender, EventArgs e)
        {

            if (!SPContext.Current.Web.CurrentUser.IsSiteAdmin)
            {
                JavascriptHelper.Alert(this.Page, "Sorry, You do not have permission to complete this action, please contact site administrator.", "unlockingWFDisabled");
                return;
            }

            string wfAssociationIdStr = ddlWFName.SelectedValue;

            if (!string.IsNullOrEmpty(wfAssociationIdStr))
            {
                Queue<string> unCompletedWFItems;

                FindUnCompletedWFItem(new Guid(wfAssociationIdStr), out unCompletedWFItems);

                if (unCompletedWFItems.Count > 0)
                {
                    AutoUnlockWF(new Guid(wfAssociationIdStr), unCompletedWFItems);
                }
            }

        }

        private void AutoUnlockWF(Guid wfAssociationId, Queue<string> unCompletedWFItems)
        {
            SPListItem workflowItem, eventDataItem;

            while (unCompletedWFItems.Count > 0)
            {
                string wfNumber = unCompletedWFItems.Dequeue();

                GetSpecificWFItem(wfNumber, out workflowItem, out eventDataItem);

                if (workflowItem != null
                    && eventDataItem != null
                    && !eventDataItem[SPFieldName.UnlockHistory].AsString().Equals("Successful"))
                {
                    try
                    {
                        string[] split = new string[] { "#;" };
                        SPContext.Current.Web.AllowUnsafeUpdates = true;
                        List<WorkflowTask> oldTasks = SaveOldTasks(eventDataItem, workflowItem.Tasks);
                        CancelWorkflowInstance(workflowItem, wfAssociationId);


                        if (eventDataItem.Fields.ContainsField(SPFieldName.EventData))
                        {

                            string[] eventDatas = eventDataItem[SPFieldName.EventData].AsString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            string newWfInstanceId = string.Empty;

                            for (int i = 0; i < eventDatas.Length; i++)
                            {
                                if (i == 0)
                                {
                                    if (StartWorkflowInstance(workflowItem, eventDatas[i], out newWfInstanceId) == null)
                                        break;
                                }
                                else
                                {
                                    if (!ResumeWorkflow(workflowItem, oldTasks, eventDatas[i], newWfInstanceId))
                                        break;
                                }
                            }

                            if (eventDatas.Length > 1)
                            {
                                UpdateNotStartedTasksStartDate(workflowItem, newWfInstanceId);
                            }

                        }

                        SPContext.Current.Web.AllowUnsafeUpdates = false;
                        this.lbResault.Text += string.Format("  {0}", wfNumber);
                        SaveUnlockWFHistory(wfNumber, unCompletedWFItems);
                        eventDataItem[SPFieldName.UnlockHistory] = "Successful";
                        eventDataItem[SPFieldName.HasException] = 0;
                        eventDataItem.Update();

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        eventDataItem[SPFieldName.UnlockHistory] = string.Format("Error Message:{0}, Stack Trace:{1}", ex.Message, ex.StackTrace);
                        eventDataItem[SPFieldName.HasException] = 1;
                        eventDataItem.Update();
                        SPContext.Current.Web.AllowUnsafeUpdates = false;
                    }
                }
            }

        }

        private void SaveUnlockWFHistory(string wfNumber, Queue<string> unCompletedWFItems)
        {
            SPList list = SPContext.Current.Web.Lists[SPListName.AutoUnlockWorkflowHistory];

            QueryField titleField = new QueryField("Title");

            CAMLExpression<object> exps = titleField.Equal(currList.Title);

            SPQuery query = new SPQuery();
            query.Query = CAMLBuilder.Where(exps);

            SPListItemCollection items = list.GetItems(query);

            if (items.Count > 0)
            {
                string UnlockedWFNo = items[0][SPFieldName.UnlockedWFNo].AsString();

                List<string> UnlockedWFNoList = new List<string>();
                if (UnlockedWFNo.IsNotNullOrWhitespace())
                {
                    UnlockedWFNoList.AddRange(UnlockedWFNo.Split(';'));
                }

                if (!UnlockedWFNoList.Contains(wfNumber))
                {
                    UnlockedWFNoList.Add(wfNumber);
                    SPListItem unLockedWFNoItem = items[0];
                    unLockedWFNoItem[SPFieldName.UnlockedWFNo] = string.Join(";", UnlockedWFNoList.ToArray());
                    unLockedWFNoItem.Update();
                }

            }
            else
            {
                SPListItem item = list.Items.Add();
                item[SPBuiltInFieldId.Title] = currList.Title;
                item[SPFieldName.NeedUnlockedWFNo] = string.Join(";", unCompletedWFItems.ToArray());
                item.Update();
            }
        }


        private void UpdateNotStartedTasksStartDate(SPListItem workflowItem, string newWfInstanceId)
        {
            Guid associationId = new Guid(newWfInstanceId);
            SPWorkflow workflow = workflowItem.Workflows[associationId];
            foreach (SPWorkflowTask task in workflow.Tasks)
            {
                if (task[SPBuiltInFieldId.TaskStatus].AsString() == "Not Started")
                {
                    if (lastTaskCompleteDate.IsNotNullOrWhitespace())
                    {
                        task[SPBuiltInFieldId.StartDate] = lastTaskCompleteDate;
                        task.SystemUpdate();
                    }
                }
            }
        }

        private bool ResumeWorkflow(SPListItem workflowItem, List<WorkflowTask> oldTasks, string eventDriven, string newWfAssociationId)
        {
            bool isSuccessful = false;
            Guid associationId = new Guid(newWfAssociationId);
            using (SPSite osite = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb oweb = osite.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPListItem oitem = oweb.Lists[currList.Title].GetItemById(workflowItem.ID);
                    Hashtable hash;

                    SPWorkflow workflow = oitem.Workflows[associationId];

                    foreach (SPWorkflowTask newWorkflowTask in workflow.Tasks)
                    {
                        if (!excutedTaskList.Contains(newWorkflowTask.ID))
                        {
                            if (GetSpecificTaskItem(newWorkflowTask, oldTasks, eventDriven, out hash))
                            {
                                if (SPWorkflowTask.AlterTask(newWorkflowTask as SPListItem, hash, true))
                                {
                                    AddExcutedTaskList(workflow.Tasks);
                                    isSuccessful = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return isSuccessful;
        }

        private void AddExcutedTaskList(SPWorkflowTaskCollection tasks)
        {
            foreach (SPWorkflowTask task in tasks)
            {
                if (!excutedTaskList.Contains(task.ID))
                {
                    excutedTaskList.Add(task.ID);
                    task[SPBuiltInFieldId.StartDate] = CancelTask_StartDate;
                    task[SPBuiltInFieldId.TaskDueDate] = CancelTask_EndDate;
                    task.SystemUpdate();
                }
            }
        }

        private bool GetSpecificTaskItem(SPWorkflowTask newWorkflowTask, List<WorkflowTask> oldTasks, string eventDriven, out Hashtable hash)
        {
            bool isSpecific = false;
            hash = new Hashtable();

            SPFieldUserValue newTaskUserValue = new SPFieldUserValue(newWorkflowTask.Web, newWorkflowTask[SPBuiltInFieldId.AssignedTo].AsString());

            foreach (WorkflowTask oldTask in oldTasks)
            {
                SPFieldUserValue oldTaskUserValue = new SPFieldUserValue(newWorkflowTask.Web, oldTask[WorkflowTaskFieldName.AssignedTo].AsString());
                if (newTaskUserValue.User.ID.Equals(oldTaskUserValue.User.ID)
                    && oldTask[WorkflowTaskFieldName.Status] == "Completed")
                {
                    string outCome = oldTask[WorkflowTaskFieldName.Outcome].AsString();
                    CancelTask_StartDate = oldTask[WorkflowTaskFieldName.StartDate].AsString();
                    CancelTask_EndDate = oldTask[WorkflowTaskFieldName.CompleteDate].AsString();

                    hash.Add("__TaskOutcome", outCome);
                    hash.Add("__Action", "Commit");
                    hash.Add("__WorkflowVaribales", eventDriven);
                    hash.Add("StartDate", oldTask[WorkflowTaskFieldName.StartDate].AsString());
                    hash.Add("DueDate", oldTask[WorkflowTaskFieldName.CompleteDate].AsString());
                    hash.Add("Body", oldTask[WorkflowTaskFieldName.Comments].AsString());
                    //hash.Add("AssignedTo", oldTaskUserValue.User);

                    isSpecific = true;

                    oldTasks.Remove(oldTask);
                    break;
                }
            }

            return isSpecific;
        }

        private SPWorkflow StartWorkflowInstance(SPListItem workflowItem, string eventData, out string newWfInstanceId)
        {
            SPWorkflow newWorkflowInstance = null;

            SPFieldUserValue userValue = new SPFieldUserValue(SPContext.Current.Web, workflowItem[SPBuiltInFieldId.Author].AsString());

            using (SPSite osite = new SPSite(SPContext.Current.Site.ID, userValue.User.UserToken))
            {
                using (SPWeb oweb = osite.OpenWeb(SPContext.Current.Web.ID))
                {

                    SPList olist = oweb.Lists[currList.Title];
                    SPListItem oitem = olist.GetItemById(workflowItem.ID);
                    SPWorkflowAssociation wfAss = olist.WorkflowAssociations.GetAssociationByName(ddlWFName.SelectedItem.Text, CultureInfo.CurrentCulture);
                    oweb.AllowUnsafeUpdates = true;
                    newWorkflowInstance = osite.WorkflowManager.StartWorkflow(oitem, wfAss, eventData);
                    oweb.AllowUnsafeUpdates = false;
                    newWfInstanceId = newWorkflowInstance.InstanceId.ToString();
                }
            }

            return newWorkflowInstance;
        }


        private void CancelWorkflowInstance(SPListItem workflowItem, Guid wfAssociationId)
        {
            SPWorkflowManager wfManager = SPContext.Current.Site.WorkflowManager;

            //SPWorkflowManager.CancelWorkflow(workflow);
            foreach (SPWorkflow workflow in workflowItem.Workflows)
            {
                if (workflow.AssociationId.Equals(wfAssociationId))
                {
                    wfManager.RemoveWorkflowFromListItem(workflow);
                    break;
                }
            }
        }

        private List<WorkflowTask> SaveOldTasks(SPListItem eventDataItem, SPWorkflowTaskCollection oldTasks)
        {
            SourceTasks sourceTasks = new SourceTasks();
            if (eventDataItem[SPFieldName.TasksHistory].AsString().IsNullOrWhitespace())
            {

                foreach (SPWorkflowTask task in oldTasks)
                {
                    if (task[SPBuiltInFieldId.TaskStatus].AsString() == "Completed")
                    {
                        lastTaskCompleteDate = task[SPBuiltInFieldId.Modified].AsString();
                    }

                    WorkflowTask workflowTask = new WorkflowTask();
                    workflowTask.Add(WorkflowTaskFieldName.Title, task.Title);
                    workflowTask.Add(WorkflowTaskFieldName.AssignedTo, task[SPBuiltInFieldId.AssignedTo].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.Status, task[SPBuiltInFieldId.TaskStatus].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.Created, task[SPBuiltInFieldId.Created].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.CreatedBy, task[SPBuiltInFieldId.Author].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.StartDate, task[SPBuiltInFieldId.StartDate].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.CompleteDate, task[SPBuiltInFieldId.Modified].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.Outcome, task[SPBuiltInFieldId.WorkflowOutcome].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.Comments, task[SPBuiltInFieldId.Body].AsString());
                    workflowTask.Add(WorkflowTaskFieldName.UIVersion, task[SPBuiltInFieldId._UIVersion].AsString());

                    Hashtable extendedPropertiesAsHashtable = SPWorkflowTask.GetExtendedPropertiesAsHashtable(task);
                    workflowTask.Add(WorkflowTaskFieldName.WorkflowVariables, extendedPropertiesAsHashtable["__WorkflowVaribales"].AsString().Replace('\"', '\''));

                    sourceTasks.tasks.Add(workflowTask);
                }

                eventDataItem[SPFieldName.TasksHistory] = SerializeUtil.Serialize(sourceTasks);
                eventDataItem.Update();
            }
            else
            {
                sourceTasks = SerializeUtil.Deserialize(typeof(SourceTasks), eventDataItem[SPFieldName.TasksHistory].AsString()) as SourceTasks;
                foreach (WorkflowTask sourceTask in sourceTasks.tasks)
                {
                    if (sourceTask[WorkflowTaskFieldName.Status].AsString() == "Completed")
                    {
                        lastTaskCompleteDate = sourceTask[WorkflowTaskFieldName.CompleteDate].AsString();
                    }
                }
            }


            return sourceTasks.tasks;

        }

        private void GetSpecificWFItem(string wfNo, out SPListItem workflowItem, out SPListItem eventDataItem)
        {
            workflowItem = null;
            eventDataItem = null;

            QueryField titleField = new QueryField("Title");

            CAMLExpression<object> exp = titleField.Equal(wfNo);

            SPQuery query = new SPQuery();
            query.Query = CAMLBuilder.Where(exp);

            SPListItemCollection items = currList.GetItems(query);

            if (items.Count > 0)
            {

                workflowItem = items[0];

                QueryField listNameField = new QueryField("WorkflowListName");
                QueryField idField = new QueryField("Title");

                CAMLExpression<object> exps = idField.Equal(workflowItem.Title) && listNameField.Equal(currList.Title);

                SPList eventDatalist = SPContext.Current.Web.Lists[SPListName.UnlockWorkflow];

                SPQuery eventDataQuery = new SPQuery();

                eventDataQuery.Query = CAMLBuilder.Where(exps);

                SPListItemCollection eventDataItems = eventDatalist.GetItems(eventDataQuery);
                if (eventDataItems.Count > 0)
                {
                    eventDataItem = eventDataItems[0];
                }
            }
        }

        private void FindUnCompletedWFItem(Guid wfAssociationId, out Queue<string> unCompletedWFItems)
        {
            unCompletedWFItems = new Queue<string>();
            foreach (SPListItem item in currList.Items)
            {
                foreach (SPWorkflow workflow in SPContext.Current.Site.WorkflowManager.GetItemActiveWorkflows(item))
                {

                    if (workflow.AssociationId.Equals(wfAssociationId)
                        && !workflow.IsCompleted)
                    {
                        unCompletedWFItems.Enqueue(item.Title);
                        break;
                    }
                }
            }
        }
    }
}
