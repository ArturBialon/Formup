using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
