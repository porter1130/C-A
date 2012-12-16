using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using System.Web.UI.HtmlControls;
using QuickFlow;
using System.Runtime.Serialization;

namespace CA.WorkFlow.UI.Code
{
    [DataContractAttribute]
    class ItemCode
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Descrption { get; set; }

        [DataMember]
        public string AssetClass { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string VendorId { get; set; }

        [DataMember]
        public string DeliveryPeriod { get; set; }

        [DataMember]
        public string ItemType { get; set; }

        [DataMember]
        public string TaxValue { get; set; }

        [DataMember]
        public string UnitPrice { get; set; }
    }

    [DataContractAttribute]
    class Vendor
    {
        [DataMember]
        public string VendorId { get; set; }

        [DataMember]
        public string Name { get; set; }

        //[DataMember]
        //public string Address { get; set; }

        //[DataMember]
        //public string PostCode { get; set; }

        //[DataMember]
        //public string ContactPerson { get; set; }

        //[DataMember]
        //public string Phone { get; set; }

        //[DataMember]
        //public string Fax { get; set; }

        //[DataMember]
        //public string Email { get; set; }

    }
}
