using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Teram.IT.Module.BackupManagement.Controllers
{


    public class BackupController : Controller
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(
        string lpszUsername,
        string lpszDomain,
        string lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        out IntPtr phToken);


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessFolder()
        {
            string folderPath = @"\\192.168.99.210\BK\";
            string domain = "TERAMCHAP";
            string username = "backup";
            string password = "B@h#$232)";

            IntPtr userToken = IntPtr.Zero;

            try
            {            
                bool success = LogonUser(username, domain, password, 9, 3, out userToken);
                if (!success)
                {
                    throw new InvalidOperationException($"Failed to log in as {username}");
                }

#pragma warning disable CA1416

                using WindowsIdentity newId = new WindowsIdentity(userToken);

                WindowsIdentity.RunImpersonated(newId.AccessToken, () =>
                {
                    try
                    {
                        var files = Directory.GetDirectories(folderPath);

                        foreach (var file in files)
                        {

                        }

                        return Ok("Folder accessed successfully.");
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        return StatusCode(403, $"Unauthorized access to the folder: {ex.Message}");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        return StatusCode(404, $"Folder not found: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Error accessing folder: {ex.Message}");
                    }
                });
#pragma warning restore CA1416
            }
            finally
            {               
                if (userToken != IntPtr.Zero)
                {
                    CloseHandle(userToken);
                }
            }
            return Ok();
        }
    }
}
