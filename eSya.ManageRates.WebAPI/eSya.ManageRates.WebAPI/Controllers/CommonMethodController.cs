using eSya.ManageRates.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ManageRates.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonMethodController : ControllerBase
    {
        private readonly ICommonMethodRepository _commonMethodRepository;
        public CommonMethodController(ICommonMethodRepository commonMethodRepository)
        {
            _commonMethodRepository = commonMethodRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ac = await _commonMethodRepository.GetBusinessKey();
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codetype)
        {
            var ac = await _commonMethodRepository.GetApplicationCodesByCodeType(codetype);
            return Ok(ac);
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrencyCodes()
        {
            var ac = await _commonMethodRepository.GetCurrencyCodes();
            return Ok(ac);
        }
    }
}
