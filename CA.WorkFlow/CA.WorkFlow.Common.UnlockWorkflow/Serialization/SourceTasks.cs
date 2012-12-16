using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CA.WorkFlow.Common.UnlockWorkflow.Serialization
{
    [Serializable, XmlRoot("Tasks")]
    public class SourceTasks
    {
        [XmlElement("Task", typeof(WorkflowTask))]
        public List<WorkflowTask> tasks { get; set; }

        public SourceTasks()
        {
            tasks = new List<WorkflowTask>();
        }
    }
}