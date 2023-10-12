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
        public async Task<IActionResult> GetDoctorServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int serviceId, int doctorId, string currencycode, int ratetype)
        {
            var msg = await _DoctorServiceRateRepository.GetDoctorServiceRateByBKeyServiceIdCurrCodeRateType(businessKey, serviceId, doctorId, currencycode, ratetype);
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
        public async Task<IActionResult> GetActiveDoctos()
        {
            var msg = await _DoctorServiceRateRepository.GetActiveDoctos();
            return Ok(msg);
        }

        #endregion

        #region Specialty Service Rate
        /// <summary>
        /// Get Specialty Service Rate by Business Key Service Id Curr Code Rate Type and Doctor Id
        /// UI Reffered - Specialty Service Rate,
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyServiceRateByBKeyServiceIdCurrCodeRateType(int businessKey, int serviceId, int specialtyId, string currencycode, int ratetype)
        {
            var msg = await _DoctorServiceRateRepository.GetSpecialtyServiceRateByBKeyServiceIdCurrCodeRateType(businessKey, serviceId, specialtyId, currencycode, ratetype);
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
        public async Task<IActionResult> GetActiveSpecialites()
        {
            var msg = await _DoctorServiceRateRepository.GetActiveSpecialites();
            return Ok(msg);
        }
        #endregion
    }
}
