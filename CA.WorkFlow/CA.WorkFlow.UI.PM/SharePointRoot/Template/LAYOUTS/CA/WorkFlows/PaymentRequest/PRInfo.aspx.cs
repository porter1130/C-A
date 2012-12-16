namespace CA.WorkFlow.UI.PaymentRequest
{
    using System;
    using QuickFlow.Core;
    using SharePoint.Utilities.Common;
    using Microsoft.SharePoint;
    using Microsoft.Office.Core;
    using System.Web.UI.WebControls;
    using System.Data;
    using System.Web.Script.Serialization;
    using System.Text;
    using System.Collections.Generic;

    public partial class PRInfo : CAWorkFlowPage
    {
        StringBuilder mOutputStr = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Applicant"] != null)
            {
                //object vendorCode = Request.QueryString["VendorCode"];
                //object vendorName = Request.QueryString["VendorName"];
                string applicant = Request.QueryString["Applicant"].ToString();
                object workFlowNumber = Request.QueryString["WorkFlowNumber"];
                object contractNumber = Request.QueryString["ContractNumber"];
                object paymentRequestType = Request.QueryString["PaymentRequestType"];
                applicant = applicant.ToLower();

                DataTable prInfo = PaymentRequestComm.GetPRInfoByapplicant(paymentRequestType.ToString());
                if (null == prInfo)
                {
                    mOutputStr.Append("No Any Records!");
                }
                else
                {
                    if (prInfo.Rows.Count > 0)
                    {
                        List<PRInfoData> prList = new List<PRInfoData>();
                        EnumerableRowCollection<DataRow> data = prInfo.AsEnumerable();
                        if (workFlowNumber.AsString() != "" && contractNumber.AsString() == "")
                        {
                            data = from dr in prInfo.AsEnumerable()
                                   where dr.Field<string>("SubPRNo").AsString().Contains(workFlowNumber.AsString())
                                       && dr.Field<string>("SubPRNo").AsString().Contains("_1")
                                       && (dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant)
                                       || applicant.ToString().Contains(dr.Field<string>("Author").AsString().ToLower()) )
                                   select dr;
                        }
                        if (workFlowNumber.AsString() == "" && contractNumber.AsString() != "")
                        {
                            data = from dr in prInfo.AsEnumerable()
                                   where dr.Field<string>("ContractPONo").AsString().Contains(contractNumber.AsString())
                                       && dr.Field<string>("SubPRNo").AsString().Contains("_1")
                                       && (dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant)
                                       || applicant.ToString().Contains(dr.Field<string>("Author").AsString().ToLower()))
                                   select dr;
                        }
                        if (workFlowNumber.AsString() != "" && contractNumber.AsString() != "")
                        {
                            data = from dr in prInfo.AsEnumerable()
                                   where dr.Field<string>("SubPRNo").AsString().Contains(workFlowNumber.AsString())
                                       && dr.Field<string>("SubPRNo").AsString().Contains("_1")
                                       && dr.Field<string>("ContractPONo").AsString().Contains(contractNumber.AsString())
                                       && (dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant)
                                       || applicant.ToString().Contains(dr.Field<string>("Author").AsString().ToLower()))
                                   select dr;
                        }
                        if (workFlowNumber.AsString() == "" && contractNumber.AsString() == "")
                        {
                            data = from dr in prInfo.AsEnumerable()
                                   where  dr.Field<string>("SubPRNo").AsString().Contains("_1")
                                       && (dr.Field<string>("Applicant").AsString().ToLower().Contains(applicant)
                                       || applicant.ToString().Contains(dr.Field<string>("Author").AsString().ToLower()))
                                   select dr;
                        }
                        foreach (DataRow dr in data)
                        {
                            PRInfoData pr = new PRInfoData();
                            pr.WorkFlowNumber = dr["SubPRNo"].AsString();
                            pr.VendorName = dr["VendorName"].AsString();
                            pr.ContractNumber = dr["ContractPONo"].AsString();
                            pr.ID = dr["ID"].AsString();
                            pr.RequestType = dr["RequestType"].AsString() == "Capex" ? "Capex_AssetNo" : dr["RequestType"].AsString();
                            prList.Add(pr);
                        }
                        mOutputStr.Append(new JavaScriptSerializer().Serialize(prList));
                    }
                }
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Response.Write(mOutputStr.ToString());
        }
    }

   class PRInfoData
    {
        string mWorkFlowNumber = string.Empty;
        public string WorkFlowNumber
        {
            get
            {
                return mWorkFlowNumber;
            }
            set
            {
                mWorkFlowNumber = value;
            }
        }

        string mVendorName = string.Empty;
        public string VendorName
        {
            get
            {
                return mVendorName;
            }
            set
            {
                mVendorName = value;
            }
        }

        string mContractNumber = string.Empty;
        public string ContractNumber
        {
            get
            {
                return mContractNumber;
            }
            set
            {
                mContractNumber = value;
            }
        }

        string mID = string.Empty;
        public string ID
        {
            get
            {
                return mID;
            }
            set
            {
                mID = value;
            }
        }
        string mRequestType = string.Empty;
        public string RequestType
        {
            get
            {
                return mRequestType;
            }
            set
            {
                mRequestType = value;
            }
        }
    }
}