using eSya.ManageRates.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.IF
{
    public interface IServiceRatesRepository
    {
        #region ServiceBaseRate
        Task<List<DO_ServiceBaseRate>> GetServiceBaseRateByBKeyRateTypeCurrCode(int businessKey, int ratetype, string currencycode);
        Task<DO_ReturnParameter> AddOrUpdateServiceBaseRate(List<DO_ServiceBaseRate> obj);
        Task<DO_ReturnParameter> AddServiceBaseRate(List<DO_ServiceBaseRate> obj);

        #endregion
    }
}
