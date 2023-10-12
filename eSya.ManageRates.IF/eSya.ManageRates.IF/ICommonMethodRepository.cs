using eSya.ManageRates.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.IF
{
    public interface ICommonMethodRepository
    {
        Task<List<DO_BusinessLocation>> GetBusinessKey();
        Task<List<DO_ApplicationCode>> GetApplicationCodesByCodeType(int codetype);
        Task<List<DO_CurrencyCode>> GetCurrencyCodes();
    }
}
