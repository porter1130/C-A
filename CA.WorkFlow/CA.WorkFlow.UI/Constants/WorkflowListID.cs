using System.Collections;
namespace CA.WorkFlow.UI
{
    public static class WorkflowListID
    {
        private static Hashtable ht = new Hashtable();

        public static string GetListId(string listName)
        {
            var listId = string.Empty;
            if (ht.ContainsKey(listName))
            {
                listId = ht[listName].ToString();
            }
            else
            {
                listId = WorkFlowUtil.GetListId(listName);
                ht.Add(listName, listId);
            }
            return listId;
        }
    }
}