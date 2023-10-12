using eSya.ManageRates.DO;
using eSya.ManageRates.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ManageRates.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClinicServicesController : ControllerBase
    {
        private readonly IClinicServiceRepository _ClinicRepository;

        public ClinicServicesController(IClinicServiceRepository clinicRepository)
        {
            _ClinicRepository = clinicRepository;
        }

        #region ClinicVisitRate
        [HttpGet]
        public async Task<IActionResult> GetServicesPerformedByDoctor()
        {
            var ac = await _ClinicRepository.GetServicesPerformedByDoctor();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetClinicVisitRateByBKeyClinicTypeCurrCodeRateType(int businessKey, int clinictype, string currencycode, int ratetype)
        {
            var ac = await _ClinicRepository.GetClinicVisitRateByBKeyClinicTypeCurrCodeRateType(businessKey, clinictype, currencycode, ratetype);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateClinicVisitRate(List<DO_ClinicVisitRate> obj)
        {
            var msg = await _ClinicRepository.AddOrUpdateClinicVisitRate(obj);
            return Ok(msg);
        }
        #endregion
    }
}
