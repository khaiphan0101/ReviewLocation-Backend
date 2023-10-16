using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;

        public ProvinceController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }

        //B1: tạo entity
        //B2: khai báo các hàm tương ứng trong service để thêm xóa sửa và lấy data province
        //B3: tài khoản admin: dùng tk này login để lấy token mà test 
        //       + adminweb
        //       + 12345678
        [Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] ProvinceReqModel req)
        {
            var response = await _provinceService.Add(req);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] ProvinceReqModel req, string id)
        {
            var response = await _provinceService.Update(req, id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _provinceService.Delete(id);
            return Ok(response);
        }

        //route này danh cho tất cả nguoiqf dùng xài
        [HttpGet("getSelect/{id}")]
        public async Task<IActionResult> GetSelect(string id)
        {
            var response = await _provinceService.GetSelect(id);
            return Ok(response);
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _provinceService.GetAll();
            return Ok(response);
        }
    }
}
