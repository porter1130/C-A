using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using Microsoft.SharePoint;
using QuickFlow.Core;
using System.Text;

namespace CA.WorkFlow.UI.Accelerator
{
    public partial class DataEdit : BaseWorkflowUserControl
    {

        public bool bIsEdt = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;  
            }
            Bind();
            SetValue();
            WorkflowNO.Visible = bIsEdt;
        }

        /// <summary>
        ///  绑定Accelerator Type
        /// </summary>
        void Bind()
        {
            DataTable dt = AcceleratorComm.GetAcceleratorType();
            DropDownListAccelerator.DataSource = dt;// GetAcceleratorType();
            DropDownListAccelerator.DataTextField = "Content";
            DropDownListAccelerator.DataValueField = "ID";
            DropDownListAccelerator.DataBind();
        }

        /// <summary>
        /// 设置Fields的值
        /// </summary>
        void SetValue()
        {
            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (null != fields["AcceleratorID"] && null != fields["AcceleratorContent"])
            {
                DropDownListAccelerator.SelectedValue = fields["AcceleratorID"].ToString();
                //RadioButtonListAccelerator.SelectedItem.Text = fields["AcceleratorContent"].ToString();
            }

            if (null != fields["FromDate"])
            {
                DateTime dtFromDate = DateTime.Now;
                if (DateTime.TryParse(fields["FromDate"].ToString(), out dtFromDate))
                {
                    CADateTimeFrom.SelectedDate = dtFromDate;
                }
            }
            if (null != fields["ToDate"])
            {
                DateTime dtToDate = DateTime.Now;
                if (DateTime.TryParse(fields["ToDate"].ToString(), out dtToDate))
                {
                    CADateTimeTo.SelectedDate = dtToDate;
                }
            }
        }

        /// <summary>
        /// 得到Accelerator内容
        /// </summary>
        /// <returns></returns>
        public string GetAcceleratorType()
        {
            return DropDownListAccelerator.SelectedItem.Text;
        }

        /// <summary>
        /// 得到Accelerator的ID
        /// </summary>
        /// <returns></returns>
        public string GetAcceleratorID()
        {
            return DropDownListAccelerator.SelectedValue;
        }

        /// <summary>
        /// 得到FromDate值
        /// </summary>
        /// <returns></returns>
        public DateTime GetFromDate()
        {
            return CADateTimeFrom.SelectedDate;
        }

       /// <summary>
        /// 得到ToDate值
       /// </summary>
       /// <returns></returns>
        public DateTime GetToDate()
        {
            return CADateTimeTo.SelectedDate;
        }

        /// <summary>
        /// 验证数据合法性
        /// </summary>
        /// <returns></returns>
        public bool CheckDate()
        {
            bool isOK = true;
            StringBuilder sb = new StringBuilder();
            if (CADateTimeFrom.IsDateEmpty)
            {
                sb.Append("FromDate can not be empty.\\n");
                isOK= false;
            }
            if (CADateTimeTo.IsDateEmpty)
            {
                sb.Append("ToDate can not be empty.\\n");
                isOK= false;
            }
            DateTime dtFrom = CADateTimeFrom.SelectedDate;
            DateTime dtTo = CADateTimeTo.SelectedDate;
            if (dtFrom > dtTo)
            {
                sb.Append("FromDate date should be later than ToDate .\\n");
                isOK= false;
            }

            TimeSpan tsfrom =dtFrom- DateTime.Parse(DateTime.Now.ToShortDateString());
            if (tsfrom.Days < 7)
            {
                sb.Append("FromDate must be greater than current date at least 7 days!.\\n");
                isOK = false;
            }
            

            if (FormFieldClass.Value == null)
            {
                sb.Append("Class can not be empty.\\n");
                isOK = false;
            }
            if (DropDownListAccelerator.SelectedIndex == -1)
            {
                sb.Append("Accelerator Type can not be empty.\\n");
                isOK = false;
            }

            if (!isOK)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('" + sb.ToString() + "')</script>");
            }
            return isOK;
        }
    }
}