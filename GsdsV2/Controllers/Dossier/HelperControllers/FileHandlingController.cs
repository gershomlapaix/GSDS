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
        public static async Task<string> UploadFiles(IFormFileCollection files, string complaintCode, ClaimsPrincipal user, IWebHostEnvironment hostEnvironment, GsdsDb db)
        {
           
            try
            {
                string path = "";
                foreach (var file in files)
                    {
                        string fileName = Path.GetFileName(file.FileName);

                    //var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadsFolder = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);

                        var attachment = new ComplaintAttachment();
                        attachment.ComplaintCode = complaintCode;
                        attachment.UploadedBy = user.FindFirstValue(ClaimTypes.NameIdentifier);
                        attachment.FilePath = Path.Combine(filePath, file.FileName);
                        attachment.FileName = uniqueFileName;
                        attachment.Extension = Path.GetExtension(fileName);
                        attachment.FileType = file.ContentType;

                        db.Attachments.Add(attachment);
                        await db.SaveChangesAsync();
                    }
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


        // Download the file
        public async Task<string> GetTheFile(HttpContext context)
        {
            var fileName = context.Request.RouteValues["fileName"].ToString();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }

            // Set the Content-Disposition header to "inline" to open the file in the browser
            context.Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);

            // Determine the appropriate Content-Type based on the file extension
            var contentType = GetContentType(fileName);
            context.Response.Headers.Add("Content-Type", contentType);

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            await stream.CopyToAsync(context.Response.Body);

            return $"http://41.74.167.30/api/files/download/{fileName}";
        }

        string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                // Add more file extensions and corresponding MIME types as needed
                _ => "application/octet-stream", // Default to binary data
            };
        }
    }
}
