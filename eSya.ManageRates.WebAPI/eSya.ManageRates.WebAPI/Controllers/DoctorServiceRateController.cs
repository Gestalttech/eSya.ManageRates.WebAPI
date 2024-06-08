using eSya.ManageRates.DO;
using eSya.ManageRates.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ManageRates.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DoctorServiceRateController : ControllerBase
    {
        private readonly IDoctorServiceRateRepository _DoctorServiceRateRepository;

        public DoctorServiceRateController(IDoctorServiceRateRepository DoctorServiceRateRepository)
        {
            _DoctorServiceRateRepository = DoctorServiceRateRepository;
        }

        #region Doctor Service Rate
        /// <summary>
        /// Get Doctor Service Rate by Business Key Service Id Curr Code Rate Type and Doctor Id
        /// UI Reffered - Doctor Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDoctorServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int clinicId, int consultationId, int specialtyId, int doctorId, string currencycode, int ratetype)
        {
            var msg = await _DoctorServiceRateRepository.GetDoctorServiceRateByBKeyServiceIdCurrCodeRateType(businessKey, clinicId, consultationId, specialtyId, doctorId, currencycode, ratetype);
            return Ok(msg);
        }

        /// <summary>
        /// Insert Or Update Doctor Service Rate
        /// UI Reffered - Doctor Service Rate,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateDoctorServiceRate(List<DO_DoctorServiceRate> obj)
        {
            var msg = await _DoctorServiceRateRepository.InsertOrUpdateDoctorServiceRate(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get Doctors for dropdown
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDoctosbyBusinessKey(int businesskey)
        {
            var msg = await _DoctorServiceRateRepository.GetDoctosbyBusinessKey(businesskey);
            return Ok(msg);
        }
        /// <summary>
        /// Get Clinic Types for dropdown
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetClinicTypesbyBusinessKey(int businesskey)
        {
            var msg = await _DoctorServiceRateRepository.GetClinicTypesbyBusinessKey(businesskey);
            return Ok(msg);
        }
        /// <summary>
        /// Get Consultation for dropdown
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetConsultationbyClinicID(int businesskey, int clinicId)
        {
            var msg = await _DoctorServiceRateRepository.GetConsultationbyClinicID(businesskey, clinicId);
            return Ok(msg);
        }
        /// <summary>
        /// Get Specialties for dropdown
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtiesbyDoctorID(int businesskey, int doctorId)
        {
            var msg = await _DoctorServiceRateRepository.GetSpecialtiesbyDoctorID(businesskey, doctorId);
            return Ok(msg);
        }
        #endregion

        #region Specialty Service Rate
        /// <summary>
        /// Get Specialty Service Rate by Business Key Service Id Curr Code Rate Type and Doctor Id
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int clinicId, int consultationId, int specialtyId, string currencycode, int ratetype)
        {
            var msg = await _DoctorServiceRateRepository.GetSpecialtyServiceRateByBKeyServiceIdCurrCodeRateType(businessKey, clinicId, consultationId, specialtyId, currencycode, ratetype);
            return Ok(msg);
        }

        /// <summary>
        /// Insert Or Update Specialty Service Rate
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateSpecialityServiceRate(List<DO_SpecialityServiceRate> obj)
        {
            var msg = await _DoctorServiceRateRepository.InsertOrUpdateSpecialityServiceRate(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Get Specialites for drop down
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialitesbyBusinessKey(int businesskey)
        {
            var msg = await _DoctorServiceRateRepository.GetSpecialitesbyBusinessKey(businesskey);
            return Ok(msg);
        }
        #endregion
    }
}
