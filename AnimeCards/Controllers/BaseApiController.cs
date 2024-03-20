using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeCards.Controllers
{
    [Authorize]
    [Route("api/Cards/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
