using eSya.ManageRates.DO;
using eSya.ManageRates.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ManageRates.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceRatesController : ControllerBase
    {
        private readonly IServiceRatesRepository _serviceRatesRepository;

        public ServiceRatesController(IServiceRatesRepository serviceRatesRepository)
        {
            _serviceRatesRepository = serviceRatesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceBaseRateByBKeyRateTypeCurrCode(int businessKey, int ratetype, string currencycode)
        {
            var ac = await _serviceRatesRepository.GetServiceBaseRateByBKeyRateTypeCurrCode(businessKey, ratetype, currencycode);
            return Ok(ac);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateServiceBaseRate(List<DO_ServiceBaseRate> obj)
        {
            var msg = await _serviceRatesRepository.AddOrUpdateServiceBaseRate(obj);
            return Ok(msg);
        }
        [HttpPost]
        public async Task<IActionResult> AddServiceBaseRate(List<DO_ServiceBaseRate> obj)
        {
            var msg = await _serviceRatesRepository.AddServiceBaseRate(obj);
            return Ok(msg);
        }
    }
}
