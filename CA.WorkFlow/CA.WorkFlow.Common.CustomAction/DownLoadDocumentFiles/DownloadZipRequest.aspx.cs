using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.IO;
using NewICSharpCode.SharpZipLib.Zip;
using System.Text;

namespace CA.WorkFlow.Common.CustomAction.DownLoadDocumentFiles
{
    public partial class DownloadZipRequest : LayoutsPageBase//System.Web.UI.Page// 
    {
        /// <summary>
        /// 文档库名
        /// </summary>
        string sListName = string.Empty;

        /// <summary>
        /// 项目中保存下载文件的根路径
        /// </summary>
        string sPath = "~/DownLoadDocumentFiles/";

        /// <summary>
        /// 当前ZIP文件保存的路径
        /// </summary>
        string sServerZipPath = string.Empty;
        
        /// <summary>
        /// 绝对路径前（相对于 相对路径的目录长度）
        /// </summary>
        int iDirectoryLength = 0;

        /// <summary>
        /// 所选中的文件的大小
        /// </summary>
        long lTotalSize = 0;
        
        /// <summary>
        /// 选 中 的 要 下载的项的ID
        /// </summary>
        string sIDS = string.Empty;

        /// <summary>
        /// zip包的完整路径
        /// </summary>
        string sZipFileFullPathName = string.Empty;


