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
    public class ClinicServiceRepository: IClinicServiceRepository
    {
        private readonly IStringLocalizer<ClinicServiceRepository> _localizer;
        public ClinicServiceRepository(IStringLocalizer<ClinicServiceRepository> localizer)
        {
            _localizer = localizer;
        }

        #region ClinicVisitRate
        public async Task<List<DO_ClinicVisitRate>> GetClinicVisitRateByBKeyClinicTypeCurrCodeRateType(int businessKey, int clinictype, string currencycode, int ratetype)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    //var defaultDate = DateTime.Now.Date;
                    //var result = db.GtEsclsls.Where(w => w.BusinessKey == businessKey && w.ActiveStatus && (clinictype == 0 ? true : w.ClinicId == clinictype))
                    //    .Join(db.GtEssrms,
                    //    l => l.ServiceId,
                    //    s => s.ServiceId,
                    //    (l, s) => new { l, s })
                    //    .Join(db.GtEcapcds,
                    //    ls => ls.l.ClinicId,
                    //    c => c.ApplicationCode,
                    //    (ls, c) => new { ls, c })
                    //    .Join(db.GtEcapcds,
                    //    lsc => lsc.ls.l.ConsultationId,
                    //    n => n.ApplicationCode,
                    //    (lsc, n) => new { lsc, n })
                    //    .GroupJoin(db.GtEsclsts.Where(w => w.BusinessKey == businessKey && (clinictype == 0 ? true : w.ClinicId == clinictype) && w.CurrencyCode == currencycode && w.RateType == ratetype).OrderByDescending(o => o.ActiveStatus),
                    //    lscn => lscn.lsc.ls.l.ClinicId,
                    //    r => r.ClinicId,
                    //    (lscn, r) => new { lscn, r = r.Where(w => w.ConsultationId == lscn.lsc.ls.l.ConsultationId && w.ServiceId == lscn.lsc.ls.l.ServiceId).FirstOrDefault() })
                    //             .Select(x => new DO_ClinicVisitRate
                    //             {
                    //                 ServiceId = x.lscn.lsc.ls.s.ServiceId,
                    //                 ClinicId = x.lscn.lsc.c.ApplicationCode,
                    //                 ConsultationId = x.lscn.n.ApplicationCode,
                    //                 ServiceDesc = x.lscn.lsc.ls.s.ServiceDesc,
                    //                 ClinicDesc = x.lscn.lsc.c.CodeDesc,
                    //                 ConsultationDesc = x.lscn.n.CodeDesc,
                    //                 Tariff = x.r != null ? x.r.Tariff : 0,
                    //                 EffectiveDate = x.r != null ? x.r.EffectiveDate : defaultDate,
                    //                 ActiveStatus = x.r != null ? x.r.ActiveStatus : true,
                    //             }
                    //    ).ToListAsync();
                    //return await result;

                    var defaultDate = DateTime.Now.Date;
                    var result = db.GtEsclsls.Where(w => w.BusinessKey == businessKey && w.ActiveStatus && (clinictype == 0 ? true : w.ClinicId == clinictype))
                        .Join(db.GtEssrms,
                        l => l.ServiceId,
                        s => s.ServiceId,
                        (l, s) => new { l, s })
                        .Join(db.GtEcapcds,
                        ls => ls.l.ClinicId,
                        c => c.ApplicationCode,
                        (ls, c) => new { ls, c })
                        .Join(db.GtEcapcds,
                        lsc => lsc.ls.l.ConsultationId,
                        n => n.ApplicationCode,
                        (lsc, n) => new { lsc, n })
                        .GroupJoin(db.GtEsclsts.Where(w => w.BusinessKey == businessKey && (clinictype == 0 ? true : w.ClinicId == clinictype) && w.CurrencyCode == currencycode && w.RateType == ratetype).OrderByDescending(o => o.ActiveStatus),
                        lscn => new { lscn.lsc.ls.l.ClinicId,lscn.lsc.ls.l.ConsultationId,lscn.lsc.ls.l.ServiceId},
                        r => new { r.ClinicId,r.ConsultationId,r.ServiceId },
                        (lscn, r) => new { lscn, r  })
                        .SelectMany(z => z.r.DefaultIfEmpty(),
                                 (a, b) => new DO_ClinicVisitRate
                                 {
                                     ServiceId = a.lscn.lsc.ls.s.ServiceId,
                                     ClinicId = a.lscn.lsc.c.ApplicationCode,
                                     ConsultationId = a.lscn.n.ApplicationCode,
                                     ServiceDesc = a.lscn.lsc.ls.s.ServiceDesc,
                                     ClinicDesc = a.lscn.lsc.c.CodeDesc,
                                     ConsultationDesc = a.lscn.n.CodeDesc,
                                     Tariff =b != null ? b.Tariff : 0,
                                     EffectiveDate = b != null ? b.EffectiveDate : defaultDate,
                                     ActiveStatus =b != null ? b.ActiveStatus : true,
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
        public async Task<DO_ReturnParameter> AddOrUpdateClinicVisitRate(List<DO_ClinicVisitRate> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var c_visitrate in obj)
                        {
                            var ServiceExist = db.GtEsclsts.Where(w => w.ServiceId == c_visitrate.ServiceId && w.BusinessKey == c_visitrate.BusinessKey && w.ClinicId == c_visitrate.ClinicId && w.ConsultationId == c_visitrate.ConsultationId && w.CurrencyCode == c_visitrate.CurrencyCode && w.RateType == c_visitrate.RateType && w.EffectiveTill == null).FirstOrDefault();
                            if (ServiceExist != null)
                            {
                                if (c_visitrate.EffectiveDate != ServiceExist.EffectiveDate)
                                {
                                    if (c_visitrate.EffectiveDate < ServiceExist.EffectiveDate)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W0155", Message = string.Format(_localizer[name: "W0155"]) };
                                    }
                                    ServiceExist.EffectiveTill = c_visitrate.EffectiveDate.AddDays(-1);
                                    ServiceExist.ModifiedBy = c_visitrate.UserID;
                                    ServiceExist.ModifiedOn = c_visitrate.CreatedOn;
                                    ServiceExist.ModifiedTerminal = c_visitrate.TerminalID;
                                    ServiceExist.ActiveStatus = false;

                                    var clinicvisitrate = new GtEsclst
                                    {
                                        BusinessKey = c_visitrate.BusinessKey,
                                        ServiceId = c_visitrate.ServiceId,
                                        ClinicId = c_visitrate.ClinicId,
                                        ConsultationId = c_visitrate.ConsultationId,
                                        RateType = c_visitrate.RateType,
                                        CurrencyCode = c_visitrate.CurrencyCode,
                                        EffectiveDate = c_visitrate.EffectiveDate,
                                        Tariff = c_visitrate.Tariff,
                                        ActiveStatus = c_visitrate.ActiveStatus,
                                        FormId = c_visitrate.FormId,
                                        CreatedBy = c_visitrate.UserID,
                                        CreatedOn = c_visitrate.CreatedOn,
                                        CreatedTerminal = c_visitrate.TerminalID
                                    };
                                    db.GtEsclsts.Add(clinicvisitrate);


                                }
                                else
                                {
                                    ServiceExist.Tariff = c_visitrate.Tariff;
                                    ServiceExist.ActiveStatus = c_visitrate.ActiveStatus;

                                    ServiceExist.ModifiedBy = c_visitrate.UserID;
                                    ServiceExist.ModifiedOn = c_visitrate.CreatedOn;
                                    ServiceExist.ModifiedTerminal = c_visitrate.TerminalID;
                                }

                            }
                            else
                            {
                                if (c_visitrate.Tariff != 0)
                                {
                                    var clinicvisitrate = new GtEsclst
                                    {
                                        BusinessKey = c_visitrate.BusinessKey,
                                        ServiceId = c_visitrate.ServiceId,
                                        ClinicId = c_visitrate.ClinicId,
                                        ConsultationId = c_visitrate.ConsultationId,
                                        RateType = c_visitrate.RateType,
                                        CurrencyCode = c_visitrate.CurrencyCode,
                                        EffectiveDate = c_visitrate.EffectiveDate,
                                        Tariff = c_visitrate.Tariff,
                                        ActiveStatus = c_visitrate.ActiveStatus,
                                        FormId = c_visitrate.FormId,
                                        CreatedBy = c_visitrate.UserID,
                                        CreatedOn = c_visitrate.CreatedOn,
                                        CreatedTerminal = c_visitrate.TerminalID
                                    };
                                    db.GtEsclsts.Add(clinicvisitrate);
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
        public async Task<List<DO_ServiceCode>> GetServicesPerformedByDoctor()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEssrms
                        .Join(db.GtEspasms,
                        s => s.ServiceId,
                        p => p.ServiceId,
                        (s, p) => new { s, p })
                        .Where(w => w.p.ParameterId == 5 && w.p.ParmAction)
                                 .Select(x => new DO_ServiceCode
                                 {
                                     ServiceId = x.s.ServiceId,
                                     ServiceDesc = x.s.ServiceDesc
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
        #endregion


    }
}
