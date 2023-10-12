using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DO
{
    public class DO_ServiceCode
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int ServiceGroupId { get; set; }
        public int ServiceClassId { get; set; }
        public string ServiceDesc { get; set; }
        public string ServiceShortDesc { get; set; }
        public string Gender { get; set; }
        public bool IsServiceBillable { get; set; }
        public decimal ServiceCost { get; set; }
        public string InternalServiceCode { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TerminalID { get; set; }

        public string ServiceTypeDesc { get; set; }
        public string ServiceGroupDesc { get; set; }
        public string ServiceClassDesc { get; set; }

        public bool BusinessLinkStatus { get; set; }

        public List<DO_eSyaParameter> l_ServiceParameter { get; set; }
    }
}
