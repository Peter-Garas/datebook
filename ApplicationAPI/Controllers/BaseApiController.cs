using ApplicationAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
                
    }
}