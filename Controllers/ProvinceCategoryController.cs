using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceCategoryController : ControllerBase
    {
        private readonly IProvinceCategoryService _provinceCategoryService;

        public ProvinceCategoryController(IProvinceCategoryService provinceCategoryService )
        {
            _provinceCategoryService = provinceCategoryService;
        }
        [Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] ProvinceCategoryReqModel req)
        {
            var response = await _provinceCategoryService.Add(req);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] ProvinceCategoryReqModel req, string id)
        {
            var response = await _provinceCategoryService.Update(req, id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _provinceCategoryService.Delete(id);
            return Ok(response);
        }

        //route này danh cho tất cả nguoiqf dùng xài
        [HttpGet("getSelect/{id}")]
        public async Task<IActionResult> GetSelect(string id)
        {
            var response = await _provinceCategoryService.GetSelect(id);
            return Ok(response);
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _provinceCategoryService.GetAll();
            return Ok(response);
        }
        [HttpGet("getByProVince/{id}")]
        public async Task<IActionResult> GetByProvince(string id)
        {
            var response = await _provinceCategoryService.GetByProvince(id);
            return Ok(response);
        }
    }
}
