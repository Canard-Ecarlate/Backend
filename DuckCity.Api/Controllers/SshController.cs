using DuckCity.Api.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DuckCity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SshController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> TestSsh()
        {
            const string strCmdText = "-c \"sshpass -p Iamroot!01 ssh localadm@54.36.80.250 -o StrictHostKeyChecking=no ls\"";

            ProcessStartInfo procStartInfo = new("/bin/bash", strCmdText)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process proc = new();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd();

            return new OkObjectResult("ok\n" + result);
        }
            
        [HttpGet]
        public ActionResult<string> Bonjour()
        {
            return new OkObjectResult("Bonjour");
        }
    }
}
