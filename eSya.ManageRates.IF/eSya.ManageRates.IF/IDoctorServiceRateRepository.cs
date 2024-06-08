using eSya.ManageRates.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ManageRates.IF
{
    public interface IDoctorServiceRateRepository
    {
        #region Doctor Service Rate
        Task<List<DO_DoctorServiceRate>> GetDoctorServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int clinicId, int consultationId, int specialtyId, int doctorId, string currencycode, int ratetype);
        Task<DO_ReturnParameter> InsertOrUpdateDoctorServiceRate(List<DO_DoctorServiceRate> obj);
        Task<List<DO_DoctorMaster>> GetDoctosbyBusinessKey(int businesskey);
        Task<List<DO_ApplicationCode>> GetClinicTypesbyBusinessKey(int businesskey);
        Task<List<DO_ApplicationCode>> GetConsultationbyClinicID(int businesskey, int clinicId);
        Task<List<DO_SpecialtyCodes>> GetSpecialtiesbyDoctorID(int businesskey, int doctorId);
        #endregion

        #region Specialty Service Rate
        Task<List<DO_SpecialityServiceRate>> GetSpecialtyServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int clinicId, int consultationId, int specialtyId, string currencycode, int ratetype);
        Task<DO_ReturnParameter> InsertOrUpdateSpecialityServiceRate(List<DO_SpecialityServiceRate> obj);
        Task<List<DO_SpecialtyCodes>> GetSpecialitesbyBusinessKey(int businesskey);
        #endregion
    }
}
