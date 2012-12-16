using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Globalization;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using CA.WorkFlow.UI;
using CA.SharePoint.Utilities.Common;

namespace CA.WorkFlow.Eventhandler.EventHandlers.Features
{
    public class TravelRequestHandlerReceiver : SPItemEventReceiver
    {

        public override void ItemAdded(SPItemEventProperties properties)
        {
            SPListItem item = properties.ListItem;
            var delemans = item["Delegates"].AsString();
            char[] split = { '#'};
            string[] deles = delemans.Split(split);
            List<object> deleList = new List<object>();
            for (int i = 0; i < deles.Length; i++)
            {
                string[] person = new string[4];
                person[0] = deles[i];
                person[1] = "zhoutaomtv@gmail.com";
                person[2] = "test";
                person[3] = "N/A";
                deleList.Add(person);
            }
            if (deleList.Count > 0)
            {
                WorkFlowUtil.AssignPermission(item.ParentList.ID, item.ID, deleList);
            }            
        }
       
    }
}
