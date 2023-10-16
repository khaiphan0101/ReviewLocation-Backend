using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.ReqModels;
using review.Services;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Authorize(Roles = "Admin")]//những route chỉ cho admin xài thì khai báo ntn 
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] CategoryReqModel req)
        {
            var response = await _categoryService.Add(req);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromForm] CategoryReqModel req, string id)
        {
            var response = await _categoryService.Update(req, id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _categoryService.Delete(id);
            return Ok(response);
        }

        //route này danh cho tất cả nguoiqf dùng xài
        [HttpGet("getSelect/{id}")]
        public async Task<IActionResult> GetSelect(string id)
        {
            var response = await _categoryService.GetSelect(id);
            return Ok(response);
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAll();
            return Ok(response);
        }
    }
}
