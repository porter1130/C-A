namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using CA.SharePoint.Utilities.Common;
    using CA.SharePoint;
    using Microsoft.SharePoint;
    using System.Linq;
    using System.Globalization;
    using System.Text;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using System.ComponentModel;
    using QuickFlow.Core;
    using QuickFlow;
    using CodeArt.SharePoint.CamlQuery;
    using System.Configuration;
    using System.Collections;
    
    public partial class PRTS : PaymentRequestControl 
    {
        protected void Page_Load(object sender, EventArgs e) 
        {
            if(!Page.IsPostBack)
            {
                Employee employee = UserProfileUtil.GetEmployeeEx(SPContext.Current.Web.CurrentUser.LoginName);
                this.hfApplicant.Value = employee.DisplayName + "(" + employee.UserAccount+")";
            }
        }
    }

}
