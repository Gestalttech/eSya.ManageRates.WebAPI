using System;
using System.Collections.Generic;

namespace eSya.ManageRates.DL.Entities
{
    public partial class GtEcbsln
    {
        public int BusinessId { get; set; }
        public int LocationId { get; set; }
        public int BusinessKey { get; set; }
        public string LocationDescription { get; set; } = null!;
        public string BusinessName { get; set; } = null!;
        public byte[] EBusinessKey { get; set; } = null!;
        public int Isdcode { get; set; }
        public int CityCode { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public string LocnDateFormat { get; set; } = null!;
        public int TaxIdentification { get; set; }
        public string ESyaLicenseType { get; set; } = null!;
        public byte[] EUserLicenses { get; set; } = null!;
        public byte[] EActiveUsers { get; set; } = null!;
        public byte[]? ENoOfBeds { get; set; }
        public bool? TolocalCurrency { get; set; }
        public bool TocurrConversion { get; set; }
        public bool TorealCurrency { get; set; }
        public bool IsBookOfAccounts { get; set; }
        public int BusinessSegmentId { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
