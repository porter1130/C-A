using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Linq;
using System.Collections.Generic;

namespace CA.WorkFlow.UI.NTSC
{
    public partial class DataEdit : BaseWorkflowUserControl
    {
        #region

        private static string department = "Buying";

        public Employee Applicant
        {
            get
            {
                return (this.ViewState["Applicant"] as Employee);
            }
            set
            {
                this.ViewState["Applicant"] = value;
            }
        }

        private string dataFormMode;

        public string DataFormMode
        {
            get { return dataFormMode; }
            set { dataFormMode = value; }
        }

        public bool SubmitStatus
        {
            get
            {
                return (bool)this.ViewState["SubmitStatus"];
            }
            set
            {
                this.ViewState["SubmitStatus"] = value;
            }
        }

        public string SupplierName 
        {
            set 
            {
                this.txtSupplierName.Value = value;
            }
            get 
            {
                return this.txtSupplierName.Value.Trim();
            }
        }

        public string SubDivision
        {
            set
            {
                this.dpSubDivision.SelectedValue = value;
            }
            get
            {
                return this.dpSubDivision.SelectedValue;
            }
        }

        public string Reason
        {
            set
            {
                this.txtReason.Value = value;
            }
            get
            {
                return this.txtReason.Value;
            }
        }

        public string IsMondial
        {
            set
            {
                this.ddlIsMondial.SelectedValue = value;
            }
            get
            {
                return this.ddlIsMondial.SelectedValue;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (DataFormMode.Equals("New", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.cpfUser.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.ToString();
                }
                if (DataFormMode.Equals("Edit", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.cpfUser.CommaSeparatedAccounts = this.Applicant.UserAccount;
                }
                LoadBuyInfo();
            }
            this.cpfUser.Load += new EventHandler(cpfUser_Load);
        }

        private void LoadBuyInfo()
        {
            this.SubmitStatus = false;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
               SPList spList = SPContext.Current.Web.ParentWeb.Lists["Department"];
               SPQuery query = new SPQuery();
               query.Query = string.Format(@"<Where>
                                                <Or>
                                                    <Eq>
                                                       <FieldRef Name='Title' />
                                                       <Value Type='Text'>{0}</Value>
                                                    </Eq>
                                                    <Eq>
                                                       <FieldRef Name='DisplayName' />
                                                       <Value Type='Text'>{1}</Value>
                                                    </Eq>
                                                </Or>
                                            </Where>", department, department);
               SPListItemCollection listItems = spList.GetItems(query);
               if (listItems.Count > 0)
               {
                   DataTable data = listItems.GetDataTable();
                   dpSubDivision.Items.Clear();
                   foreach (DataRow row in data.Rows)
                   {
                       ListItem li = new ListItem();
                       li.Text = row["Title"].ToString();
                       li.Value = row["Title"].ToString();
                       if (row["Title"].ToString().Equals(Applicant.Department, StringComparison.CurrentCultureIgnoreCase)
                          ||row["DisplayName"].ToString().Equals(Applicant.Department, StringComparison.CurrentCultureIgnoreCase))
                       {
                           li.Selected = true;
                           this.SubmitStatus = true;
                       }
                       dpSubDivision.Items.Add(li);
                   }
               }
           });
        }

        protected void cpfUser_Load(object sender, EventArgs e)
        {
            LoadUser();
        }

        protected void btnPeopleInfo_Click(object sender, EventArgs e)
        {
            LoadUser();
            LoadBuyInfo();
        }

        private void LoadUser()
        {
            if (cpfUser.Accounts.Count == 0)
            {
                return;
            }
            this.Applicant = UserProfileUtil.GetEmployeeEx(cpfUser.Accounts[0].ToString());
            this.lbApplicant.Text = Applicant.DisplayName + "(" + Applicant.UserAccount + ")";
            this.lbDepartment.Text = Applicant.Department;
            this.lblSubDivision.Text = Applicant.Department;
        }

    }
}
