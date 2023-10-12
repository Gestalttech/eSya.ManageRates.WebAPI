using System;
using System.Collections.Generic;

namespace eSya.ManageRates.DL.Entities
{
    public partial class GtEccuco
    {
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public decimal DecimalPlaces { get; set; }
        public bool ShowInMillions { get; set; }
        public bool SymbolSuffixToAmount { get; set; }
        public string? DecimalPortionWord { get; set; }
        public bool UsageStatus { get; set; }
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
