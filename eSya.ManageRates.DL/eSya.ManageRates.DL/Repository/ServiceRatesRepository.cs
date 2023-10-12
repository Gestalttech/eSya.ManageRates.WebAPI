using eSya.ManageRates.DL.Entities;
using eSya.ManageRates.DO;
using eSya.ManageRates.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.DL.Repository
{
    public class ServiceRatesRepository: IServiceRatesRepository
    {
        private readonly IStringLocalizer<ServiceRatesRepository> _localizer;
        public ServiceRatesRepository(IStringLocalizer<ServiceRatesRepository> localizer)
        {
            _localizer = localizer;
        }

        #region ServiceBaseRate
        public async Task<List<DO_ServiceBaseRate>> GetServiceBaseRateByBKeyRateTypeCurrCode(int businessKey, int ratetype, string currencycode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    //var defaultDate = DateTime.Now.Date;
                    //var result = db.GtEssrms
                    //    .Join(db.GtEssrcls,
                    //    s => s.ServiceClassId,
                    //    c => c.ServiceClassId,
                    //    (s, c) => new { s, c })
                    //    .Join(db.GtEssrgrs,
                    //    sc => sc.c.ServiceGroupId,
                    //    g => g.ServiceGroupId,
                    //    (sc, g) => new { sc, g })
                    //    .Join(db.GtEssrties,
                    //    scg => scg.g.ServiceTypeId,
                    //    t => t.ServiceTypeId,
                    //    (scg, t) => new { scg, t })
                    //    .GroupJoin(db.GtEssrbrs.Where(w => w.BusinessKey == businessKey && w.RateType == ratetype && w.CurrencyCode == currencycode).OrderByDescending(o => o.ActiveStatus),
                    //    scgt => scgt.scg.sc.s.ServiceId,
                    //    r => r.ServiceId,
                    //    (scgt, r) => new { scgt, r = r.FirstOrDefault() })
                    //             .Select(x => new DO_ServiceBaseRate
                    //             {
                    //                 ServiceId = x.scgt.scg.sc.s.ServiceId,
                    //                 ServiceDesc = x.scgt.scg.sc.s.ServiceDesc,
                    //                 ServiceTypeDesc = x.scgt.t.ServiceTypeDesc,
                    //                 ServiceGroupDesc = x.scgt.scg.g.ServiceGroupDesc,
                    //                 ServiceClassDesc = x.scgt.scg.sc.c.ServiceClassDesc,
                    //                 EffectiveDate = x.r != null ? x.r.EffectiveDate : defaultDate,
                    //                 ServiceRule = x.r != null ? x.r.ServiceRule : "F",
                    //                 OpbaseRate = x.r != null ? x.r.OpbaseRate : 0,
                    //                 IpbaseRate = x.r != null ? x.r.IpbaseRate : 0,
                    //                 IsIprateWardwise = x.r != null ? x.r.IsIprateWardwise : true,
                    //                 ActiveStatus = x.r != null ? x.r.ActiveStatus : true,
                    //             }
                    //    ).ToListAsync();
                    //return await result;


                    var defaultDate = DateTime.Now.Date;
                    var result = db.GtEssrms
                        .Join(db.GtEssrcls,
                        s => s.ServiceClassId,
                        c => c.ServiceClassId,
                        (s, c) => new { s, c })
                        .Join(db.GtEssrgrs,
                        sc => sc.c.ServiceGroupId,
                        g => g.ServiceGroupId,
                        (sc, g) => new { sc, g })
                        .Join(db.GtEssrties,
                        scg => scg.g.ServiceTypeId,
                        t => t.ServiceTypeId,
                        (scg, t) => new { scg, t })
                        .GroupJoin(db.GtEssrbrs.Where(w => w.BusinessKey == businessKey && w.RateType == ratetype && w.CurrencyCode == currencycode).OrderByDescending(o => o.ActiveStatus),
                        scgt => scgt.scg.sc.s.ServiceId,
                        r => r.ServiceId,
                        (scgt, r) => new { scgt, r })
                        .SelectMany(z => z.r.DefaultIfEmpty(),
                               (a, b) => new DO_ServiceBaseRate
                                 {
                                     ServiceId = a.scgt.scg.sc.s.ServiceId,
                                     ServiceDesc = a.scgt.scg.sc.s.ServiceDesc,
                                     ServiceTypeDesc = a.scgt.t.ServiceTypeDesc,
                                     ServiceGroupDesc = a.scgt.scg.g.ServiceGroupDesc,
                                     ServiceClassDesc = a.scgt.scg.sc.c.ServiceClassDesc,
                                     EffectiveDate = b != null ? b.EffectiveDate : defaultDate,
                                     ServiceRule = b != null ? b.ServiceRule : "F",
                                     OpbaseRate = b != null ? b.OpbaseRate : 0,
                                     IpbaseRate = b != null ? b.IpbaseRate : 0,
                                     IsIprateWardwise = b != null ? b.IsIprateWardwise : true,
                                     ActiveStatus = b != null ? b.ActiveStatus : true,
                                 }
                        ).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> AddOrUpdateServiceBaseRate(List<DO_ServiceBaseRate> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var ser_br in obj)
                        {
                            var ServiceExist = db.GtEssrbrs.Where(w => w.ServiceId == ser_br.ServiceId && w.BusinessKey == ser_br.BusinessKey && w.RateType == ser_br.RateType && w.CurrencyCode == ser_br.CurrencyCode && w.EffectiveTill == null).FirstOrDefault();
                            if (ServiceExist != null)
                            {
                                if (ser_br.EffectiveDate != ServiceExist.EffectiveDate)
                                {
                                    if (ser_br.EffectiveDate < ServiceExist.EffectiveDate)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W0155", Message = string.Format(_localizer[name: "W0155"]) };
                                    }
                                    ServiceExist.EffectiveTill = ser_br.EffectiveDate.AddDays(-1);
                                    ServiceExist.ModifiedBy = ser_br.UserID;
                                    ServiceExist.ModifiedOn = ser_br.CreatedOn;
                                    ServiceExist.ModifiedTerminal = ser_br.TerminalID;
                                    ServiceExist.ActiveStatus = false;

                                    var servicebaserate = new GtEssrbr
                                    {
                                        BusinessKey = ser_br.BusinessKey,
                                        ServiceId = ser_br.ServiceId,
                                        RateType = ser_br.RateType,
                                        CurrencyCode = ser_br.CurrencyCode,
                                        EffectiveDate = ser_br.EffectiveDate,

                                        ServiceRule = ser_br.ServiceRule,
                                        OpbaseRate = ser_br.OpbaseRate,
                                        IpbaseRate = ser_br.IpbaseRate,
                                        IsIprateWardwise = ser_br.IsIprateWardwise,

                                        ActiveStatus = ser_br.ActiveStatus,
                                        FormId = ser_br.FormId,
                                        CreatedBy = ser_br.UserID,
                                        CreatedOn = ser_br.CreatedOn,
                                        CreatedTerminal = ser_br.TerminalID
                                    };
                                    db.GtEssrbrs.Add(servicebaserate);


                                }
                                else
                                {
                                    ServiceExist.ServiceRule = ser_br.ServiceRule;
                                    ServiceExist.OpbaseRate = ser_br.OpbaseRate;
                                    ServiceExist.IpbaseRate = ser_br.IpbaseRate;
                                    ServiceExist.IsIprateWardwise = ser_br.IsIprateWardwise;
                                    ServiceExist.ActiveStatus = ser_br.ActiveStatus;

                                    ServiceExist.ModifiedBy = ser_br.UserID;
                                    ServiceExist.ModifiedOn = ser_br.CreatedOn;
                                    ServiceExist.ModifiedTerminal = ser_br.TerminalID;
                                }

                            }
                            else
                            {
                                if (ser_br.OpbaseRate != 0 || ser_br.IpbaseRate != 0)
                                {
                                    var servicebaserate = new GtEssrbr
                                    {
                                        BusinessKey = ser_br.BusinessKey,
                                        ServiceId = ser_br.ServiceId,
                                        RateType = ser_br.RateType,
                                        CurrencyCode = ser_br.CurrencyCode,
                                        EffectiveDate = ser_br.EffectiveDate,

                                        ServiceRule = ser_br.ServiceRule,
                                        OpbaseRate = ser_br.OpbaseRate,
                                        IpbaseRate = ser_br.IpbaseRate,
                                        IsIprateWardwise = ser_br.IsIprateWardwise,

                                        ActiveStatus = ser_br.ActiveStatus,
                                        FormId = ser_br.FormId,
                                        CreatedBy = ser_br.UserID,
                                        CreatedOn = ser_br.CreatedOn,
                                        CreatedTerminal = ser_br.TerminalID
                                    };
                                    db.GtEssrbrs.Add(servicebaserate);
                                }

                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }
            }
        }
        public async Task<DO_ReturnParameter> AddServiceBaseRate(List<DO_ServiceBaseRate> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var ser_br in obj)
                        {
                            if (ser_br.OpbaseRate != 0 || ser_br.IpbaseRate != 0)
                            {
                                var ServiceExist = db.GtEssrbrs.Where(w => w.ServiceId == ser_br.ServiceId && w.BusinessKey == ser_br.BusinessKey && w.RateType == ser_br.RateType && w.CurrencyCode == ser_br.CurrencyCode).FirstOrDefault();
                                if (ServiceExist != null)
                                {
                                    var ser = db.GtEssrms.Where(w => w.ServiceId == ser_br.ServiceId).Select(r => r.ServiceDesc).ToArray();
                                    return new DO_ReturnParameter() { Status = false, Message = string.Format(_localizer[name: "W0156"]) + ser[0].ToString() + "'" };
                                }
                                else
                                {
                                    var servicebaserate = new GtEssrbr
                                    {
                                        BusinessKey = ser_br.BusinessKey,
                                        ServiceId = ser_br.ServiceId,
                                        RateType = ser_br.RateType,
                                        CurrencyCode = ser_br.CurrencyCode,
                                        EffectiveDate = ser_br.EffectiveDate,

                                        ServiceRule = ser_br.ServiceRule,
                                        OpbaseRate = ser_br.OpbaseRate,
                                        IpbaseRate = ser_br.IpbaseRate,
                                        IsIprateWardwise = ser_br.IsIprateWardwise,

                                        ActiveStatus = ser_br.ActiveStatus,
                                        FormId = ser_br.FormId,
                                        CreatedBy = ser_br.UserID,
                                        CreatedOn = ser_br.CreatedOn,
                                        CreatedTerminal = ser_br.TerminalID
                                    };
                                    db.GtEssrbrs.Add(servicebaserate);
                                }

                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }
            }
        }
        #endregion
    }
}
