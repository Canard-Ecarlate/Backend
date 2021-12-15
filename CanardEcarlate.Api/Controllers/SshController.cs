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
            string strCmdText= "-c \"sshpass -p Iamroot!01 ssh localadm@54.36.80.250 ls\"";

            //Process.Start("/bin/bash",strCmdText);

            ProcessStartInfo procStartInfo = new ProcessStartInfo("/bin/bash", strCmdText);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            string result = proc.StandardOutput.ReadToEnd();

            return new OkObjectResult("ok" + result);
        }
    }
}