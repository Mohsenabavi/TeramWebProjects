using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity.UI.Services;
using Renci.SshNet;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Models;
using Teram.Module.EmailSender.Services.Interfaces;
using Teram.ServiceContracts;

namespace Teram.IT.Module.BackupManagement.Services
{
    public class IOService : IIOService
    {
        private readonly IConfiguration configuration;

        public IOService(IConfiguration configuration)
        {
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
        }




        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        public BusinessOperationResult<BackupResultModel> CopyFiles(string applicationTitle, string sourcePath, string destinationPath, string? fileName = null)
        {

            var backupSettings = configuration.GetSection("BackupSettings");
            var domain = backupSettings.GetValue<string>("Domain");
            var userName = backupSettings.GetValue<string>("Username");
            var Password = backupSettings.GetValue<string>("Password");

            IntPtr userToken = IntPtr.Zero;

            var result = new BusinessOperationResult<BackupResultModel>();
            result.ResultEntity=new BackupResultModel();

            try
            {
                bool success = LogonUser(userName, domain, Password, 9, 3, out userToken);
                if (!success)
                {
                    result.SetErrorMessage($"Failed to log in as {userName}");
                    return result;
                }

#pragma warning disable CA1416

                using WindowsIdentity newId = new(userToken);

                WindowsIdentity.RunImpersonated(newId.AccessToken, () =>
                {
                    try
                    {
                        var directory = new DirectoryInfo(sourcePath);
                        if (directory.Exists)
                        {
                            var myFileList = directory.GetFiles();

                            if (myFileList.Any())
                            {
                                var myFile = (string.IsNullOrEmpty(fileName)) ? myFileList.OrderByDescending(f => f.LastWriteTime).First() : myFileList.Where(x => x.Name.Contains(fileName)).OrderByDescending(f => f.LastWriteTime).First();

                                string newZipFileName = myFile.Name.Replace(".bak", "") +"-" +  DateTime.Now.ToPersianDate().Replace("/", "-")+ ".zip";
                                string destinationZipFilePath = System.IO.Path.Combine(destinationPath, newZipFileName);
                                using (var zipFileStream = new FileStream(destinationZipFilePath, FileMode.Create))
                                {
                                    using (var zip = new ZipArchive(zipFileStream, ZipArchiveMode.Create, true))
                                    {
                                        var entry = zip.CreateEntry(myFile.Name);
                                        using (var entryStream = entry.Open())
                                        using (var fileStream = new FileStream(myFile.FullName, FileMode.Open, FileAccess.Read))
                                        {
                                            fileStream.CopyTo(entryStream);
                                        }
                                    }
                                }
                                var emailContext = SetEmailContext(applicationTitle, sourcePath, destinationZipFilePath, true);
                                var backupResult = new BackupResultModel { IsSuccess=true, EmailContext= emailContext };
                                result.SetSuccessResult(backupResult);
                            }
                        }
                        else
                        {
                            var emailContext = SetEmailContext(applicationTitle, sourcePath, "-", false, $"{directory} Path Not Found");
                            var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                            result.ResultEntity= backupResult;
                            result.SetErrorMessage(emailContext);
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        var emailContext = SetEmailContext(applicationTitle, sourcePath, "-", false, ex.Message + ex.InnerException);
                        var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        var emailContext = SetEmailContext(applicationTitle, sourcePath, "-", false, ex.Message + ex.InnerException);
                        var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }
                    catch (Exception ex)
                    {
                        var emailContext = SetEmailContext(applicationTitle, sourcePath, "-", false, ex.Message + ex.InnerException);
                        var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }
                });
                return result;
#pragma warning restore CA1416
            }
            catch (Exception ex)
            {
                var emailContext = SetEmailContext(applicationTitle, sourcePath, "-", false);
                var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                result.ResultEntity= backupResult;
                result.SetErrorMessage(emailContext);
                return result;
            }
        }

        public BusinessOperationResult<BackupResultModel> MizitoBackup()
        {
            string internal_Dest_Path = @"\\192.168.99.210\BK\";
            string sourceFile = "/home/mizito/backup/db/mizitoDB.zip";
            var result = new BusinessOperationResult<BackupResultModel>();
            result.ResultEntity=new BackupResultModel();
            string emailContext = string.Empty;
            var backupResult = new BackupResultModel();

            try
            {
                SftpClient sftpclient = new SftpClient("192.168.1.17", "mizito", "M!z0@&#");
                sftpclient.Connect();
                var memoryStream = new MemoryStream();

                var filename = "mizitoDB" + DateTime.Now.ToShortDateString().Replace("/", "");
                using (Stream fileStream = File.Create(internal_Dest_Path + @"\Mizito\" + filename + ".zip"))
                {
                    sftpclient.DownloadFile(sourceFile, fileStream);
                }
                emailContext = SetEmailContext("Mizito", sourceFile, internal_Dest_Path, true);
                backupResult = new BackupResultModel { IsSuccess=true, EmailContext= emailContext };
                result.ResultEntity= backupResult;
                result.SetErrorMessage(emailContext);
                return result;
            }
            catch (Exception ex)
            {
                emailContext = SetEmailContext("Mizito", sourceFile, internal_Dest_Path, false, $"{ex.InnerException + "-" + ex.Message}");
                backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                result.ResultEntity= backupResult;
                result.SetErrorMessage(emailContext);
                return result;
            }
        }

        public BusinessOperationResult<BackupResultModel> Esko_Backup()
        {
            var backupSettings = configuration.GetSection("BackupSettings");
            var domain = backupSettings.GetValue<string>("Domain");
            var userName = backupSettings.GetValue<string>("Username");
            var Password = backupSettings.GetValue<string>("Password");
            IntPtr userToken = IntPtr.Zero;
            string Esko_Source_Root = @"\\192.168.1.13\AEBackup\";
            string internal_Dest_Path = @"\\192.168.99.210\BK\";
            var Esko_Internal_Dest_Root = internal_Dest_Path + @"Esko\";
            var result = new BusinessOperationResult<BackupResultModel>();
            result.ResultEntity=new BackupResultModel();

            try
            {
                bool success = LogonUser(userName, domain, Password, 9, 3, out userToken);
                if (!success)
                {
                    result.SetErrorMessage($"Failed to log in as {userName}");
                    return result;
                }

                DateTime lastHigh = DateTime.MinValue;
                string lastDirectory = String.Empty;

#pragma warning disable CA1416

                using WindowsIdentity newId = new(userToken);

                WindowsIdentity.RunImpersonated(newId.AccessToken, () =>
                {
                    foreach (string subdir in Directory.GetDirectories(Esko_Source_Root))
                    {
                        DirectoryInfo fi1 = new DirectoryInfo(subdir);
                        DateTime created = fi1.LastWriteTime;

                        if (created > lastHigh)
                        {
                            lastDirectory = subdir;
                            lastHigh = created;
                        }
                    }

                    string emailContext = string.Empty;
                    var backupResult = new BackupResultModel();

                    if (string.IsNullOrWhiteSpace(lastDirectory))
                    {
                        emailContext = SetEmailContext("Esko", Esko_Source_Root, Esko_Internal_Dest_Root, false, $"{Esko_Internal_Dest_Root} Is Null Or Emplty");
                        backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }

                    else
                    {
                        Directory.CreateDirectory(Esko_Internal_Dest_Root + System.IO.Path.GetFileName(lastDirectory));
                        Esko_Internal_Dest_Root = System.IO.Path.Combine(Esko_Internal_Dest_Root, System.IO.Path.GetFileName(lastDirectory));

                        foreach (string dirPath in Directory.GetDirectories(lastDirectory, "*", SearchOption.AllDirectories))
                        {
                            var dir = dirPath.Replace(lastDirectory, Esko_Internal_Dest_Root);
                            Directory.CreateDirectory(dir);
                        }

                        foreach (string newPath in Directory.GetFiles(lastDirectory, "*.*", SearchOption.AllDirectories))
                    {
                            try
                            {
                                File.Copy(newPath, newPath.Replace(lastDirectory, Esko_Internal_Dest_Root), true);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                        
                        }
                        

                        emailContext = SetEmailContext("Esko", Esko_Source_Root, Esko_Internal_Dest_Root, true);
                        backupResult = new BackupResultModel { IsSuccess=true, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }
                });

                return result;
            }
#pragma warning restore CA1416
            catch (Exception ex)
            {
                var emailContext = SetEmailContext("Esko", Esko_Source_Root, Esko_Internal_Dest_Root, false, $"{ex.InnerException + "-" + ex.Message}");
                var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                result.ResultEntity= backupResult;
                result.SetErrorMessage(emailContext);
                return result;
            }
        }

        public BusinessOperationResult<BackupResultModel> EdariWeb_Backup()
        {

            var backupSettings = configuration.GetSection("BackupSettings");
            var domain = backupSettings.GetValue<string>("Domain");
            var userName = backupSettings.GetValue<string>("Username");
            var Password = backupSettings.GetValue<string>("Password");
            IntPtr userToken = IntPtr.Zero;
            string EdariWeb_Source_Root = @"\\192.168.1.3\BACKUPSET\";
            string internal_Dest_Path = @"\\192.168.99.210\BK\";
            string EdariWeb_Internal_Dest_Root = internal_Dest_Path + @"EdariWeb\";
            var result = new BusinessOperationResult<BackupResultModel>();
            result.ResultEntity=new BackupResultModel();

            DateTime lastHigh = DateTime.MinValue;
            string lastDirectory = String.Empty;

            try
            {
#pragma warning disable CA1416

                bool success = LogonUser(userName, domain, Password, 9, 3, out userToken);
                if (!success)
                {
                    result.SetErrorMessage($"Failed to log in as {userName}");
                    return result;
                }

                using WindowsIdentity newId = new(userToken);

                WindowsIdentity.RunImpersonated(newId.AccessToken, () =>
                {

                    foreach (string subdir in Directory.GetDirectories(EdariWeb_Source_Root))
                    {
                        DirectoryInfo fi1 = new DirectoryInfo(subdir);
                        DateTime created = fi1.LastWriteTime;

                        if (created > lastHigh)
                        {
                            lastDirectory = subdir;
                            lastHigh = created;
                        }
                    }

                    string emailContext = string.Empty;
                    var backupResult = new BackupResultModel();

                    if (string.IsNullOrWhiteSpace(lastDirectory))
                    {
                        emailContext = SetEmailContext("Edari_Web", EdariWeb_Source_Root, EdariWeb_Internal_Dest_Root, false, $"{EdariWeb_Internal_Dest_Root} Is Null Or Emplty");
                        backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }

                    else
                    {
                        Directory.CreateDirectory(EdariWeb_Internal_Dest_Root + System.IO.Path.GetFileName(lastDirectory));
                        EdariWeb_Internal_Dest_Root = System.IO.Path.Combine(EdariWeb_Internal_Dest_Root, System.IO.Path.GetFileName(lastDirectory));

                        foreach (string dirPath in Directory.GetDirectories(lastDirectory, "*", SearchOption.AllDirectories))
                        {
                            var dir = dirPath.Replace(lastDirectory, EdariWeb_Internal_Dest_Root);
                            Directory.CreateDirectory(dir);
                        }

                        foreach (string newPath in Directory.GetFiles(lastDirectory, "*.*", SearchOption.AllDirectories))
                        {

                            try
                            {
                                File.Copy(newPath, newPath.Replace(lastDirectory, EdariWeb_Internal_Dest_Root), true);
                            }
                            catch (Exception ex)
                            {
                                emailContext = SetEmailContext("Edari_Web", EdariWeb_Source_Root, EdariWeb_Internal_Dest_Root, false, ex.InnerException + ex.Message);
                                backupResult = new BackupResultModel { IsSuccess=true, EmailContext= emailContext };
                                result.ResultEntity= backupResult;
                                result.SetErrorMessage(emailContext);                                
                            }
                        }

                        emailContext = SetEmailContext("Edari_Web", EdariWeb_Source_Root, EdariWeb_Internal_Dest_Root, true);
                        backupResult = new BackupResultModel { IsSuccess=true, EmailContext= emailContext };
                        result.ResultEntity= backupResult;
                        result.SetErrorMessage(emailContext);
                    }
                });
                return result;
            }
#pragma warning restore CA1416
            catch (Exception ex)
            {
                var emailContext = SetEmailContext("Esko", EdariWeb_Source_Root, EdariWeb_Internal_Dest_Root, false, $"{ex.InnerException + "-" + ex.Message}");
                var backupResult = new BackupResultModel { IsSuccess=false, EmailContext= emailContext };
                result.ResultEntity= backupResult;
                result.SetErrorMessage(emailContext);
                return result;
            }
        }

        private string SetEmailContext(string applicationTitle, string sourcePath, string destinationZipFilePath, bool isSuccess, string? exception = null)
        {

            string emailContext = string.Empty;

            var statusText = isSuccess ? "Successfull" : "Failed";

            emailContext+=$"<h3>Application Title :{applicationTitle}</h2>";
            emailContext+=$"<h4> Source Path : {sourcePath}</h4>";
            emailContext+=$"<h4> Destination File: {destinationZipFilePath} </h4>";
            emailContext+=$"<h4> Operation Time : {DateTime.Now.TimeOfDay} </h4>";
            emailContext+= isSuccess ? $"<h4 style=\"color:Green;\"> Result : {statusText}</h4>" : $"<h4 style=\"color:Red;\"> Result : {statusText}</h4>";
            emailContext+=!string.IsNullOrEmpty(exception) ? $"<h4> Error : {exception} </h4>" : "";
            emailContext+="<hr style=\"height:2px;border-width:0;color:Black;background-color:Black\">";
            return emailContext;
        }
    }

}
