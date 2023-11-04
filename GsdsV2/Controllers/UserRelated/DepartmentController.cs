using Gsds.Data;
using Gsds.Models.Dossier;
using GsdsV2.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GsdsV2.Controllers.UserRelated
{
    public class DepartmentController : ControllerBase
    {
        public static async Task<IResult> GetAllDepartments(GsdsDb db)
        {
            var departments = await db.Departments.ToListAsync();
            return TypedResults.Ok(departments);
        }

        //Get One department
        public static async Task<IResult> GetDepartmentById(string departmentId, GsdsDb db)
        {
            //return await db.Complainers.Where(c => c.Id == complainerNId).Include(_ => _.Complaints).ToListAsync()
            return await db.Departments.Where(c => c.Id == departmentId).ToListAsync()
               is List<Department> theDepartment
           ? TypedResults.Ok(theDepartment[0])
           : TypedResults.NotFound();
        }
    }
}
