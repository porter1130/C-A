using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.SharePoint;

namespace CA.WorkFlow.UI.CreditCardClaim
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string isCurrentYearValue = context.Request.Form.Get("isCurrentYear");
            bool isCurrentYear = Convert.ToBoolean(int.Parse(isCurrentYearValue));
            
            IsExistMonth(context.Request.Form.Get("fileFullPath"), isCurrentYear, context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void IsExistMonth(string fileFullPath, bool isCurrentYear, HttpContext context)
        {

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileFullPath).ToLower();

            int monthNumber;
            DateTime uploadDate = DateTime.Now;
            if (!isCurrentYear)
            {
                uploadDate = uploadDate.AddYears(-1);
            }
            string[] fileNameArray = fileNameWithoutExt.Split('_');
            if (int.TryParse(fileNameArray[fileNameArray.Length - 1], out monthNumber))
            {
                DateTime.TryParse(uploadDate.Year.ToString() + '-' + monthNumber.ToString(), out uploadDate);
            }

            bool isExistMonth = IsExistSameMonthData(uploadDate.ToString("yyyy-MM"));

            context.Response.Write(isExistMonth.ToString());

        }

        private bool IsExistSameMonthData(string uploadDate)
        {
            bool isExist = false;
            SPList list = SPContext.Current.Web.Lists[WorkflowConfigName.CreditCardBill];

            SPQuery query = new SPQuery();
            string queryFormt = @"   <Where>
      <Eq>
         <FieldRef Name='UploadDate' />
         <Value Type='Text'>{0}</Value>
      </Eq>
   </Where>";

            query.Query = string.Format(queryFormt, uploadDate);

            if (list.GetItems(query).Count > 0)
            {
                isExist = true;
            }

            return isExist;
        }
    }
}