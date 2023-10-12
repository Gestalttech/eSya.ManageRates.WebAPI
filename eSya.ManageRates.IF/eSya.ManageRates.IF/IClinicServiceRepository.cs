using eSya.ManageRates.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.IF
{
    public interface IClinicServiceRepository
    {
        #region ClinicVisitRate
        Task<List<DO_ClinicVisitRate>> GetClinicVisitRateByBKeyClinicTypeCurrCodeRateType(int businessKey, int clinictype, string currencycode, int ratetype);
        Task<DO_ReturnParameter> AddOrUpdateClinicVisitRate(List<DO_ClinicVisitRate> obj);
        Task<List<DO_ServiceCode>> GetServicesPerformedByDoctor();
        #endregion
    }
}
