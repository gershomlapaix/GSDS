using Gsds.Data;
using GsdsV2.DTO.Dossier;
using GsdsV2.Models.HelperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.Mail;

namespace GsdsV2.Controllers.Dossier.HelperControllers
{
    public class FileHandlingController : ControllerBase
    {
        // upload the file
        public static async Task<string> uploadFiles(IFormFileCollection files, string complaintCode, GsdsDb db)
        {
            try
            {
                foreach (var file in files)
                {
                        string fileName = Path.GetFileName(file.FileName);
                        string contentType = file.ContentType;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);

                        var attachment = new ComplaintAttachment();
                        attachment.ComplaintCode = complaintCode;
                        attachment.File = ms.ToArray();
                        attachment.FileName = fileName;

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

        // GET ONE FILE
        public static async Task<IResult> downloadFile(int id, GsdsDb db)
        {
            return TypedResults.Ok(await db.Attachments
                 .Where(c => c.Id == id)
                 .ToListAsync());
        }


        // // DOWNLOAD FILE
        // public static async Task<IResult> downloadFile(int fileId, GsdsDb db)
        //{
        //    byte[] bytes;
        //    string fileName, contentType;
        //}
    }
}
