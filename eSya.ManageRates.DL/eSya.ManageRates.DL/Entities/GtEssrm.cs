using System;
using System.Collections.Generic;

namespace eSya.ManageRates.DL.Entities
{
    public partial class GtEssrm
    {
        public GtEssrm()
        {
            GtEspasms = new HashSet<GtEspasm>();
        }

        public int ServiceId { get; set; }
        public int ServiceClassId { get; set; }
        public int ServiceFor { get; set; }
        public string ServiceDesc { get; set; } = null!;
        public string? ServiceShortDesc { get; set; }
        public string Gender { get; set; } = null!;
        public string? InternalServiceCode { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEssrcl ServiceClass { get; set; } = null!;
        public virtual ICollection<GtEspasm> GtEspasms { get; set; }
    }
}
