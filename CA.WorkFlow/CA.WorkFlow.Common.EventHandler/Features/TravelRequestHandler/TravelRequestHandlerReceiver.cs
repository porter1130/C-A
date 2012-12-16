using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using CA.WorkFlow.UI;
using CA.SharePoint.Utilities.Common;
using System.Collections;

namespace CA.WorkFlow.Eventhandler.EventHandlers.Features
{
    public class TravelRequestHandlerReceiver : SPItemEventReceiver
    {
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            SPListItem item = properties.ListItem;
            SPFieldUser userField = item.Fields["ApplicantSPUser"] as SPFieldUser;
            SPFieldUserValue userFieldValue = userField.GetFieldValue(item["ApplicantSPUser"].ToString()) as SPFieldUserValue;
            string status = item["Status"].ToString();
            string currentUserName = properties.UserLoginName;
            if (currentUserName != userFieldValue.User.LoginName)
            {
                if (!SecurityValidateForDelete(properties))
                {
                    properties.ErrorMessage = string.Format("You have no enough permissions to delete this item, please contact administrator");
                    properties.Status = SPEventReceiverStatus.CancelWithError;
                    properties.Cancel = true;
                }
            }
            else {
                if (status != "Pending") {
                    properties.ErrorMessage = string.Format("Your request has been submitted, so you can't delete it now!If you have any questions, please contact administrator.");
                    properties.Status = SPEventReceiverStatus.CancelWithError;
                    properties.Cancel = true;
                    }
            }
        }
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            SPListItem item1 = properties.ListItem;
            var delemans = item1["Delegates"].AsString();
            if (delemans.AsString().IsNullOrWhitespace())
            {
                return;
            }
            char[] split = { '#' };
            string[] deles = delemans.Split(split);
            List<SPPrincipal> principals = new List<SPPrincipal>();
            for (int i = 0; i < deles.Length; i++)
            {
                if (deles[i].IsNullOrWhitespace())
                {
                    continue;
                }
                principals.Add(item1.Web.Users[deles[i]]);
            }

            if (principals.Count > 0)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                        {
                            try
                            {
                                SPListItem item = web.Lists["Travel Request Workflow2"].GetItemById(item1.ID);
                                if (!item.HasUniqueRoleAssignments)
                                {
                                    item.BreakRoleInheritance(true);
                                }

                                SPRoleDefinition AdminRoleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Administrator);
                                SPRoleDefinition GuestRoleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Guest);
                                foreach (SPPrincipal principal in principals)
                                {
                                    SPRoleAssignment RoleAssignment = new SPRoleAssignment(principal);
                                    RoleAssignment.RoleDefinitionBindings.Add(AdminRoleDefinition);
                                    RoleAssignment.RoleDefinitionBindings.Remove(GuestRoleDefinition);
                                    item.RoleAssignments.Remove(principal);
                                    item.RoleAssignments.Add(RoleAssignment);
                                }
                                item["Delegates"] = string.Empty;
                                base.DisableEventFiring();
                                item.Update();
                                base.EnableEventFiring();
                            }
                            catch (Exception ex)
                            {
                                //TO-DO
                            }
                        }
                    }
                });
            }

        }

        private static bool DoesPrincipalHasPermissions(ISecurableObject item, SPPrincipal principal, SPBasePermissions permissions)
        {
            SPRoleAssignment roleAssignment = null;
            try
            {
                roleAssignment = item.RoleAssignments.GetAssignmentByPrincipal(principal);
            }
            catch
            {
                //if the user has no permission on the item (SPPrincipal is not in permissionlist -> item.RoleAssignments is empty), an exception is thrown.
                return false;
            }
            foreach (SPRoleDefinition definition in roleAssignment.RoleDefinitionBindings)
            {
                if ((definition.BasePermissions & permissions) == permissions)
                {
                    return true;
                }
            }
            return false;
        }

        protected bool SecurityValidateForDelete(SPItemEventProperties properties)
        {
            bool isValid = false;
            var currUser = properties.UserLoginName;

            var siteId = properties.SiteId;

            SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(siteId))
                    {
                        using (SPWeb web = site.OpenWeb("WorkFlowCenter"))
                        {
                            try
                            {
                                SPRoleAssignment roleAssignment = web.RoleAssignments.GetAssignmentByPrincipal(web.Users[currUser]);
                                SPRoleDefinitionBindingCollection roleDefs = roleAssignment.RoleDefinitionBindings;
                                foreach (SPRoleDefinition roleDef in roleDefs)
                                {
                                    if (web.RoleDefinitions.GetByType(SPRoleType.Administrator).Name.Equals(roleDef.Name))
                                    {
                                        isValid = true;
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //TO-DO
                            }
                        }
                    }
                });

            return isValid;
        }
    }
}
