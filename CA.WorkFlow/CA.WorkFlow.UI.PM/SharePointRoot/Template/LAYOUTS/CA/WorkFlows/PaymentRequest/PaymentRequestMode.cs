using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CA.WorkFlow.UI
{
    public enum PaymentRequestMode
    {
        New,
        Edit
    }

    public enum PaymentRequestStatus
    {
        NoCompleted,
        InProcess,
        Completed
    }
}
