using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace DuckCity.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SshController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> TestSsh(string containerId)
    {
        string strCmdText = "-c \"sshpass -p 'Iamroot!01' ssh localadm@adm.canardecarlate.fr -o StrictHostKeyChecking=no cd /opt/Projet/ansible; ./run_container.sh " + containerId + "\"";

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