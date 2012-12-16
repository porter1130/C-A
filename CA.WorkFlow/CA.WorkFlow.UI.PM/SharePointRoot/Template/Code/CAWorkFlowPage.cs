using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using QuickFlow.Core;
using CA.Web;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace CA.WorkFlow.UI
{
    public class CAWorkFlowPage:Page
    { 
        public SPContext CurrentContext
        {
            get
            {
                return SPContext.Current;
            }
        }

        public SPList CurrentList
        {
            get
            {
                return CurrentContext.List;
            }
        }
        private string str = string.Empty;
        public string TaskOutcome
        {   
            get
            {
                return  str;
            }
            set
            {
               this.str=value;
            }
        }

        public SPListItem CurrentListItem
        {
            get
            {
                return CurrentContext.ListItem;
            }
        }

        private SPControlMode _ControlMode = SPControlMode.Invalid;
        public virtual SPControlMode ControlMode
        {
            get
            {
                return this._ControlMode;
            }
            set
            {
                this._ControlMode = value;
            }
        }

        private Employee _CurrentEmployee=null;
        public Employee CurrentEmployee
        {
            get
            {
                if (_CurrentEmployee == null)
                {
                    try
                    {
                        this.ViewState["CA_CurrentEmployee"] = UserProfileUtil.GetEmployee(SPContext.Current.Web.CurrentUser.LoginName);
                        _CurrentEmployee = (Employee)this.ViewState["CA_CurrentEmployee"];
                    }
                    catch {
                        _CurrentEmployee = new Employee { DisplayName = SPContext.Current.Web.CurrentUser.Name };
                    }                  
                }
                return _CurrentEmployee;
            }
        }

        private Script _script;
        public Script Script
        {
            get
            {
                if (_script == null)
                    _script = new Script(this);
                return _script;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SPContext.Current.FormContext.SetFormMode(this.ControlMode, true);

        }

        public virtual void SetControlMode()
        {
            SetMode(this);
        }

        protected virtual void SetMode(Control ctl)
        {
            if (ctl.Controls.Count > 0)
            {
                foreach (Control tmp in ctl.Controls)
                {
                    if (tmp is BaseFieldControl)
                    {
                        ((BaseFieldControl)tmp).ControlMode = this.ControlMode;
                        continue;
                    }
                    else if (tmp is FormComponent)
                    {
                        ((FormComponent)tmp).ControlMode = this.ControlMode;
                    }
                    else if (tmp is QFUserControl)
                    {
                        ((QFUserControl)tmp).ControlMode = this.ControlMode;
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {          
           // AddPermisson();
            base.OnLoad(e);           
        }

        protected override void OnPreInit(EventArgs e)
        {
            CodeArt.SharePoint.MultiLanSupport.UICultureManager.CurrentInstance.SetThreadCulture();
            base.OnPreInit(e);

            ApplyMaster();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e); 
            ApplyWaitUI();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);            
        }

        protected virtual void ApplyWaitUI()
        {
            this.Form.Attributes.Add("onsubmit", "return CAShowWaitUI();");
        }

        protected virtual void HideWaitUI()
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "hidDiv", "HideWaitUI();");
        }

        /// <summary>
        /// 将站点模板页应用到页面
        /// </summary>
        protected virtual void ApplyMaster()
        {
            //if (SPContext.Current != null)
            //{
            //    this.MasterPageFile = SPContext.Current.Web.CustomMasterUrl;
            //    return;
            //}

        }

        protected void DisplayMessage(string msg)
        {
            string script = "alert('" + msg + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);

            //this.Script.Alert(msg); 用这个就可以
        }

     


        protected virtual void Back()
        {
            //if (Page.Request.QueryString["Source"] != null)
            //    Page.Response.Redirect(Page.Request.QueryString["Source"]);
            //else
            Thread.Sleep(3000);
            Page.Response.Redirect(@"/ca/Mytasks.aspx");
        }
        

        public readonly PermissionSet PermissionSet = new PermissionSet();

        protected virtual void AddPermisson()
        {
            PermissionSet.Add(HttpContext.Current.User.Identity.Name, PermissionType.Edit);
        }

        protected void UpdatePermissions()
        {
            PermissionSet permissionSet = this.PermissionSet;

            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                   {
                       using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                       {
                           SPList list = web.Lists[this.CurrentList.ID];

                           SPListItem item = list.GetItemById(Convert.ToInt32(WorkflowContext.Current.DataFields["ID"]));
                           try
                           {
                               item.BreakRoleInheritance(true);
                           }
                           catch { }
                           item.ParentList.ParentWeb.AllowUnsafeUpdates = true;
                           //算法应该优化...
                           for (int index = item.RoleAssignments.Count - 1; index >= 0; index--)
                           {
                               SPRoleAssignment ra = item.RoleAssignments[index];

                               // 如果某个RoleAssignment的Member为系统管理员, 则不要删除之
                               if (ra.Member.Name.ToLower() == "SHAREPOINT\\system".ToLower()) continue;

                               if (ra.Member is SPGroup)
                               {
                                   item.RoleAssignments.Remove(index);
                               }
                               else if (ra.Member is SPUser)
                               {
                                   SPUser user = (SPUser)ra.Member;
                                   if (user.IsDomainGroup)
                                   {
                                       item.RoleAssignments.Remove(index);
                                   }
                                   else if (HasEditPermission(ra))
                                   {
                                       permissionSet.Add(user.LoginName, PermissionType.Edit);
                                       item.RoleAssignments.Remove(index);
                                   }
                               }
                               else//删除所有管理权限
                               {
                                   //permissionSet.Add( ra.Member.Name, PermissionType.View);
                                   item.RoleAssignments.Remove(index);
                               }

                               //item.RoleAssignments.Remove(index);
                           }

                           //添权限
                           foreach (Permission p in permissionSet)
                           {
                               if (p.PermissionType == PermissionType.Edit)
                               {
                                   PermissionUtil.AddManageListItemPermission(item, p.Identity);
                               }
                               else
                               {
                                   PermissionUtil.AddViewListItemPermission(item, p.Identity);
                               }
                           }
                       }
                   }

               });
        }
        

        private bool HasEditPermission(SPRoleAssignment ra)
        {
            foreach (SPRoleDefinition ed in ra.RoleDefinitionBindings)
            {
                if ((ed.BasePermissions & SPBasePermissions.EditListItems) == SPBasePermissions.EditListItems ||
                    (ed.BasePermissions & SPBasePermissions.DeleteListItems) == SPBasePermissions.DeleteListItems)
                {
                    return true;
                }
            }
            return false;
        }

        //Validate whether the task and item are consistent. If no, return false.
        protected bool SecurityValidate(string uTaskId, string uListGUID, string uId, bool isCheckUser)
        {
            bool isValid = false;
            uListGUID = "{" + uListGUID + "}";
            SPList list = WorkFlowUtil.GetWorkflowList("Tasks");
            SPListItem lc = list.GetItemById(Convert.ToInt32(uTaskId));
            int id = Convert.ToInt32(lc["WorkflowItemId"]);
            string listGUID = lc["WorkflowListId"] as string;
            isValid = (id == Convert.ToInt32(uId)) && String.Equals(uListGUID, listGUID, StringComparison.CurrentCultureIgnoreCase);
            if (isCheckUser)
            {
                string assignTo = lc["AssignedTo"].ToString();
                string currUser = SPContext.Current.Web.CurrentUser.Name;
                isValid = assignTo.Contains(currUser);
            }
            return isValid;
        }

        //Save the current approver into the "Approvers" column
        protected void SaveToApprovers()
        {
            //format: ca\test1;ca\test2;
            string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString();
            string currentAcc = SPContext.Current.Web.CurrentUser.LoginName;
            if (!approvers.Contains(currentAcc + ";"))
            {
                approvers += currentAcc + ";";
                WorkflowContext.Current.DataFields["Approvers"] = approvers;
            }
        }

        /**
         * 返回审批人和一组新帐号的合集，如果帐号在审批人字段中已经存在，则不需要重复添加
         * 审批人字段中存储的类型为文本
         */
        protected string ReturnAllApprovers(params string[] accounts)
        {
            //format: ca\test1;ca\test2;
            string approvers = WorkflowContext.Current.DataFields["Approvers"].AsString();

            foreach (var account in accounts)
            {
                if (!approvers.Contains(account + ";"))
                {
                    approvers += account + ";";
                }
            }
            
            return approvers;
        }

        /**
         * 将一组用户帐号添加到字段中，如果字段中已包含该帐号则不用重复添加
         * 添加字段为People Group类型，返回类型同样为People Group类型
         */
        protected SPFieldUserValueCollection ReturnAllApproversSP(string approverCol, params string[] accounts)
        {
            var web = SPContext.Current.Web;
            SPUser user = null;
            SPFieldUserValue spUser = null;
            SPFieldUserValueCollection approvers = WorkflowContext.Current.DataFields[approverCol] as SPFieldUserValueCollection;

            foreach (var account in accounts)
            {
                user = web.Users[account];
                spUser = new SPFieldUserValue(web, user.ID, user.Name);
                
                if (approvers == null)
                {
                    approvers = new SPFieldUserValueCollection();
                }
                if (!approvers.Contains(spUser))
                {
                    approvers.Add(spUser);
                }
            }
            

            return approvers;
        }

        protected void SaveToApprovers(string currentStepApproversField, string approvedLoginNamesField, string approvedSPFieldUserValueCollField)
        {
            var rootweb = SPContext.Current.Site.RootWeb;
            var web = SPContext.Current.Web;
            //var item = SPContext.Current.ListItem;
            var fields = WorkflowContext.Current.DataFields;
            var curUser = web.CurrentUser;

            //审批过该条单子的人集合
            var approverList = new List<string>();
            var approvers = fields[approvedLoginNamesField].AsString();
            var userValueColl = fields[approvedSPFieldUserValueCollField] as SPFieldUserValueCollection;
            if (userValueColl == null)
            {
                userValueColl = new SPFieldUserValueCollection();
            }

            if (!string.IsNullOrEmpty(approvers))
            {
                approverList = approvers.Split(';').ToList<string>();
            }

            //新步骤的审批人
            var currentStepApprovers = fields[currentStepApproversField].AsString();

            //加上新步骤的审批人
            if (!string.IsNullOrEmpty(currentStepApprovers))
            {
                foreach (var currentStepApprover in currentStepApprovers.Split(';'))
                {
                    if (!string.IsNullOrEmpty(currentStepApprover) && !approverList.Contains(currentStepApprover))
                    {
                        try
                        {
                            var spUser = web.Users[currentStepApprover];
                            approverList.Add(currentStepApprover);
                            userValueColl.Add(new SPFieldUserValue(rootweb, spUser.ID, spUser.Name));
                        }
                        catch (Microsoft.SharePoint.SPException)
                        { }
                    }
                }

            }

            //加上当前审批者（当前审批者是某个审批人的代理的场合）
            if (!approverList.Contains(curUser.LoginName))
            {
                try
                {
                    approverList.Add(curUser.LoginName);
                    userValueColl.Add(new SPFieldUserValue(rootweb, curUser.ID, curUser.Name));
                }
                catch (Microsoft.SharePoint.SPException)
                { }
            }

            fields[approvedLoginNamesField] = string.Join(";", approverList.ToArray());
            fields[approvedSPFieldUserValueCollField] = userValueColl;
            
        }

        protected void SaveToApprovers(string currentStepApproversField, string approvedSPFieldUserValueCollField)
        {
            var rootweb = SPContext.Current.Site.RootWeb;
            var web = SPContext.Current.Web;
            //var item = SPContext.Current.ListItem;
            var fields = WorkflowContext.Current.DataFields;
            var curUser = web.CurrentUser;

            //审批过该条单子的人集合
            var approverList = new List<string>();
            var userValueColl = fields[approvedSPFieldUserValueCollField] as SPFieldUserValueCollection;
            if (userValueColl == null)
            {
                userValueColl = new SPFieldUserValueCollection();
            }

            //新步骤的审批人
            var currentStepApprovers = fields[currentStepApproversField].AsString();

            //加上新步骤的审批人
            if (!string.IsNullOrEmpty(currentStepApprovers))
            {
                foreach (var currentStepApprover in currentStepApprovers.Split(';'))
                {
                    if (!string.IsNullOrEmpty(currentStepApprover) && !approverList.Contains(currentStepApprover))
                    {
                        try
                        {
                            var spUser = web.Users[currentStepApprover];
                            approverList.Add(currentStepApprover);
                            userValueColl.Add(new SPFieldUserValue(rootweb, spUser.ID, spUser.Name));
                        }
                        catch (Microsoft.SharePoint.SPException)
                        { }
                    }
                }

            }

            //加上当前审批者（当前审批者是某个审批人的代理的场合）
            if (!approverList.Contains(curUser.LoginName))
            {
                try
                {
                    approverList.Add(curUser.LoginName);
                    userValueColl.Add(new SPFieldUserValue(rootweb, curUser.ID, curUser.Name));
                }
                catch (Microsoft.SharePoint.SPException)
                { }
            }

            fields[approvedSPFieldUserValueCollField] = userValueColl;

        }

        //Add one account to list
        protected void AddToEmployees(List<Employee> employees, Employee e)
        {
            bool isExist = false;
            foreach (Employee emp in employees)
            {
                if (emp.UserAccount.Equals(e.UserAccount, StringComparison.CurrentCultureIgnoreCase))
                {
                    isExist = true;
                    break;
                }
            }
            if (!isExist)
            {
                employees.Add(e);
            }
        }

        protected void RedirectToTask()
        {
            this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
        }

        protected void SendNotificationMail(string templateName, List<string> parameters, List<string> to, bool toApplicant)
        {
            var emailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(templateName);
            if (emailTemplate == null)
            {
                return;
            }
            string bodyTemplate = emailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string subject = emailTemplate["Subject"].AsString();

                WorkFlowUtil.SendMail(toApplicant ? subject : parameters[1] + ":" + subject, bodyTemplate, parameters, to);
            }
        }

        protected string GetRootURL(string url)
        {
            return url.EndsWith("/") ? url : url + "/";
        }

        //Validate whether the item can be viewed by current user
        protected bool SecurityValidateForView()
        {
            bool isValid = false;            
            var currUser = SPContext.Current.Web.CurrentUser.LoginName;
            var applicantUser = WorkflowContext.Current.DataFields["Applicant"].AsString();

            var item = SPContext.Current.ListItem;
            SPFieldUser fieldUser = item.Fields["Created By"] as SPFieldUser;
            SPFieldUserValue fieldValue = fieldUser.GetFieldValue(item["Created By"].AsString()) as SPFieldUserValue;
            var createdUser = fieldValue.User.LoginName;

            var approverUsers = WorkflowContext.Current.DataFields["Approvers"].AsString();

            if (applicantUser.Contains(currUser) || createdUser.Contains(currUser) || approverUsers.Contains(currUser))
            {
                isValid = true;
            }
            else
            {
                
                var siteId = SPContext.Current.Site.ID;
                var listId = SPContext.Current.List.ID;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                        {
                            try
                            {
                                SPList list = web.Lists["MailMember"];
                                foreach (SPListItem tmpItem in list.Items)
                                {
                                    if (tmpItem["Account"].AsString().Equals(currUser, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        isValid = true;
                                        break;
                                    }
                                }

                                if (!isValid)
                                {
                                    list = web.Lists[listId];
                                    SPRoleAssignment roleAssignment = list.RoleAssignments.GetAssignmentByPrincipal(web.Users[currUser]);
                                    SPRoleDefinitionBindingCollection roleDefs = roleAssignment.RoleDefinitionBindings;
                                    foreach (SPRoleDefinition roleDef in roleDefs)
                                    {
                                        if (web.RoleDefinitions.GetByType(SPRoleType.Administrator).Equals(roleDef))
                                        {
                                            isValid = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Create the source, if it does not already exist.
                                if (!EventLog.SourceExists("C&A"))
                                {
                                    EventLog.CreateEventSource("C&A", "Mail");
                                }

                                // Create an EventLog instance and assign its source.
                                EventLog myLog = new EventLog();
                                myLog.Source = "C&A";

                                // Write an informational entry to the event log.
                                var err = "SecurityValidateForView:" + ex.Message + ",currUser:" + currUser;
                                myLog.WriteEntry(err, EventLogEntryType.Error);
                            }
                        }
                    }
                });
            }
            
            return isValid;
        }


        #region 获得原始审批人和其创建的代理人
        /// <summary>
        /// 获得原始审批人和其创建的代理人
        /// </summary>
        /// <param name="approverNameCollection">原始审批人</param>
        /// <param name="modulesId">所属List模块ID</param>
        /// <returns></returns>
        public QuickFlow.NameCollection GetDelemanNameCollection(QuickFlow.NameCollection approverNameCollection, string modulesId)
        {
            QuickFlow.NameCollection delemanNameCollection = new QuickFlow.NameCollection();
            foreach (string approver in approverNameCollection)
            {
                if (approver.IsNotNullOrWhitespace())
                {
                    delemanNameCollection.Add(approver);
                    //一个模块只有一个代理
                    var deleman = WorkFlowUtil.GetDeleman(approver, modulesId);
                    if (null != deleman)
                    {
                        delemanNameCollection.Add(deleman);
                    }
                }
            }
            return delemanNameCollection;
        }
        #endregion

        #region 添加工作流审批任务操作人及将除代理人以外的原始审批人添加到任务操作人中
        public void AddWorkFlowStepApprovers(string workFlowStep, string approvers, string approversLoginName)
        {
            var rootweb = SPContext.Current.Site.RootWeb;
            var web = SPContext.Current.Web;
            var fields = WorkflowContext.Current.DataFields;
            var curUser = web.CurrentUser;
            //审批过该条单子的人集合
            var approverList = new List<string>();
            //var approverUsers = fields[approvers].AsString();
            //var userValueColl = fields[approversLoginName] as SPFieldUserValueCollection;
            var approverUsers = fields[approversLoginName].AsString();
            var userValueColl = fields[approvers] as SPFieldUserValueCollection;
            if (userValueColl == null)
            {
                userValueColl = new SPFieldUserValueCollection();
            }
            if (!string.IsNullOrEmpty(approverUsers))
            {
                approverList = approverUsers.Split(';').ToList<string>();
            }
            List<string> workFlowStepsAndUsers = fields["WorkFlowStepsAndUsers"].AsString().Split(';').ToList<string>();
            foreach (string str in workFlowStepsAndUsers)
            {
                if (str.IndexOf(workFlowStep) != -1)
                {
                    string workFlowStepsAndUsersNameCollection = str.Replace(workFlowStep + ":", "").Trim();
                    foreach (var currentStepApprover in workFlowStepsAndUsersNameCollection.Split(','))
                    {
                        if (!string.IsNullOrEmpty(currentStepApprover) && !approverList.Contains(currentStepApprover))
                        {
                            try
                            {
                                var spUser = web.Users[currentStepApprover];
                                approverList.Add(currentStepApprover);
                                userValueColl.Add(new SPFieldUserValue(rootweb, spUser.ID, spUser.Name));
                            }
                            catch (Microsoft.SharePoint.SPException)
                            {
                                //错误信息日志记录
                            }
                        }
                    }
                }
            }
            //加上当前审批者（当前审批者是某个审批人的代理的场合）
            if (!approverList.Contains(curUser.LoginName))
            {
                try
                {
                    approverList.Add(curUser.LoginName);
                    userValueColl.Add(new SPFieldUserValue(rootweb, curUser.ID, curUser.Name));
                }
                catch (Microsoft.SharePoint.SPException)
                {
                    //错误信息日志记录
                }
            }
            fields[approvers] = userValueColl;
            fields[approversLoginName] = string.Join(";", approverList.ToArray());
        }
        #endregion
        
    }
}
