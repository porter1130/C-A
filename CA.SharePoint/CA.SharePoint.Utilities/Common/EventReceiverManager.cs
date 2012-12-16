using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CA.SharePoint.EventReveivers
{
    public static class EventReceiverManager
    {
        /// <summary>
        /// �����¼��������������ڣ���ɾ����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <param name="eventTypes"></param>
        public static void SetEventReceivers(SPList list, Type t, params SPEventReceiverType[] eventTypes)
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

        /// <summary>
        /// �����б���¼�������
        /// </summary>
        /// <param name="list">Ҫ�����¼����������б�</param>
        /// <param name="t">�¼�������������</param>
        /// <param name="eventData">����������</param>
        /// <param name="eventTypes">�¼�����</param>
        public static void SetEventReceivers(SPList list, Type t, string eventData, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;
            //���ƶ����͵Ĵ������Ѿ���������ɾ��
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
        }

        /// <summary>
        /// ɾ���¼�������
        /// </summary>
        /// <param name="list">Ҫɾ���¼����������б�</param>
        /// <param name="t">Ҫɾ���¼�����������</param>
        public static void RemoveEventReceivers(SPList list, Type t)
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

        /// <summary>
        /// ��ȡ�¼�����������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t"></param>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public static SPEventReceiverDefinition GetEventDefinition(SPList list, Type t, SPEventReceiverType eventType)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            for (int i = list.EventReceivers.Count - 1; i >= 0; i--)
            {
                SPEventReceiverDefinition def = list.EventReceivers[i];

                if (def.Class == className && def.Type == eventType)
                    return def;
            }

            return null;
        }

        /// <summary>
        /// ����¼�������
        /// </summary>
        /// <param name="list">�б�</param>
        /// <param name="t">�¼�������������</param>
        /// <param name="eventTypes">�¼�����</param>
        public static void AddEventReceivers(SPList list, Type t, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            foreach (SPEventReceiverType et in eventTypes)
            {
                list.EventReceivers.Add(et, assambly, className);
            }

            list.Update();
        }

        public static void AddEventReceivers(SPList list, Type t, string eventData, params SPEventReceiverType[] eventTypes)
        {
            string assambly = t.Assembly.FullName;
            string className = t.FullName;

            foreach (SPEventReceiverType et in eventTypes)
            {
                SPEventReceiverDefinition ef = list.EventReceivers.Add();

                ef.Assembly = assambly;
                ef.Class = className;
                ef.Type = et;
                ef.Data = eventData;
                ef.Update();
            }
        }


    }
}
