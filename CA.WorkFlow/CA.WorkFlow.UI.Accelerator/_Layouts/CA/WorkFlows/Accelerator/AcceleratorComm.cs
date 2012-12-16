using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuickFlow;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Data;

namespace CA.WorkFlow.UI.Accelerator
{
    public class AcceleratorComm
    {


        /// <summary>
        /// 得到可用的Accelerator Type
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAcceleratorType()
        {
            SPQuery spCamel = new SPQuery();
            spCamel.Query = @"
                            <Where>
                                <Eq>
                                    <FieldRef Name='IsActive' />
                                    <Value Type='Boolean'>1</Value>
                                </Eq>
                            </Where>";
            DataTable dt = SPContext.Current.Web.Lists["AcceleratorType"].GetItems(spCamel).GetDataTable();
            return dt;
        }

        //得到组里的用户
        public static NameCollection GetTaskUsers(string group)
        {
            NameCollection taskUsers = new NameCollection();
            List<string> groupUsers = null;
            groupUsers = WorkFlowUtil.UserListInGroup(group);
            taskUsers.AddRange(groupUsers.ToArray());
            return taskUsers;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="sApplicant"></param>
        /// <param name="sStatus"></param>
        /// <param name="sOSPNO"></param>
        /// <param name="sApproverName"></param>
        public void SendMail(string sApprovers, string sStatus, string sNO, string sCurrentUserName)
        {
            List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApprovers);
            List<string> listPars = new List<string>();//设置发送mail主体内容参数
            listPars.Add("RecieverName");
            listPars.Add(sNO);
            listPars.Add(sStatus);
            listPars.Add(sCurrentUserName);

            string title = "Accelerator";
            SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
            if (listMailTemplate == null)
            {
                CommonUtil.logError("Send Accelerator notice mail failed,Because mail template is null");
                return;
            }
            string bodyTemplate = listMailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string sTitle = listMailTemplate["Subject"].AsString();
                WorkFlowUtil.SendMail(sTitle, bodyTemplate, listPars, employeReceiver);
            }
            else
            {
                CommonUtil.logError("Send Accelerator notice mail failed,Because mail template Body is null");
            }
        }


        /// <summary>
        /// 给CMO组用户发送邮件
        /// </summary>
        /// <param name="sApprovers"></param>
        /// <param name="sApplicant"></param>
        /// <param name="sID"></param>
        /// <param name="sCurrentUserName"></param>
        public void SendMMCMail(string sApprovers,string sApplicant, string sID, string sCurrentUserName)
        {
            List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApprovers);
            List<string> listPars = new List<string>();//设置发送mail主体内容参数
            listPars.Add("RecieverName");
            listPars.Add(sApplicant);
            listPars.Add(sCurrentUserName);
            listPars.Add(sID);

            string title = "AcceleratorCMO";
            SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
            if (listMailTemplate == null)
            {
                CommonUtil.logError("Send Accelerator notice mail failed,Because mail template is null");
                return;
            }
            string bodyTemplate = listMailTemplate["Body"].AsString();
            if (bodyTemplate.IsNotNullOrWhitespace())
            {
                string sTitle = listMailTemplate["Subject"].AsString();
                WorkFlowUtil.SendMail(sTitle, bodyTemplate, listPars, employeReceiver);
            }
            else
            {
                CommonUtil.logError("Send Accelerator notice mail failed,Because mail template Body is null");
            }
        }

        /// <summary>
        /// 查找提交人的DMM级用户等级为L-4或L-5的
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public static Employee GetDMMApprover(Employee emp)
        {
            Employee approver = null;
            approver = string.IsNullOrEmpty(emp.ManagerID) ? null : UserProfileUtil.GetEmployeeByProp("EmployeeId", emp.ManagerID);

            if (approver == null)
            {
                return null;
            }
            var jobLevel = approver != null ? approver.JobLevel : "L-0";

            var start = jobLevel.IndexOf("-") + 1; //L-3
            var length = jobLevel.Length - start;
            var level = Convert.ToInt32(jobLevel.Substring(start, length));
            while (level >5)
            {
                approver = UserProfileUtil.GetEmployeeByProp("EmployeeId", approver.ManagerID);
                jobLevel = approver != null ? approver.JobLevel : "L-0";
                length = jobLevel.Length - start;
                level = Convert.ToInt32(jobLevel.Substring(start, length));
            }

            if (level < 4)
            {
                return null;
            }
            return approver;
        }


    }
}