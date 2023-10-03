using Gsds.Data;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.Mail;
using System.Security.Claims;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class FileHandlingController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileHandlingController() { }
        public FileHandlingController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        // upload the file
        public static async Task<string> UploadFiles(IFormFileCollection files, string complaintCode, ClaimsPrincipal user, GsdsDb db)
        {
           
            try
            {
                string path = "";
                foreach (var file in files)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);

                                var attachment = new ComplaintAttachment();
                                attachment.ComplaintCode = complaintCode;
                                attachment.UploadedBy = user.FindFirstValue(ClaimTypes.NameIdentifier);
                                attachment.FilePath = "~/UploadedFiles/"+fileName;
                                attachment.FileName = fileName;

                                db.Attachments.Add(attachment);
                                await db.SaveChangesAsync();

                    }

                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    await file.CopyToAsync(ms);

                    //    var attachment = new ComplaintAttachment();
                    //    attachment.ComplaintCode = complaintCode;
                    //    attachment.UploadedBy = user.FindFirstValue(ClaimTypes.NameIdentifier);
                    //    attachment.FilePath = filePath;
                    //    attachment.FileName = fileName;

                    //    db.Attachments.Add(attachment);
                    //    await db.SaveChangesAsync();
                    //}

                }
                    return "Done";
            }

            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }


        // get files
        public static async Task<IResult> getFiles(GsdsDb db)
        {
            return TypedResults.Ok(await db.Attachments.ToArrayAsync());
        }

        // GET ONE FILE
        //public string downloadFile(int id, GsdsDb db)
        public static async Task<string> downloadFile(IPathProvider pathProvider)
        {
            
            Console.WriteLine(pathProvider.MapPath("/UploadedFiles/Gershom_Reflective Thinking_2023-09-19.docx"));
            return pathProvider.MapPath("/UploadedFiles/Gershom_Reflective Thinking_2023-09-19.docx");

            //return TypedResults.Ok(await db.Attachments
            //     .Where(c => c.Id == id)
            //     .ToListAsync());
        }


        // // DOWNLOAD FILE
        // public static async Task<IResult> downloadFile(int fileId, GsdsDb db)
        //{
        //    byte[] bytes;
        //    string fileName, contentType;
        //}
    }
}
