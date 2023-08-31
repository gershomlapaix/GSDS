using Gsds.Data;
using Gsds.Models.Dossier;
using Microsoft.AspNetCore.Mvc;

namespace Gsds.Controllers.Dossier
{
    public class ComplainerController: ControllerBase{

        // register a complainer method

        public static async Task<IResult> RegisterComplainer(Complainer complainer, GsdsDb db){
            // Console.WriteLine(complainer);
            // // db.Complainers.Add(complainer);
            // // await db.SaveChangesAsync();

            if(complainer.Name != null){
                return TypedResults.Ok("sana");
            }
            else{
                return TypedResults.Ok("No no");
            }

            // return TypedResults.Created($"/complainer/{complainer.Id}", complainer);
        }
    }
}