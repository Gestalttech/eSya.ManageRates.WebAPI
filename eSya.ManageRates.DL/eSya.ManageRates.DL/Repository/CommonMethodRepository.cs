using eSya.ManageRates.DL.Entities;
using eSya.ManageRates.DO;
using eSya.ManageRates.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DL.Repository
{
    public class CommonMethodRepository: ICommonMethodRepository
    {
        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + " - " + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_ApplicationCode>> GetApplicationCodesByCodeType(int codetype)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcapcds
                        .Where(w => w.ActiveStatus && w.CodeType == codetype)
                        .Select(r => new DO_ApplicationCode
                        {
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_CurrencyCode>> GetCurrencyCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var cc = db.GtEccucos
                        .Where(w => w.ActiveStatus)
                        .Select(c => new DO_CurrencyCode
                        {
                            CurrencyCode = c.CurrencyCode,
                            CurrencyName = c.CurrencyName
                        }).ToListAsync();

                    return await cc;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
