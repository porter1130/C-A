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

    public partial class VendorInfo : CAWorkFlowPage
    {
        StringBuilder mOutputStr = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["vid"] != null)
            {
                string vid = Request.QueryString["vid"].ToString();
                DataTable vTable = PaymentRequestComm.GetVendorInfoByVendorID(vid);
                if (vTable != null)
                {
                    Vendor vendor = new Vendor();
                    vendor.VendorID = vid;
                    vendor.VendorName = vTable.Rows[0]["Title"].AsString();
                    vendor.BankName = vTable.Rows[0]["BankName"].AsString();
                    vendor.BankAccount = vTable.Rows[0]["BankAccount"].AsString();
                    vendor.SwiftCode = vTable.Rows[0]["SwiftCode"].AsString();
                    vendor.BankCity = vTable.Rows[0]["BankCity"].AsString();
                    vendor.VendorCity = vTable.Rows[0]["City"].AsString();
                    vendor.VendorCountry = vTable.Rows[0]["Country"].AsString();
                    mOutputStr.Append(new JavaScriptSerializer().Serialize(vendor));
                }
                else 
                {
                    Vendor vendor = new Vendor();
                    vendor.Result = "0";
                    mOutputStr.Append(new JavaScriptSerializer().Serialize(vendor));
                }
            }
            if (Request.QueryString["vendorCode"] != null && Request.QueryString["vendorName"] != null)
            {
                string vendorCode = Request.QueryString["vendorCode"].AsString();
                string vendorName = Request.QueryString["vendorName"].AsString();
                DataTable dt = WorkFlowUtil.GetCollectionByList("Vendors").GetDataTable();
                EnumerableRowCollection<DataRow> row = null;
                DataTable result = dt.Clone();
                if (vendorCode != "" && vendorName != "")
                {
                    row = dt.AsEnumerable()
                           .Where(dr => dr.Field<string>("Title").AsString().ToLower().Contains(vendorName.ToLower())
                                     && dr.Field<string>("VendorId").AsString() == vendorCode)
                           .OrderByDescending(dr => dr.Field<string>("VendorId"));
                }
                if (vendorCode != "" && vendorName == "")
                {
                    row = dt.AsEnumerable()
                            .Where(dr => dr.Field<string>("VendorId").AsString() == vendorCode)
                            .OrderByDescending(dr => dr.Field<string>("VendorId")); 
                }
                if (vendorCode == "" && vendorName != "")
                {
                    row = dt.AsEnumerable()
                            .Where(dr => dr.Field<string>("Title").AsString().ToLower().Contains(vendorName.ToLower()))
                            .OrderByDescending(dr => dr.Field<string>("VendorId")); 
                }
                if (vendorCode == "" && vendorName == "") 
                {
                    row = dt.AsEnumerable()
                            .OrderByDescending(dr => dr.Field<string>("VendorId")); 
                }
                foreach (DataRow dr in row)
                {
                    result.ImportRow(dr);
                }
                if (result.Rows.Count > 0)
                {
                    List<Vendor> list = new List<Vendor>();
                    foreach (DataRow dr in result.Rows)
                    {
                        Vendor vendor = new Vendor();
                        vendor.VendorID = dr["VendorId"].AsString();
                        vendor.VendorName = dr["Title"].AsString();
                        vendor.BankName = dr["BankName"].AsString();
                        vendor.BankAccount = dr["BankAccount"].AsString();
                        vendor.SwiftCode = dr["SwiftCode"].AsString();
                        vendor.BankCity = dr["BankCity"].AsString();
                        vendor.VendorCity = dr["City"].AsString();
                        vendor.VendorCountry = dr["Country"].AsString();
                        list.Add(vendor);
                    }
                    mOutputStr.Append(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list));
                }
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Response.Write(mOutputStr.ToString());
        }
    }

    class Vendor
    {
        string mVendorCity = string.Empty;
        public string VendorCity
        {
            get
            {
                return mVendorCity;
            }
            set
            {
                mVendorCity = value;
            }
        }

        string mVendorCountry = string.Empty;
        public string VendorCountry
        {
            get
            {
                return mVendorCountry;
            }
            set
            {
                mVendorCountry = value;
            }
        }

        string mBankCity = string.Empty;
        public string BankCity
        {
            get
            {
                return mBankCity;
            }
            set
            {
                mBankCity = value;
            }
        }


        string mVendorID = string.Empty;
        public string VendorID
        {
            get
            {
                return mVendorID;
            }
            set
            {
                mVendorID = value;
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
        string mBankName = string.Empty;
        public string BankName
        {
            get
            {
                return mBankName;
            }
            set
            {
                mBankName = value;
            }
        }
        string mBankAccount = string.Empty;
        public string BankAccount
        {
            get
            {
                return mBankAccount;
            }
            set
            {
                mBankAccount = value;
            }
        }
        string mSwiftCode = string.Empty;
        public string SwiftCode
        {
            get
            {
                return mSwiftCode;
            }
            set
            {
                mSwiftCode = value;
            }
        }
        string mResult = "1";
        public string Result
        {
            get
            {
                return mResult;
            }
            set
            {
                mResult = value;
            }
        }
    }
}