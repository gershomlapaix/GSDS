using System.Security.Claims;
using Gsds.Data;
using Gsds.Models.Dossier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gsds.Controllers.Dossier
{
    public class ComplainerController: ControllerBase{

        // register a complainer method

        public static async Task<IResult> RegisterComplainer(Complainer complainer, GsdsDb db){
            Console.WriteLine(complainer);
            db.Complainers.Add(complainer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/complainer/{complainer.Id}", complainer);
        }

        public static async Task<IResult> GetAllComplainers(GsdsDb db){
            return TypedResults.Ok(await db.Complainers.ToArrayAsync());
        }
    }
}