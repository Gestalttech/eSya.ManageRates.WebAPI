using System;
using System.Collections.Generic;

namespace eSya.ManageRates.DL.Entities
{
    public partial class GtEssrgr
    {
        public GtEssrgr()
        {
            GtEssrcls = new HashSet<GtEssrcl>();
        }

        public int ServiceGroupId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceGroupDesc { get; set; } = null!;
        public int ServiceCriteria { get; set; }
        public int PrintSequence { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEssrty ServiceType { get; set; } = null!;
        public virtual ICollection<GtEssrcl> GtEssrcls { get; set; }
    }
}
