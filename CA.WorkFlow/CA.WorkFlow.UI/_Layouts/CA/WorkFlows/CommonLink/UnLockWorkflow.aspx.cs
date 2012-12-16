using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint.Workflow;

namespace CA.WorkFlow.UI.CommonLink
{
    public partial class UnLockWorkflow : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUnlockWF_Click(object sender, EventArgs e)
        {
            string wfListName = txtWFListName.Text.Trim();
            string wfNo = txtWFNo.Text.Trim();

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            SPList list = web.Lists[wfListName];
                            if (list != null)
                            {
                                SPListItem wfItem = GetWFItem(list, wfNo);

                                if (wfItem != null)
                                {
                                    UnLockWorkflowTasks(wfItem);
                                }
                                else
                                {
                                    DisplayMessage(string.Format("Sorry, the workflow number({0}) does not exist, please resubmit it.", wfNo));
                                }
                            }
                            else
                            {
                                DisplayMessage(string.Format("Sorry, the list({1}) does not exist, please resubmit it.", wfListName));
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                DisplayMessage(string.Format("Unlock workflow tasks fails, please try it again.\n More details:{0}", ex.Message));
            }

        }

        private void UnLockWorkflowTasks(SPListItem wfItem)
        {
            bool isNeedUnlock = false;
            foreach (SPWorkflow workflow in wfItem.Workflows)
            {
                foreach (SPWorkflowTask task in workflow.Tasks)
                {
                    if (task[SPBuiltInFieldId.WorkflowVersion].AsString() != "1")
                    {
                        isNeedUnlock = true;
                        task[SPBuiltInFieldId.WorkflowVersion] = "1";
                        task.SystemUpdate();
                    }
                }
            }

            if (isNeedUnlock)
            {
                DisplayMessage(string.Format("The workflow {0} is unlocked now.", wfItem["Title"].AsString()));
            }
            else
            {
                DisplayMessage(string.Format("The workflow {0} was not locked before.", wfItem["Title"].AsString()));
            }
        }

        private SPListItem GetWFItem(SPList list, string wfNo)
        {
            SPListItem wfItem = null;

            foreach (SPListItem item in list.Items)
            {
                if (item["Title"].AsString() == wfNo)
                {
                    wfItem = item;
                    break;
                }
            }

            return wfItem;
        }
    }
}