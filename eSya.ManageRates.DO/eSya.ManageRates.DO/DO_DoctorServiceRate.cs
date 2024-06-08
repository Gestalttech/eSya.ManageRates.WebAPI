using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DO
{
    public class DO_DoctorServiceRate
    {
        public int BusinessKey { get; set; }
        public int ClinicId { get; set; }
        public int ConsultationId { get; set; }
        public int ServiceId { get; set; }
        public int RateType { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Tariff { get; set; }
        public bool ActiveStatus { get; set; }
        public DateTime? EffectiveTill { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? ServiceDesc { get; set; }
        public string? DoctorDesc { get; set; }

    }
}
