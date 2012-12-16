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

namespace CA.WorkFlow.UI.PurchaseRequestGeneral
{
    public partial class DataEdit : BaseWorkflowUserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;  
            }
            BindCurrency();
            SetValue();
        }

        /// <summary>
        /// 绑定币种
        /// </summary>
        void BindCurrency()
        {
            DropDownListCurrency.DataSource = GetCurrencyData();
            DropDownListCurrency.DataValueField = "Rate";
            DropDownListCurrency.DataTextField = "Title";
            DropDownListCurrency.DataBind();
            DropDownListCurrency.Items.Insert(0,new ListItem("RMB","1"));
        }

        /// <summary>
        /// 设置控件选定值
        /// </summary>
        void SetValue()
        {

            WorkflowDataFields fields = WorkflowContext.Current.DataFields;
            if (null != fields["Currency"])
            {
                ListItem li = DropDownListCurrency.Items.FindByText(fields["Currency"].ToString());
                if (null != li)
                {
                    li.Selected = true;
                }
            }
            if (null != fields["IsAnnualBudget"])
            {
                RadioListIsAnnualBudget.SelectedValue = fields["IsAnnualBudget"].ToString() == "True" ? "1" : "0"; ;
            }
            if (null != fields["IsNeedBid"])
            {
                RadioListIsNeedBid.SelectedValue = fields["IsNeedBid"].ToString() == "True" ? "1" : "0"; ;
            }

            if (null != fields["IncurredFrom"])
            {
                DateTime dtIncurredFrom=DateTime.Now;
                if (DateTime.TryParse(fields["IncurredFrom"].ToString(), out dtIncurredFrom))
                {
                    CADateTimeincurredFrom.SelectedDate = dtIncurredFrom;
                }
            }

            if (null != fields["IncurredTo"])
            {
                DateTime dtIncurredTo = DateTime.Now;
                if (DateTime.TryParse(fields["IncurredTo"].ToString(), out dtIncurredTo))
                {
                    CADateTimeincurredTo.SelectedDate = dtIncurredTo;
                }
            }

            if (null != fields["IsIncurred"])
            {
                RadioButtonListIncurred.SelectedValue = fields["IsIncurred"].ToString()=="True"?"1":"0";
            }

            if (null != fields["PeriodFrom"])
            {
                DateTime dtPeriodFrom = DateTime.Now;
                if (DateTime.TryParse(fields["PeriodFrom"].ToString(), out dtPeriodFrom))
                {
                    CADateTimeFrom.SelectedDate = dtPeriodFrom;
                }
            }

            if (null != fields["PeriodTo"])
            {
                DateTime dtPeriodTo = DateTime.Now;
                if (DateTime.TryParse(fields["PeriodTo"].ToString(), out dtPeriodTo))
                {
                    CADateTimeTo.SelectedDate = dtPeriodTo;
                }
            }
        }

        /// <summary>
        /// 得到汇率信息。
        /// </summary>
        /// <returns></returns>
        DataTable GetCurrencyData()
        {
            DataTable dt = new DataTable();
            dt = SPContext.Current.Web.Lists["ExchangeRates"].Items.GetDataTable();
            return dt;
        }

        /// <summary>
        /// 得到币种
        /// </summary>
        /// <returns></returns>
        public string GetCurrency()
        {
            return DropDownListCurrency.SelectedItem.Text;
        }

        /// <summary>
        /// 得到汇率
        /// </summary>
        /// <returns></returns>
        public string GetRate()
        {
            return DropDownListCurrency.SelectedValue;
        }

        /// <summary>
        /// 是否是全年预算
        /// </summary>
        /// <returns></returns>
        public bool IsAnnualBudget()
        {
            return RadioListIsAnnualBudget.SelectedIndex == 0 ? true : false;
        }

        /// <summary>
        /// 是否要招标会
        /// </summary>
        /// <returns></returns>
        public bool IsNeedBid()
        {
            return RadioListIsNeedBid.SelectedIndex == 0 ? true : false;
        }

        /// <summary>
        /// 是否己经发生过
        /// </summary>
        /// <returns></returns>
        public bool IsIncurred()
        {
            return RadioButtonListIncurred.SelectedIndex == 0 ? true : false; 
        }


        /// <summary>
        /// 发生过的From
        /// </summary>
        /// <returns></returns>
        public DateTime IncurredFrom()
        {
            return CADateTimeincurredFrom.SelectedDate;
        }


        /// <summary>
        /// 发生过的To
        /// </summary>
        /// <returns></returns>
        public DateTime IncurredTo()
        {
            return CADateTimeincurredTo.SelectedDate;
        }

        /// <summary>
        /// Period from
        /// </summary>
        /// <returns></returns>
        public DateTime PeriodFrom()
        {
            return CADateTimeFrom.SelectedDate;
        }

        /// <summary>
        /// Period to
        /// </summary>
        /// <returns></returns>
        public DateTime PeriodTo()
        {
            return CADateTimeTo.SelectedDate;
        }

        public bool CheckDate()
        {
            bool IsOK = true;
            StringBuilder sb = new StringBuilder();
            if (FormFieldCostCenter.Value.ToString().Trim().Length == 0)
            {
                IsOK = false;
                sb.Append("cost center can not be empty.\\n");
            }
            if (ApplicantFieldContent.Value.ToString().Trim().Length == 0)
            {
                IsOK = false;
                sb.Append("Goods/Services to be purchased can not be empty.\\n");
            }
            if (FormFieldReasons.Value.ToString().Trim().Length == 0)
            {
                IsOK = false;
                sb.Append("Bid reason case can not be empty.\\n");
            }

            if (CADateTimeFrom.IsDateEmpty)
            {
                IsOK = false;
                sb.Append("Period begin date can not be empty.\\n");
            }

            if (CADateTimeTo.IsDateEmpty)
            {
                IsOK = false;
                sb.Append("Period end date can not be empty.\\n");
            }

            if (!CADateTimeFrom.IsDateEmpty&&CADateTimeFrom.SelectedDate > DateTime.Now)
            {
                IsOK = false;
                sb.Append("Period begin date should be later than current date .\\n");  
            }

            if (!CADateTimeFrom.IsDateEmpty && !CADateTimeTo.IsDateEmpty)
            {
                if (CADateTimeFrom.SelectedDate > CADateTimeTo.SelectedDate)
                {
                    IsOK = false;
                    sb.Append("Period begin date should be later than end date .\\n");
                }   
            }

            if (FormFieldCost.Value == null)
            {
                IsOK = false;
                sb.Append("Total cost can not be empty.\\n");
            }
            if (FormFieldBudgetAmount.Value == null)
            {
                IsOK = false;
                sb.Append("Budget amount can not be empty.\\n");
            }
            if (FormFieldUsedAmount.Value == null)
            {
                IsOK = false;
                sb.Append("Used amount reason can not be empty.\\n");
            }

            if (RadioListIsAnnualBudget.SelectedValue == "0" && FormFieldAnnualBudgetComm.Value.ToString().Trim().Length == 0)
            {
                IsOK = false;
                sb.Append("Annual budget can not be empty.\\n");
            }

            if (RadioListIsNeedBid.SelectedValue == "0" && FormFieldBidComm.Value.ToString().Trim().Length == 0)
            {
                IsOK = false;
                sb.Append("Bid budget reason can not be empty.\\n");
            }

            if (RadioButtonListIncurred.SelectedValue == "1")
            {
                if (CADateTimeincurredFrom.IsDateEmpty)
                {
                    IsOK = false;
                    sb.Append("Latest Purchase Lasting Period begin date can not be empty.\\n");
                }
                if (CADateTimeincurredTo.IsDateEmpty)
                {
                    IsOK = false;
                    sb.Append("Latest Purchase Lasting Period end date can not be empty.\\n");
                }

                if (!CADateTimeincurredFrom.IsDateEmpty && CADateTimeincurredFrom.SelectedDate > DateTime.Now)
                {
                    IsOK = false;
                    sb.Append("Period begin date should be later than current date .\\n");
                }

                if (!CADateTimeincurredFrom.IsDateEmpty && !CADateTimeincurredTo.IsDateEmpty)
                {
                    if (CADateTimeincurredFrom.SelectedDate > CADateTimeincurredTo.SelectedDate)
                    {
                        IsOK = false;
                        sb.Append("Latest Purchase Lasting begin date should be later than end date .\\n");
                    }  
                }

                if (FormFieldLatestAmount.Value == null)
                {
                    IsOK = false;
                    sb.Append("Latest Purchase Lasting Period amount can not be empty.\\n");
                }
            }
            if (!IsOK)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script>alert('" + sb.ToString() + "')</script>");
            }

            return IsOK;
        }

        public void ShowWorkFlowNo()
        {
            Title.Visible = true;
        }
    }
}