        /// <summary>
        /// 文件 长度
        /// </summary>
        int iParentFolderLength = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null!= Request.QueryString["ActionType"])
            {
                string sIDS = Request.QueryString["FilesID"];
                GetTotalSize(sIDS);
            }
            else
            {

                if (null != Request.QueryString["FilesID"])
                {
                    //DateTime tdstart = DateTime.Now;

                    string sPackage = string.Empty;
                    sIDS = Request.QueryString["FilesID"];
                    sPackage = StartCreateDownLoadPackage(sIDS);
                    if (sPackage.Length <= 0)
                    {
                        Response.Clear();
                        Response.Write("1");//1代表选的文件大于1G
                        Response.End();
                    }
                    Response.Clear();
                    Response.Write(sPackage);
                    Response.End();

                    /*DateTime dtend = DateTime.Now;

                    TimeSpan ts = dtend - tdstart;
                    double iAll = ts.TotalSeconds;
                
                    Response.Clear();
                    Response.Write("总时间：" + iAll);
                    Response.End();*/
                }
            
            }


        }


        #region 下载主体功能

        /// <summary>
        /// 开始创建压缩包
        /// </summary>
        /// <param name="sIDs"></param>
        /// <returns></returns>
        string StartCreateDownLoadPackage(string sIDs)
        {
            string sServerUrl = Request.QueryString["serverUrl"];
            SPWeb sweb = SPContext.Current.Site.OpenWeb();
            SPList sList = sweb.GetList(sServerUrl);
            sListName = sList.Title;

            string sAppath = HttpContext.Current.Request.ApplicationPath;

            string[] strArray=sIDs.Split('-');
            if (strArray.Length == 1)//单个文件 
            {
                SPListItem item = sList.GetItemById(int.Parse(strArray[0]));
                if (null == item.Folder)//是文件 类型 直接下载
                {
                    Response.Clear();
                    string sUrl = item.Url;
                    Response.Write(string.Concat(sweb.Url, "/", sUrl));
                    Response.End();
                }
            }

            string sCurrentFolderName = string.Concat(SPContext.Current.Web.CurrentUser.Name.Replace("\\", "-"),"-", DateTime.Now.ToString().Replace("/", "-").Replace(":", "-"));

            sServerZipPath = HttpContext.Current.Server.MapPath(string.Concat(sPath, sListName, "/"));
            iDirectoryLength = sServerZipPath.Length;
            string zipPack = string.Concat(sServerZipPath, sCurrentFolderName); 
            sZipFileFullPathName = string.Concat(zipPack, ".zip");
            CreateDirectory(sServerZipPath);

            NewCompress(sZipFileFullPathName, sList);
            string sSendClientPath = string.Concat(sAppath, sPath.Replace("~/", ""), sListName, "/", sCurrentFolderName, ".zip");
            return sSendClientPath;
        }

        /// <summary>
        /// 得到所选的文件大小并更改.exe文件的后缀名。
        /// </summary>
        /// <param name="item"></param>
        void OperationItem(SPListItem item)
        {
            string sServerSavePath = Server.MapPath(sPath);
            string sFullPath = string.Concat(sServerSavePath, item.Url);
            LimitPathDeep(sFullPath);
            if (item.Folder != null)
            {
                foreach (SPFile file in item.Folder.Files)
                {
                    OperationItem(file.Item);
                }
                foreach (SPFolder folder in item.Folder.SubFolders)
                {
                    OperationItem(folder.Item);
                }
            }
            else
            {
                lTotalSize += item.File.Length;//文件内容大小。
            }
        }

        /// <summary>
        /// 得到总大小
        /// </summary>
        /// <param name="sIDs"></param>
        /// <returns></returns>
        void GetTotalSize(string sIDs)
        {
            string sLargeSize = "0";
            string sServerUrl = Request.QueryString["serverUrl"];
            SPWeb sweb = SPContext.Current.Site.OpenWeb();
            SPList sList = sweb.GetList(sServerUrl);
            sListName = sList.Title;

            string sAppath = HttpContext.Current.Request.ApplicationPath;

            string[] strArray = sIDs.Split('-');

            foreach (string sId in strArray)
            {
                SPListItem item = sList.GetItemById(int.Parse(sId));
                OperationItem(item);
            }

            long lMSize=lTotalSize / 1024 / 1024;//换算成M
            if ( lMSize> 2000)//所选中的文件大于2G
            {
                sLargeSize = "1";//所选中的文件大于2G
            }
            string sResult = string.Format("{0}|{1}", sLargeSize, lMSize);
            Response.Clear();
            Response.Write(sResult);
            Response.End() ;
           //return sLargeSize;
        }

        #region  新的压缩方法


        void NewCompress(string sZipFileFullPathName,SPList sList)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                ZipOutputStream zs = new ZipOutputStream(File.Create(sZipFileFullPathName));//D:/project/Dowload/DownLoadFile/1-2-3.zip
                zs.SetLevel(0);

                foreach (string sId in sIDS.Split('-'))
                {
                    SPListItem item = sList.GetItemById(int.Parse(sId));
                    iParentFolderLength=item.Url.LastIndexOf('/')+1;
                    NewCreateZip(item, zs);
                }
                zs.Finish();
                zs.Close();
                GC.Collect();
            });
        }

        void NewCreateZip(SPListItem item, ZipOutputStream zs)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                if (null != item.Folder)
                {
                    foreach (SPFile file in item.Folder.Files)
                    {
                        NewCreateZip(file.Item, zs);
                    }
                    foreach (SPFolder folder in item.Folder.SubFolders)
                    {
                        NewCreateZip(folder.Item, zs);
                    }
                }
                else
                {
                    ZipEntry entry = new ZipEntry(item.File.Url.Substring(iParentFolderLength));
                    zs.PutNextEntry(entry);
                    int intReadLength = 0;
                    Stream s = item.File.OpenBinaryStream();
                    do
                    {
                        byte[] buffer = new byte[1024]; 
                        intReadLength = s.Read(buffer, 0, buffer.Length);
                        zs.Write(buffer, 0, intReadLength);
                    }
                    while (intReadLength == 1024);
                    s.Dispose();
                    s.Close();
                }
            });
        }
        /// <summary>
        ///  创建文件夹
        /// </summary>
        /// <param name="sDirectory"></param>
        void CreateDirectory(string sDirectory)
        {
            if (!Directory.Exists(sDirectory))
            {
                try
                {
                    LimitPathDeep(sDirectory);

                    SPSecurity.RunWithElevatedPrivileges(delegate
                    {
                        Directory.CreateDirectory(sDirectory);
                    });
                }
                catch (ExecutionEngineException ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 文件或文件夹深度字符长度限制。
        /// </summary>
        /// <param name="sPath"></param>
        void LimitPathDeep(string sPath)
        {
            if (sPath.Length > 240)
            {
                Response.Clear();
                Response.Write("0");//0代表文件或文件夹深度字符长度超过限制
                Response.End();
            }
        }
        #endregion

        #endregion
    }
}
