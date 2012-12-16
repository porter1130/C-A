
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CA.SharePoint.Common
{
    /// <summary>
    /// �¼�����
    /// </summary>
    public static class EventUtil
    {

        /// <summary>
        /// �����¼��������������ڣ���ɾ����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <param name="eventTypes"></param>
        public static void SetEventReceivers(SPList list, Type t, params SPEventReceiverType[] eventTypes)
        {
            try
            {
                string assambly = t.Assembly.FullName;
                string className = t.FullName;

                for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
                {
                    SPEventReceiverDefinition def = list.EventReceivers[i];

                    if (def.Class == className)
                        def.Delete();
                }

                foreach (SPEventReceiverType et in eventTypes)
                {
                    list.EventReceivers.Add(et, assambly, className);
                }

                list.Update();

            }
            catch
            {
                throw;
            }
        }

        public static void SetEventReceivers(SPList list, Type t, string eventData, params SPEventReceiverType[] eventTypes)
        {
            try
            {
                string assambly = t.Assembly.FullName;
                string className = t.FullName;

                for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
                {
                    SPEventReceiverDefinition def = list.EventReceivers[i];

                    if (def.Class == className)
                        def.Delete();
                }

                foreach (SPEventReceiverType et in eventTypes)
                {
                    SPEventReceiverDefinition ef = list.EventReceivers.Add();

                    ef.Assembly = assambly;
                    ef.Class = className;
                    ef.Type = et;
                    ef.Data = eventData;
                    ef.Update();
                }

                list.Update();

            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// �����¼�������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <param name="eventTypes"></param>
       public static void AddEventReceivers( SPList list , Type t , params SPEventReceiverType[] eventTypes )
        {
            try
            {
                string assambly = t.Assembly.FullName;
                string className = t.FullName;

                foreach (SPEventReceiverType et in eventTypes)
                {
                     list.EventReceivers.Add( et , assambly, className);
                }

                list.Update();

            }
            catch 
            {
                throw;
            }
        }

        public static void AddEventReceivers(SPList list, Type t, string eventData, params SPEventReceiverType[] eventTypes  )
        {
            try
            {
                string assambly = t.Assembly.FullName;
                string className = t.FullName;

                foreach (SPEventReceiverType et in eventTypes)
                {
                    SPEventReceiverDefinition ef = list.EventReceivers.Add();

                    ef.Assembly = assambly ;
                    ef.Class = className ;
                    ef.Type = et ;
                    ef.Data = eventData;
                    ef.Update();
                }

                //list.Update();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// ɾ���¼�������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        public static void RemoveEventReceivers( SPList list, Type t )
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
            {
                SPEventReceiverDefinition def = list.EventReceivers[i];

                if (def.Class == className)
                    def.Delete();
            }

            list.Update();
        }
    }
}