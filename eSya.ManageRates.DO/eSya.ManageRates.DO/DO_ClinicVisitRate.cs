using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DO
{
    public class DO_ClinicVisitRate
    {
        public int BusinessKey { get; set; }
        public int ClinicId { get; set; }
        public int ConsultationId { get; set; }
        public int ServiceId { get; set; }
        public int RateType { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Tariff { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public bool ActiveStatus { get; set; }

        public int DoctorId { get; set; }

        public string? ClinicDesc { get; set; }
        public string? ConsultationDesc { get; set; }
        public string? ServiceDesc { get; set; }
        public string? DoctorName { get; set; }

        public string FormId { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TerminalID { get; set; }
    }
}
