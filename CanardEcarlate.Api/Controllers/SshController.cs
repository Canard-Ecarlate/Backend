using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CanardEcarlate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SshController : ControllerBase
    {

        [HttpGet]
        public ActionResult<string> TestSsh()
        {
            string strCmdText= "/C ssh localadm@87.98.135.203 'ls'";

            Process.Start("CMD.exe",strCmdText);

            return new OkObjectResult("ok");
        }
    }
}