using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DO
{
    public class DO_SpecialtyCodes
    {
        public int SpecialtyID { get; set; }
        public string SpecialtyDesc { get; set; }
        public string Gender { get; set; }
        public string SpecialtyType { get; set; }
        public string AlliedServices { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
