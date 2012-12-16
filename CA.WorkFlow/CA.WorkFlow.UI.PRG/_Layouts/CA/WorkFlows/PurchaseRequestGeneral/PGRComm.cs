using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuickFlow;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.PurchaseRequestGeneral
{
    public class PGRComm
    {
        //得到组里的用户
        public static NameCollection GetTaskUsers(string group)
        {
            NameCollection taskUsers = new NameCollection();
            List<string> groupUsers = null;
            groupUsers = WorkFlowUtil.UserListInGroup(group);
            taskUsers.AddRange(groupUsers.ToArray());
            return taskUsers;
        }



        public void SendMail(string sApplicant, string sStatus, string sOSPNO, string sApproverName)
        {
            List<Employee> employeReceiver = WorkFlowUtil.GetEmployees(sApplicant);
            List<string> listPars = new List<string>();//设置发送mail主体内容参数
            listPars.Add(employeReceiver[0].DisplayName);
            listPars.Add(sOSPNO);
            listPars.Add(sStatus);
            listPars.Add(UserProfileUtil.GetEmployeeEx(sApproverName).DisplayName);

            string title = "PurchaseRequestGeneral";
            SPListItem listMailTemplate = WorkFlowUtil.GetEmailTemplateByTitle(title);
            if (listMailTemplate == null)
            {
                CommonUtil.logError("Send PRG notice mail failed,Because mail template is null");
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
                CommonUtil.logError("Send PRG notice mail failed,Because mail template Body is null");
            }
        }

    }
}