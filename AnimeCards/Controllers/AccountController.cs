using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Response_Wrapper;

namespace AnimeCards.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly AppDbContext _dbContext;
        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var totalData = await _dbContext.Animes.ToListAsync();
            if (totalData.Count == 0)
            {
                return BadRequest("Data Not found");
            }

            //return Ok(SuccessResponseWrapper.SuccessApi(totalData));
            return BadRequest(ErrorResponseWrapper.ErrorApi("Failed Message"));
        }
    }
}
