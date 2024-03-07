using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnimeCards.Controllers
{
    [Route("api/Cards/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
