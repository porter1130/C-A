using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using System.Text;

namespace CA.WorkFlow.UI.PurchaseRequest
{
    public partial class DisableItemCode : Microsoft.SharePoint.WebControls.LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAccount();
            if (IsPostBack)
            {
                return;
            }
            string sItemcodes= GetItemCodes();
            TextBoxItemCodes.Text = sItemcodes;
        }

        protected void ButtonDisable_Click(object sender, EventArgs e)
        {
            string sItemcodes = TextBoxItemCodes.Text.Trim();
            string sErrorInfo= UnavalibleItem(sItemcodes);
            if (null != sErrorInfo && sErrorInfo.Length > 0)
            {
                sErrorInfo = sErrorInfo.Insert(0, "Can not find:\\n");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alertt find", string.Format("<script>alert('{0}');</script>", sErrorInfo));
            }
            else
            {
                bool bUpdateItem=BatchUpdate(sItemcodes, false);
                UpdateItemCodesAction("Disable");
                if (bUpdateItem)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alertt find", "<script>alert('Disable success');</script>");
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            string sItemcodes = TextBoxItemCodes.Text.Trim();
            if (sItemcodes.Length == 0)
            {
                return;
            }
            string sErrorInfo = UnavalibleItem(sItemcodes);
            if (null != sErrorInfo && sErrorInfo.Length > 0)
            {
                sErrorInfo = sErrorInfo.Insert(0, "Can not find:\\n");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", string.Format("<script>alert('{0}');</script>", sErrorInfo));
            }
            else//用户输入空值
            {
                bool bUpdateItem= BatchUpdate(sItemcodes,true);
                UpdateItemCodesAction("Enable");
                if (bUpdateItem)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alertt find", "<script>alert('Enable success');</script>");
                }
            }
        }

        /// <summary>
        /// 得到不可用的ItemCode
        /// </summary>
        /// <param name="sItemcodes"></param>
        /// <returns></returns>
        string UnavalibleItem(string sItemcodes)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(sItemcodes))
            {
                return sb.ToString();
            }
            foreach (string str in sItemcodes.Split(','))
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                if (!CheckItecode(str))
                {
                    sb.Append(str+" \\n");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 验证Itemcode是否合法
        /// </summary>
        /// <param name="sItemCode"></param>
        /// <returns></returns>
        bool CheckItecode(string sItemCode)
        {
            bool isOK=false;
            SPSecurity.RunWithElevatedPrivileges(delegate {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                { 
                    using(SPWeb web=site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        string sQuery = @"<Where>
                                                <Eq>
                                                    <FieldRef Name='Title' />
                                                    <Value Type='Text'>{0}</Value>
                                                </Eq>
                                           </Where>";
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = string.Format(sQuery, sItemCode);
                        SPListItemCollection splic = web.Lists["Item Codes"].GetItems(spQuery);
                        if (null == splic|| splic.Count == 0)
                        {
                            isOK = false;
                        }
                        else
                        {
                            isOK = true;
                        }
                    }
                }
            });
            return isOK;
        }

        /// <summary>
        /// 批量更新Itemcode
        /// </summary>
        /// <param name="sItemCodes"></param>
        /// <param name="isActive"></param>
        bool BatchUpdate(string sItemCodes, bool isActive)
        {
            if (string.IsNullOrEmpty(sItemCodes))
            {
                return false;
            }
            string sQueryCamle = GetQueryCamle(sItemCodes);
            if (string.IsNullOrEmpty(sQueryCamle))
            {
                return false;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                { 
                    using(SPWeb web=site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPQuery query = new SPQuery();
                        query.Query = sQueryCamle;
                        SPListItemCollection splic= web.Lists["Item Codes"].GetItems(query);
                        foreach (SPListItem item in splic)
                        {
                            item["IsActive"] = isActive;
                            item.Update();
                        }
                    }
                }
            });
            return true;

        }

        /// <summary>
        /// 得到Camle查询语句
        /// </summary>
        /// <param name="sItemCodes"></param>
        /// <returns></returns>
        string GetQueryCamle(string sItemCodes)
        {
            string[] strArray = sItemCodes.Split(',');
            string sCamle = string.Empty;
            for (int i = 0; i < strArray.Length; i++)
            {
                string sItem = strArray[i];
                if (string.IsNullOrEmpty(sItem))
                {
                    continue;
                }
                string sOrCondition = string.Format("<Eq><FieldRef Name='Title' /><Value Type='Text'>{0}</Value></Eq>", sItem);
                sCamle += sOrCondition;
                if (i == 1 || sCamle.ToString().IndexOf("<Or>") == 0)
                {
                    sCamle = string.Format("<Or>{0}</Or>", sCamle);
                }
            }
            if (string.IsNullOrEmpty(sCamle))
            {
                return string.Empty;
            }
            sCamle = string.Format("<Where>{0}</Where>", sCamle);
            return sCamle;
        }

        /// <summary>
        /// 添加禁用的Itemcdoe记录
        /// </summary>
        void AddItems()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate { 
                using(SPSite site=new SPSite(SPContext.Current.Site.ID))
                {
                    using(SPWeb web=site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists["DisableItemCode"];
                        SPListItem newItem= list.Items.Add();
                        newItem["Title"] = SPContext.Current.Web.CurrentUser.LoginName;
                        newItem["ItemCodes"] = TextBoxItemCodes.Text.Trim();
                        newItem.Update();
                    }
                }
            });
        }

        /// <summary>
        /// 得到用户操作的ItemCodes
        /// </summary>
        /// <returns></returns>
        string GetItemCodes()
        {
            string sItemCodes = string.Empty;
            SPListItemCollection splic = SPContext.Current.Web.Lists["DisableItemCode"].Items;
            if (null != splic && splic.Count > 0)
            { 
                if(null!=splic[0]["ItemCodes"])
                {
                    sItemCodes = splic[0]["ItemCodes"].ToString();
                }
            }
            return sItemCodes;
        }

        /// <summary>
        ///  添加或修改用户操作Itecodes的记录
        /// </summary>
        /// <param name="sAction"></param>
        void UpdateItemCodesAction(string sAction)
        {
            string sItemcodes=TextBoxItemCodes.Text.Trim();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists["DisableItemCode"];
                        SPListItemCollection splic = list.Items;
                        if (null != splic && splic.Count > 0)//更新
                        {
                            foreach (SPListItem item in splic)
                            {
                                item["Title"] = sAction;
                                item["ItemCodes"] = sItemcodes;
                                item.Update();
                            }
                        }
                        else//新加记录
                        {
                            SPListItem newItem = list.Items.Add();
                            newItem["Title"] = sAction;
                            newItem["ItemCodes"] = TextBoxItemCodes.Text.Trim();
                            newItem.Update();
                        }
                    }
                }
            });
        }

        void CheckAccount()
        {
            // wf_HO可以打开页面
            var current = SPContext.Current.Web.CurrentUser.LoginName;
            if (!PurchaseRequestCommon.IsInGroups(current, new string[] { "wf_HO" }))
            {
                this.Response.Redirect("/WorkFlowCenter/Lists/Tasks/MyItems.aspx");
            }
        }
    }
}