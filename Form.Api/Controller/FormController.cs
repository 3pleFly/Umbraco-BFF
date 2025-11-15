using Microsoft.AspNetCore.Mvc;

namespace Form.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController(UmbracoService umbraco) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var content = await umbraco.GetContentAndChildren(id);
            return Ok(content);
        }
    }
}
