using CA.SharePoint;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.SharePoint;
using System.Diagnostics;
using System;
using System.Linq;

namespace CA.WorkFlow.UI
{
    public static class WorkflowPerson
    {
        private static List<string> ceos = null;
        private static List<string> cfos = null;
        private static List<string> mtms = null;
        private static int lastDay = -1;

        static WorkflowPerson()
        {
            init();
        }

        private static void init()
        {
            int currDay = DateTime.Now.DayOfYear;
            if (currDay == lastDay)
            {
                return;
            }
            var ceoGroup = ConfigurationManager.AppSettings["CEOGroup"];
            var cfoGroup = ConfigurationManager.AppSettings["CFOGroup"];
            var mtmGroup = ConfigurationManager.AppSettings["MTMGroup"];
            if (string.IsNullOrEmpty(ceoGroup) || string.IsNullOrEmpty(cfoGroup) || string.IsNullOrEmpty(mtmGroup))
            {
                CommonUtil.logError("Configuration Invalid: CEOGroup, CFOGroup or MTMGroup is not configurated in web.config.");
            }

            ceos = string.IsNullOrEmpty(ceoGroup) ? new List<string>() : UserProfileUtil.UserListInGroup(ceoGroup);
            cfos = string.IsNullOrEmpty(cfoGroup) ? new List<string>() : UserProfileUtil.UserListInGroup(cfoGroup);
            List<string> tmps = string.IsNullOrEmpty(mtmGroup) ? new List<string>() : UserProfileUtil.UserListInGroup(mtmGroup);
            //MTM are persons except by ceo and cfo
            mtms = new List<string>();
            foreach (string account in tmps)
            {
                if (!(ceos.Contains(account, new MyCaseInsensitiveComparer()) || cfos.Contains(account, new MyCaseInsensitiveComparer())))
                {
                    mtms.Add(account);
                }
            }
            lastDay = currDay;
            CommonUtil.logInfo(DateTime.Now.ToString() + " Init the workflow person successfully.");
        }

        public static bool IsCEO(string account)
        {
            init();
            return ceos.Contains(account, new MyCaseInsensitiveComparer());
        }

        public static bool IsCFO(string account)
        {
            init();
            return cfos.Contains(account, new MyCaseInsensitiveComparer());
        }

        public static bool IsMTM(string account)
        {
            init();
            return !ceos.Contains(account, new MyCaseInsensitiveComparer()) && !cfos.Contains(account, new MyCaseInsensitiveComparer()) && mtms.Contains(account, new MyCaseInsensitiveComparer());
        }

        //CEO, CFO and MTM are high level persons
        public static bool IsHighLevel(string account)
        {
            init();
            return ceos.Contains(account, new MyCaseInsensitiveComparer()) || cfos.Contains(account, new MyCaseInsensitiveComparer()) || mtms.Contains(account, new MyCaseInsensitiveComparer());
        }

        public static List<string> GetCEO()
        {
            return ceos;
        }

        public static List<string> GetCFO()
        {
            return cfos;
        }

        public static List<string> GetMTM()
        {
            return mtms;
        }

        public class MyCaseInsensitiveComparer : IEqualityComparer<string>
        {

            public bool Equals(string x, string y)
            {
                return x.Equals(y, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}