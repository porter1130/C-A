using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint;
using System.IO;
using CA.SharePoint.Utilities.Common;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.SharePoint.Utilities;
using System.Text;
using System.DirectoryServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using System.Net.Mail;
using System.Net;

namespace CA.SharePoint.Web
{
    public partial class PublishNote : SPLayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string listName = "MarketingCommunicationNotes";
                if (SPContext.Current.List != null)
                {
                    listName = SPContext.Current.List.Title;
                }
                this.Title = listName;
                this.PagePath.Add(listName, SPContext.Current.List.DefaultViewUrl);
                this.PagePath.Add("New Item", "");
            }

            this.btnSave.Click += new ImageClickEventHandler(btnSave_Click);
            this.btnCancle.Click += new ImageClickEventHandler(btnCancle_Click);

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            SPList list = SPContext.Current.List;
            SPListItem item = list.Items.Add();
            item["Title"] = this.txtTitle.Text.Trim();
            item["Content"] = this.formFieldBody.Value;

            item.Web.AllowUnsafeUpdates = true;
            item.Update();

            
            //string sendmail = ConfigurationManager.AppSettings["mcnoticesendmail"] + "";
            //if (sendmail.ToLower() == "on")
            //{
            //    //send mails
            //    string rootweburl = ConfigurationManager.AppSettings["rootweburl"] + "";
            //    if (string.IsNullOrEmpty(rootweburl))
            //    {
            //        rootweburl = "http://cnshsps.cnaidc.cn:91";
            //    }
            //    SPSite site = new SPSite(rootweburl);
            //    SPWeb web = site.OpenWeb("documentcenter");
            //    //SPList mclist = SPContext.Current.Web.Lists["MarketingCommunication"];
            //    SPList mclist = web.Lists["Marketing Communication"];


            //    string libraryFolder = ConfigurationManager.AppSettings["marketingcommunicationlibrary"] + "";
            //    if (string.IsNullOrEmpty(libraryFolder))
            //    {
            //        libraryFolder = "MarketingCommunication/MarketingCommunication";
            //    }
            //    SPFolder folder = GetFolder(web, mclist, libraryFolder);

            //    if (folder != null)
            //    {
            //        List<string> mailList = new List<string>();

            //        try
            //        {
            //            foreach (SPRoleAssignment role in folder.Item.RoleAssignments)
            //            {
            //                string membertype = role.Member.GetType().ToString();
            //                if (membertype == "Microsoft.SharePoint.SPGroup")
            //                {
            //                    string groupName = ((SPGroup)role.Member).Name;
            //                    List<string> names = UserProfileUtil.UserListInGroup(groupName);

            //                    foreach (string name in names)
            //                    {
            //                        if (name.Equals("CNAIDC\\" + groupName, StringComparison.CurrentCultureIgnoreCase))
            //                        {
            //                            GetEmailsByGroup(mailList, groupName);
            //                        }
            //                        else
            //                        {
            //                            try
            //                            {
            //                                Employee mcuser = UserProfileUtil.GetEmployee(name);
            //                                mailList.Add(mcuser.WorkEmail);
            //                            }
            //                            catch
            //                            { }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    try
            //                    {
            //                        Employee mcuser = UserProfileUtil.GetEmployee(role.Member.ToString());
            //                        mailList.Add(mcuser.WorkEmail);
            //                    }
            //                    catch
            //                    { }
            //                }
            //            }
            //        }
            //        catch(Exception ex)
            //        {
            //            throw ex;
            //        }

            //        string mfrom = ConfigurationManager.AppSettings["mcnoticemailfrom"] + "";
            //        if (string.IsNullOrEmpty(mfrom))
            //        {
            //            mfrom = "spsadmin@C-AND-A.CN";
            //        }

            //        StringDictionary dict = new StringDictionary();
            //        mailList.Sort();
            //        dict.Add("from", mfrom);
            //        dict.Add("to", string.Join(";", mailList.Distinct().ToArray()));
            //        dict.Add("bcc", mfrom);
            //        dict.Add("subject", "Marketing Communication Note Notification");
            //        StringBuilder mcontent = new StringBuilder();
            //        mcontent.Append("<html><head></head><body>");

            //        string mbody = ConfigurationManager.AppSettings["mcnoticecontent"] + "";
            //        if (string.IsNullOrEmpty(mbody))
            //        {
            //            mbody = "A new Marketing Communication Note has been added, please go to Intranet to check.<br/> 今天有新的市场部通讯上传，请查看。<br/>";
            //        }
            //        else
            //        {
            //            mbody = mbody.Replace("\n", "<br/>");
            //        }
            //        string link =rootweburl + "/documentcenter/_layouts/ca/marketingcommunication.aspx";

            //        mcontent.Append(mbody + "<br /><a href='{0}' target='_blank'>Marketing Communication</a></body></html>");

            //        SPUtility.SendEmail(web, dict, string.Format(mcontent.ToString(), link));
            //    }
            //}

            var mails = new List<string>();                 //接收邮件的邮箱列表
            var groups = new List<string>();                //ad group
            var membersNotInProfile = new List<string>();   //ad user
            var workEmails = new List<string>();            //userprofile

            UserProfileManager profileManager = null;

            try
            {
                profileManager = new UserProfileManager(ServerContext.Current);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(string.Format("!!!{0}", ex.Message));
            }

            SPSite site = null;
            SPWeb documentCenter = null;

            try
            {
                site = SPContext.Current.Site;
                documentCenter = site.OpenWeb("documentcenter");

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    #region set groups, membersNotInProfile, workEmails

                    var folder = documentCenter.GetFolder(ConfigurationManager.AppSettings["marketingcommunicationlibrary"]);

                    if (folder.Exists)
                    {
                        foreach (SPRoleAssignment role in folder.Item.RoleAssignments)
                        {
                            var membertype = role.Member.GetType().ToString();
                            if (membertype == "Microsoft.SharePoint.SPGroup")
                            {
                                groups.Add(((SPGroup)role.Member).Name);
                            }
                            else
                            {
                                if (profileManager != null)
                                {
                                    try
                                    {
                                        var profile = profileManager.GetUserProfile(role.Member.ToString());
                                        var workEmail = profile[PropertyConstants.WorkEmail].Value + "";
                                        if (!string.IsNullOrEmpty(workEmail))
                                        {
                                            workEmails.Add(workEmail);
                                        }
                                    }
                                    catch (UserNotFoundException)
                                    {
                                        membersNotInProfile.Add(role.Member.Name);
                                    }
                                }
                                else
                                {
                                    membersNotInProfile.Add(role.Member.Name);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine(string.Format("!!!{0} not exists", folder.Url));
                    }

                    #endregion
                });
            }
            catch (FileNotFoundException)
            {
                //Console.WriteLine(string.Format("!!!{0}", ex.Message));
            }

            if (groups.Count > 0)
            {

                #region get mails by ad groups

                var filterBuilder = new StringBuilder("(|");
                foreach (var group in groups)
                {
                    filterBuilder.AppendFormat("(&(objectClass=group)(cn={0}))", group);
                }
                filterBuilder.Append(")");

                try
                {
                    var de = new DirectoryEntry(ConfigurationManager.AppSettings["grouppath"],
                                    ConfigurationManager.AppSettings["deusername"],
                                    ConfigurationManager.AppSettings["depassword"],
                                    AuthenticationTypes.Secure);
                    de.AuthenticationType = AuthenticationTypes.Secure;

                    using (de)
                    {
                        var ds = new DirectorySearcher(de, filterBuilder.ToString());
                        //ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                        var src = ds.FindAll();

                        foreach (SearchResult sr in src)
                        {﻿﻿
                            foreach (string userpath in sr.Properties["member"])
                            {
                                var user = new DirectoryEntry("LDAP://" + userpath,
                                    ConfigurationManager.AppSettings["deusername"],
                                    ConfigurationManager.AppSettings["depassword"],
                                    AuthenticationTypes.Secure);
                                var mail = user.Properties["mail"].Value + "";
                                if (!string.IsNullOrEmpty(mail))
                                {
                                    mails.Add(mail);
                                    //Console.WriteLine(string.Format("{0} from userpath {1}", mail, userpath));
                                }
                            }
                        }
                    }
                }
                catch (COMException ex)
                {
                    //Console.WriteLine(string.Format("!!!COMException:{0}", ex.Message));
                    WriteLog(string.Format("seek groups in {0}(Marketing Communication Note Notification):\n{1}",
                       ConfigurationManager.AppSettings["grouppath"], ex.Message),
                       EventLogEntryType.Error);
                }

                #endregion

            }

            if (workEmails.Count > 0)
            {
                mails.AddRange(workEmails);
                //Console.WriteLine(string.Format("\n{0}\n^^^^^from profiles({1})^^^^^\n", string.Join(", ", workEmails.ToArray()), workEmails.Count));
            }

            if (membersNotInProfile.Count > 0)
            {
                foreach (var memberName in membersNotInProfile)
                {
                    var memberMail = GetMailByADUser(memberName);
                    if (!string.IsNullOrEmpty(memberMail))
                    {
                        mails.Add(memberMail);
                        //Console.WriteLine(string.Format("{0} from ad user {1}", memberMail, memberName));
                    }
                }
            }

            //var tmpMails = mails.Distinct().ToArray();
            //Console.WriteLine(string.Format("\n**********\n{0}\n({1})", string.Join("  ", tmpMails), tmpMails.Count()));
            //Console.ReadKey();

            if (mails.Count > 0)
            {
                #region send email

                mails.Sort();
                var mailsToSend = mails.Distinct().ToArray();

                var mfrom = ConfigurationManager.AppSettings["mcnoticemailfrom"];
                if (string.IsNullOrEmpty(mfrom))
                {
                    mfrom = "spsadmin@C-AND-A.CN";
                }

                var mcontent = new StringBuilder();
                mcontent.Append("<html><head></head><body>");

                var mbody = ConfigurationManager.AppSettings["mcnoticecontent"];
                if (string.IsNullOrEmpty(mbody))
                {
                    mbody = "A new Marketing Communication Note has been added, please go to Intranet to check.<br/> 今天有新的市场部通讯上传，请查看。<br/>";
                }
                else
                {
                    mbody = mbody.Replace("\n", "<br/>");
                }

                mcontent.Append(mbody);

                mcontent.Append(string.Format("<br /><a href='{0}/{1}/_layouts/ca/marketingcommunication.aspx' target='_blank'>Marketing Communication</a></body></html>",
                    ConfigurationManager.AppSettings["rootweburl"],
                    "documentcenter"));

                var mailServer = new SmtpClient(ConfigurationManager.AppSettings["smtpserver"]);

                mailServer.Credentials = CredentialCache.DefaultNetworkCredentials;

                var mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(mfrom);
                mailMessage.Subject = "Marketing Communication Note Notification";
                mailMessage.Body = mcontent.ToString();
                mailMessage.IsBodyHtml = true;

                if (ConfigurationManager.AppSettings["environment"] == "production")
                {
                    foreach (var mail in mailsToSend)
                    {
                        mailMessage.To.Add(mail);
                    }
                }
                else
                {
                    mailMessage.To.Add(ConfigurationManager.AppSettings["exampleto"]);
                }

                try
                {
                    mailServer.Send(mailMessage);
                    WriteLog(string.Format("{0} mails (Marketing Communication Note Notification) sent.", mailsToSend.Count()),
                        EventLogEntryType.Information);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("sending mails (Marketing Communication Note Notification):\n{0}",
                         ex.InnerException.Message),
                         EventLogEntryType.Error);
                }

                #endregion
            }

            GoRedirect();

        }

        //public static SPFolder GetFolder(SPWeb web, SPList list, string folderURL)
        //{
        //    if (String.IsNullOrEmpty(folderURL))
        //        return list.RootFolder;

        //    string folderFullURL = folderURL.TrimStart('/');

        //    SPFolder f = web.GetFolder(folderFullURL);
        //    if (f.Exists)
        //        return f;
        //    else
        //        return null;
        //}

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            GoRedirect();
        }

        private void GoRedirect()
        {
            if (Request.QueryString["Source"] != null && !string.IsNullOrEmpty(Request.QueryString["Source"].ToString()))
                this.Page.Response.Redirect(Request.QueryString["Source"].ToString());
            else
                this.Page.Response.Redirect(SPContext.Current.List.DefaultViewUrl);
        }

        //private void GetEmailsByGroup(List<string> mailList, string groupName)
        //{
        //    DirectoryEntry objADAM = new DirectoryEntry("LDAP://OU=dlgroup,dc=cnaidc,dc=cn", "cnaidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
        //    DirectorySearcher objSearchADAM = new DirectorySearcher(objADAM);
        //    objSearchADAM.Filter = "(&(objectClass=group)(cn=" + groupName + "))";
        //    //objSearchADAM.SearchScope = System.DirectoryServices.SearchScope.Subtree;
        //    SearchResult results = objSearchADAM.FindOne();
        //    objADAM.Close();

        //    DirectoryEntry deGroup = new DirectoryEntry(results.Path, "cnaidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
        //    //assign a property collection
        //    System.DirectoryServices.PropertyCollection pcoll = deGroup.Properties;
        //    int n = pcoll["member"].Count;

        //    if (n > 0)
        //    {
        //        foreach (string userpath in pcoll["member"])
        //        {

        //            DirectoryEntry objUserEntry = new DirectoryEntry("LDAP://" + userpath, "cnidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
        //            //objUserEntry.RefreshCache();

        //            PropertyCollection userProps = objUserEntry.Properties;
        //            objUserEntry.Close();

        //            if (!string.IsNullOrEmpty(userProps["mail"].Value + ""))
        //            {
        //                mailList.Add(userProps["mail"].Value.ToString());
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception("No groups found");
        //    }
        //}

        void WriteLog(string err, EventLogEntryType type)
        {
            if (!EventLog.SourceExists("C&A"))
            {
                EventLog.CreateEventSource("C&A", "Mail");
            }
            var myLog = new EventLog();
            myLog.Source = "C&A";
            myLog.WriteEntry(err, type);
        }

        string GetMailByADUser(string username)
        {
            try
            {
                using (var user = new DirectoryEntry(string.Format(ConfigurationManager.AppSettings["userpath"], username),
                                    ConfigurationManager.AppSettings["deusername"],
                                    ConfigurationManager.AppSettings["depassword"],
                                    AuthenticationTypes.Secure))
                {
                    return user.Properties["mail"].Value + "";
                }
            }
            catch (COMException)
            {
                //Console.WriteLine(string.Format("!!!{0} not in profile and COMException:{1}", username, ex.Message));
            }

            return string.Empty;
        }
    }
}
