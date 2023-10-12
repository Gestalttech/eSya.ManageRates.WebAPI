using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DO
{
    public class DO_eSyaParameter
    {
        public int ParameterID { get; set; }
        public string ParameterValue { get; set; }
        public bool ParmAction { get; set; }
        public decimal ParmValue { get; set; }
        public decimal ParmPerc { get; set; }
        public string ParmDesc { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
