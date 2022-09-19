using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NLayer.API.Filters
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateFilterAttribute : ControllerBase
    {
    }
}
