﻿using eSya.ManageRates.DL.Entities;
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
    public class DoctorServiceRateRepository: IDoctorServiceRateRepository
    {
        private readonly IStringLocalizer<DoctorServiceRateRepository> _localizer;
        public DoctorServiceRateRepository(IStringLocalizer<DoctorServiceRateRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Doctor Service Rate
        public async Task<List<DO_DoctorServiceRate>> GetDoctorServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int clinicId,int consultationId,int specialtyId, int doctorId, string currencycode, int ratetype)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    //var defaultDate = DateTime.Now.Date;
                    //var result = db.GtEspasms.Where(x => x.ParameterId == 5 && x.ActiveStatus)
                    //    .Join(db.GtEssrms,
                    //    l => l.ServiceId,
                    //    s => s.ServiceId,
                    //    (l, s) => new { l, s })
                    //    .GroupJoin(db.GtEscdsts.Where(w => w.BusinessKey == businessKey && w.ServiceId == serviceId && w.DoctorId == doctorId && w.CurrencyCode == currencycode && w.RateType == ratetype),
                    //    lscn => lscn.s.ServiceId,
                    //    r => r.ServiceId,
                    //   (lscn, r) => new { lscn, r = r.DefaultIfEmpty().Where(w => w.ServiceId == lscn.s.ServiceId).FirstOrDefault() })
                    //             .Select(x => new DO_DoctorServiceRate
                    //             {
                    //                 ServiceId = x.lscn.s.ServiceId,
                    //                 ServiceDesc = x.lscn.s.ServiceDesc,
                    //                 DoctorId = doctorId,
                    //                 DoctorDesc = db.GtEsdocds.Where(doc => doc.DoctorId == doctorId).FirstOrDefault().DoctorName,
                    //                 CurrencyCode = x.r != null ? x.r.CurrencyCode : "",
                    //                 Tariff = x.r != null ? x.r.Tariff : 0,
                    //                 EffectiveDate = x.r != null ? x.r.EffectiveDate : defaultDate,
                    //                 ActiveStatus = x.r != null ? x.r.ActiveStatus : true,
                    //                 RateType = x.r != null ? x.r.RateType : 0,

                    //             }
                    //    ).ToListAsync();
                    //return await result;

                    var defaultDate = DateTime.Now.Date;
                    var result =db.GtEsclsls.Where(x => x.BusinessKey==businessKey && x.ClinicId==clinicId && x.ConsultationId==consultationId && x.ActiveStatus)
                        .Join(db.GtEssrms,
                        l => l.ServiceId,
                        s => s.ServiceId,
                        (l, s) => new { l, s }).
                        Join(db.GtEspasms.Where(x => x.ParameterId == 5 && x.ActiveStatus),
                        ss=>ss.s.ServiceId,
                        sp=>sp.ServiceId,
                        (ss,sp)=>new {ss,sp})
                        .GroupJoin(db.GtEscdsts.Where(w => w.BusinessKey == businessKey && w.SpecialtyId == specialtyId && w.DoctorId == doctorId && w.CurrencyCode == currencycode && w.RateType == ratetype
                        && w.ClinicId==clinicId && w.ConsultationId==consultationId),
                        lscn => lscn.ss.l.ServiceId,
                        r => r.ServiceId,
                       (lscn, r) => new { lscn, r  })
                        .SelectMany(z => z.r.DefaultIfEmpty(),
                                 (a, b) => new DO_DoctorServiceRate
                                 {
                                     ServiceId = a.lscn.ss.l.ServiceId,
                                     ServiceDesc = a.lscn.ss.s.ServiceDesc,
                                     //DoctorId = doctorId,
                                     //DoctorDesc = db.GtEsdocds.Where(doc => doc.DoctorId == doctorId).FirstOrDefault().DoctorName,
                                     CurrencyCode = b != null ? b.CurrencyCode : "",
                                     Tariff = b != null ? b.Tariff : 0,
                                     EffectiveDate = b != null ? b.EffectiveDate : defaultDate,
                                     ActiveStatus = b != null ? b.ActiveStatus : true,
                                     RateType = b != null ? b.RateType : 0,

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

        public async Task<DO_ReturnParameter> InsertOrUpdateDoctorServiceRate(List<DO_DoctorServiceRate> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var d_servicerate in obj)
                        {
                            var ServiceExist = db.GtEscdsts.Where(w => w.ServiceId == d_servicerate.ServiceId && w.BusinessKey == d_servicerate.BusinessKey && w.DoctorId == d_servicerate.DoctorId && w.CurrencyCode == d_servicerate.CurrencyCode && w.RateType == d_servicerate.RateType
                           && w.ClinicId== d_servicerate.ClinicId && w.ConsultationId== d_servicerate.ConsultationId && w.SpecialtyId== d_servicerate.SpecialtyId && w.EffectiveTill == null).FirstOrDefault();
                            if (ServiceExist != null)
                            {
                                if (d_servicerate.EffectiveDate != ServiceExist.EffectiveDate)
                                {
                                    if (d_servicerate.EffectiveDate < ServiceExist.EffectiveDate)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W0155", Message = string.Format(_localizer[name: "W0155"]) };
                                    }
                                    ServiceExist.EffectiveTill = d_servicerate.EffectiveDate.AddDays(-1);
                                    ServiceExist.ModifiedBy = d_servicerate.UserID;
                                    ServiceExist.ModifiedOn = DateTime.Now;
                                    ServiceExist.ModifiedTerminal = d_servicerate.TerminalID;
                                    ServiceExist.ActiveStatus = false;

                                    var docrate = new GtEscdst
                                    {
                                        BusinessKey = d_servicerate.BusinessKey,
                                        ClinicId= d_servicerate.ClinicId,
                                        ConsultationId= d_servicerate.ConsultationId,
                                        ServiceId = d_servicerate.ServiceId,
                                        DoctorId = d_servicerate.DoctorId,
                                        SpecialtyId= d_servicerate.SpecialtyId,
                                        RateType = d_servicerate.RateType,
                                        CurrencyCode = d_servicerate.CurrencyCode,
                                        EffectiveDate = d_servicerate.EffectiveDate,
                                        Tariff = d_servicerate.Tariff,
                                        ActiveStatus = d_servicerate.ActiveStatus,
                                        FormId = d_servicerate.FormID,
                                        CreatedBy = d_servicerate.UserID,
                                        CreatedOn = DateTime.Now,
                                        CreatedTerminal = d_servicerate.TerminalID
                                    };
                                    db.GtEscdsts.Add(docrate);

                                }
                                else
                                {
                                    ServiceExist.Tariff = d_servicerate.Tariff;
                                    ServiceExist.ActiveStatus = d_servicerate.ActiveStatus;
                                    ServiceExist.ModifiedBy = d_servicerate.UserID;
                                    ServiceExist.ModifiedOn = DateTime.Now;
                                    ServiceExist.ModifiedTerminal = d_servicerate.TerminalID;
                                }

                            }
                            else
                            {
                                if (d_servicerate.Tariff != 0)
                                {
                                    var doc_rate = new GtEscdst
                                    {
                                        BusinessKey = d_servicerate.BusinessKey,
                                        ClinicId= d_servicerate.ClinicId,
                                        ConsultationId= d_servicerate.ConsultationId,
                                        ServiceId = d_servicerate.ServiceId,
                                        DoctorId = d_servicerate.DoctorId,
                                        SpecialtyId= d_servicerate.SpecialtyId,
                                        RateType = d_servicerate.RateType,
                                        CurrencyCode = d_servicerate.CurrencyCode,
                                        EffectiveDate = d_servicerate.EffectiveDate,
                                        Tariff = d_servicerate.Tariff,
                                        ActiveStatus = d_servicerate.ActiveStatus,
                                        FormId = d_servicerate.FormID,
                                        CreatedBy = d_servicerate.UserID,
                                        CreatedOn = DateTime.Now,
                                        CreatedTerminal = d_servicerate.TerminalID
                                    };
                                    db.GtEscdsts.Add(doc_rate);
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

        public async Task<List<DO_DoctorMaster>> GetDoctosbyBusinessKey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEsdobls
                        .Join(db.GtEsdocds,
                        l => l.DoctorId,
                        s => s.DoctorId,
                        (l, s) => new { l, s })
                        .Where(w => w.l.ActiveStatus && w.l.BusinessKey == businesskey && w.s.ActiveStatus)
                        .Select(x => new DO_DoctorMaster
                        {
                            DoctorId = x.l.DoctorId,
                            DoctorName = x.s.DoctorName
                        }).OrderBy(x => x.DoctorName).ToListAsync();
                    var Distinctspecialties = ds.GroupBy(x => new { x.DoctorId }).Select(g => g.First()).ToList();
                    return Distinctspecialties.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ApplicationCode>> GetClinicTypesbyBusinessKey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds =await db.GtEsopcls
                        .Join(db.GtEcapcds,
                        l => l.ClinicId,
                        a => a.ApplicationCode,
                        (l, a) => new { l, a })
                        .Where(w => w.l.ActiveStatus && w.l.BusinessKey==businesskey && w.a.ActiveStatus)
                        .Select(x => new DO_ApplicationCode
                        {
                            ApplicationCode = x.l.ClinicId,
                            CodeDesc = x.a.CodeDesc
                        }).OrderBy(x => x.CodeDesc).ToListAsync();
                    var Distinctclinics = ds.GroupBy(x => new { x.ApplicationCode}).Select(g => g.First()).ToList();
                    return  Distinctclinics.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ApplicationCode>> GetConsultationbyClinicID(int businesskey,int clinicId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEsopcls
                        .Join(db.GtEcapcds,
                        l => l.ConsultationId,
                        a => a.ApplicationCode,
                        (l, a) => new { l, a })
                        .Where(w => w.l.ActiveStatus && w.l.BusinessKey == businesskey && w.l.ClinicId==clinicId && w.a.ActiveStatus)
                        .Select(x => new DO_ApplicationCode
                        {
                            ApplicationCode = x.l.ConsultationId,
                            CodeDesc = x.a.CodeDesc
                        }).OrderBy(x => x.CodeDesc).ToListAsync();
                    var Distinctconsultations = ds.GroupBy(x => new { x.ApplicationCode }).Select(g => g.First()).ToList();
                    return Distinctconsultations.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_SpecialtyCodes>> GetSpecialtiesbyDoctorID(int businesskey, int doctorId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEsdosps
                        .Join(db.GtEsspcds,
                        l => l.SpecialtyId,
                        s => s.SpecialtyId,
                        (l, s) => new { l, s })
                        .Where(w => w.l.ActiveStatus && w.l.BusinessKey == businesskey && w.l.DoctorId == doctorId && w.s.ActiveStatus)
                        .Select(x => new DO_SpecialtyCodes
                        {
                            SpecialtyID = x.l.SpecialtyId,
                            SpecialtyDesc = x.s.SpecialtyDesc
                        }).OrderBy(x=>x.SpecialtyDesc).ToListAsync();
                    var Distinctspecialties = ds.GroupBy(x => new { x.SpecialtyID }).Select(g => g.First()).ToList();
                    return Distinctspecialties.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Specialty Service Rate

        public async Task<List<DO_SpecialityServiceRate>> GetSpecialtyServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey,int clinicId,int consultationId, int specialtyId, string currencycode, int ratetype)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    //var defaultDate = DateTime.Now.Date;
                    //var result = db.GtEspasms.Where(x => x.ParameterId == 5 && x.ActiveStatus)
                    //    .Join(db.GtEssrms,
                    //    l => l.ServiceId,
                    //    s => s.ServiceId,
                    //    (l, s) => new { l, s })
                    //    .GroupJoin(db.GtEscssts.Where(w => w.BusinessKey == businessKey && w.ServiceId == serviceId && w.SpecialtyId == specialtyId && w.CurrencyCode == currencycode && w.RateType == ratetype),
                    //    lscn => lscn.s.ServiceId,
                    //    r => r.ServiceId,
                    //    (lscn, r) => new { lscn, r = r.DefaultIfEmpty().Where(w => w.ServiceId == lscn.s.ServiceId).FirstOrDefault() })
                    //             .Select(x => new DO_SpecialityServiceRate
                    //             {
                    //                 ServiceId = x.lscn.s.ServiceId,
                    //                 ServiceDesc = x.lscn.s.ServiceDesc,
                    //                 SpecialtyId = specialtyId,
                    //                 SpecialtyDesc = db.GtEsspcds.Where(spe => spe.SpecialtyId == specialtyId).FirstOrDefault().SpecialtyDesc,
                    //                 CurrencyCode = x.r != null ? x.r.CurrencyCode : "",
                    //                 Tariff = x.r != null ? x.r.Tariff : 0,
                    //                 EffectiveDate = x.r != null ? x.r.EffectiveDate : defaultDate,
                    //                 ActiveStatus = x.r != null ? x.r.ActiveStatus : true,
                    //                 RateType = x.r != null ? x.r.RateType : 0,


                    //             }
                    //    ).ToListAsync();
                    //return await result;


                    var defaultDate = DateTime.Now.Date;
                    var result = db.GtEspasms.Where(x => x.ParameterId == 5 && x.ActiveStatus)
                        .Join(db.GtEssrms,
                        l => l.ServiceId,
                        s => s.ServiceId,
                        (l, s) => new { l, s })
                        .GroupJoin(db.GtEscssts.Where(w => w.BusinessKey == businessKey  && w.SpecialtyId == specialtyId && w.CurrencyCode == currencycode && w.RateType == ratetype && w.ClinicId==clinicId && w.ConsultationId==consultationId),
                        lscn => lscn.s.ServiceId,
                        r => r.ServiceId,
                        (lscn, r) => new { lscn, r })
                        .SelectMany(z => z.r.DefaultIfEmpty(),
                                 (a, b) => new DO_SpecialityServiceRate
                                 {
                                     ServiceId = a.lscn.s.ServiceId,
                                     ServiceDesc = a.lscn.s.ServiceDesc,
                                     SpecialtyId = specialtyId,
                                     //SpecialtyDesc = db.GtEsspcds.Where(spe => spe.SpecialtyId == specialtyId).FirstOrDefault().SpecialtyDesc,
                                     CurrencyCode = b != null ? b.CurrencyCode : "",
                                     Tariff = b != null ? b.Tariff : 0,
                                     EffectiveDate = b != null ? b.EffectiveDate : defaultDate,
                                     ActiveStatus = b != null ? b.ActiveStatus : true,
                                     RateType = b != null ? b.RateType : 0,


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

        public async Task<DO_ReturnParameter> InsertOrUpdateSpecialityServiceRate(List<DO_SpecialityServiceRate> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var s_servicerate in obj)
                        {
                            var ServiceExist = db.GtEscssts.Where(w => w.ServiceId == s_servicerate.ServiceId && w.BusinessKey == s_servicerate.BusinessKey && w.SpecialtyId == s_servicerate.SpecialtyId && w.CurrencyCode == s_servicerate.CurrencyCode && w.RateType == s_servicerate.RateType
                           && w.ClinicId==s_servicerate.ClinicId && w.ConsultationId==s_servicerate.ConsultationId && w.EffectiveTill == null).FirstOrDefault();
                            if (ServiceExist != null)
                            {
                                if (s_servicerate.EffectiveDate != ServiceExist.EffectiveDate)
                                {
                                    if (s_servicerate.EffectiveDate < ServiceExist.EffectiveDate)
                                    {
                                        return new DO_ReturnParameter() { Status = false, StatusCode = "W0155", Message = string.Format(_localizer[name: "W0155"]) };
                                    }
                                    ServiceExist.EffectiveTill = s_servicerate.EffectiveDate.AddDays(-1);
                                    ServiceExist.ModifiedBy = s_servicerate.UserID;
                                    ServiceExist.ModifiedOn = DateTime.Now;
                                    ServiceExist.ModifiedTerminal = s_servicerate.TerminalID;
                                    ServiceExist.ActiveStatus = false;

                                    var docrate = new GtEscsst
                                    {
                                        BusinessKey = s_servicerate.BusinessKey,
                                        ClinicId=s_servicerate.ClinicId,
                                        ConsultationId=s_servicerate.ConsultationId,
                                        ServiceId = s_servicerate.ServiceId,
                                        SpecialtyId = s_servicerate.SpecialtyId,
                                        RateType = s_servicerate.RateType,
                                        CurrencyCode = s_servicerate.CurrencyCode,
                                        EffectiveDate = s_servicerate.EffectiveDate,
                                        Tariff = s_servicerate.Tariff,
                                        ActiveStatus = s_servicerate.ActiveStatus,
                                        FormId = s_servicerate.FormID,
                                        CreatedBy = s_servicerate.UserID,
                                        CreatedOn = DateTime.Now,
                                        CreatedTerminal = s_servicerate.TerminalID
                                    };
                                    db.GtEscssts.Add(docrate);


                                }
                                else
                                {
                                    ServiceExist.Tariff = s_servicerate.Tariff;
                                    ServiceExist.ActiveStatus = s_servicerate.ActiveStatus;
                                    ServiceExist.ModifiedBy = s_servicerate.UserID;
                                    ServiceExist.ModifiedOn = DateTime.Now;
                                    ServiceExist.ModifiedTerminal = s_servicerate.TerminalID;
                                }

                            }
                            else
                            {
                                if (s_servicerate.Tariff != 0)
                                {
                                    var spe_rate = new GtEscsst
                                    {
                                        BusinessKey = s_servicerate.BusinessKey,
                                        ClinicId = s_servicerate.ClinicId,
                                        ConsultationId = s_servicerate.ConsultationId,
                                        ServiceId = s_servicerate.ServiceId,
                                        SpecialtyId = s_servicerate.SpecialtyId,
                                        RateType = s_servicerate.RateType,
                                        CurrencyCode = s_servicerate.CurrencyCode,
                                        EffectiveDate = s_servicerate.EffectiveDate,
                                        Tariff = s_servicerate.Tariff,
                                        ActiveStatus = s_servicerate.ActiveStatus,
                                        FormId = s_servicerate.FormID,
                                        CreatedBy = s_servicerate.UserID,
                                        CreatedOn = DateTime.Now,
                                        CreatedTerminal = s_servicerate.TerminalID
                                    };
                                    db.GtEscssts.Add(spe_rate);
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

        public async Task<List<DO_SpecialtyCodes>> GetSpecialitesbyBusinessKey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEsspbls
                        .Join(db.GtEsspcds,
                        l => l.SpecialtyId,
                        s => s.SpecialtyId,
                        (l, s) => new { l, s })
                        .Where(w => w.l.ActiveStatus && w.l.BusinessKey == businesskey && w.s.ActiveStatus)
                        .Select(x => new DO_SpecialtyCodes
                        {
                            SpecialtyID = x.l.SpecialtyId,
                            SpecialtyDesc = x.s.SpecialtyDesc
                        }).OrderBy(x => x.SpecialtyDesc).ToListAsync();
                    var Distinctspecialties = ds.GroupBy(x => new { x.SpecialtyID }).Select(g => g.First()).ToList();
                    return Distinctspecialties.ToList();

                   
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
