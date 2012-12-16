using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using QuickFlow.Core;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.PADChangeRequest
{
    public partial class DisplayForm : System.Web.UI.Page
    {
        WorkflowDataFields fields = WorkflowContext.Current.DataFields;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// /绑定数据到Repeater
        /// </summary>
        void BindData()
        {
            DataTable dt = new DataTable();
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            string sPONO = fields["Title"].ToString();
         
            SPQuery query = new SPQuery();
            query.Query = string.Format(@"<Where>
                                <Eq>
                                    <FieldRef Name='Title' />
                                    <Value Type='Text'>{0}</Value>
                                </Eq>
                            </Where>", sPONO);
            dt = SPContext.Current.Web.Lists["PADChangeRequestItems"].GetItems(query).GetDataTable();
            if (null != dt && dt.Rows.Count > 0)
            {
                RepeaterPOData.DataSource = dt;
                RepeaterPOData.DataBind();

                PanelNew.Visible = true;
                PanelOldWorkFlow.Visible = false;
                LabelCount.Text = dt.Rows.Count.ToString();
            }
            else
            {
                PanelNew.Visible = false;
                PanelOldWorkFlow.Visible = true;
            }
        }

        /// <summary>
        /// 设置显示的日期格式
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        public string DateFormate(string sDate)
        {
            DateTime dtDate = DateTime.Now;
            if (DateTime.TryParse(sDate, out dtDate))
            {
                return dtDate.ToString("MM\\/dd\\/yyyy");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置每个PO的Approve状态
        /// </summary>
        /// <param name="sIsNeedApprove"></param>
        /// <returns></returns>
       public  string SetItemApproveState(string sIsNeedApprove)
        {
            string sStatus= fields["Status"].ToString();
            if (sStatus.Equals(CAWorkflowStatus.InProgress, StringComparison.InvariantCultureIgnoreCase))
            {
                return CAWorkflowStatus.InProgress;
            }
            if (sIsNeedApprove != "1")
            {
                return "Rejected";
            }
            else
            {
                return "Approved";
            }
        }

       /// <summary>
       /// 设置PO的审批状态
       /// </summary>
       /// <param name="PostStatus"></param>
       /// <returns></returns>
       public string SetPostStatus(string PostStatus)
       {
           string sStatus = fields["Status"].ToString();
           if (PostStatus != "1")
           {
               return "NO";
           }
           else
           {
               return "YES";
           }
       }
    }
}