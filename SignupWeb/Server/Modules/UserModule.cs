using Azure;
using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SignupWeb.Server.DatabaseContext;
using SignupWeb.Server.Models;

namespace SignupWeb.Server.Modules
{
    public class UserModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var bushims = app.MapGroup("/User");

            bushims.MapPost($"/{nameof(Signup)}", Signup);
        }

        [AllowAnonymous]
        async Task<IResult> Signup(Customer cusInf, EgbLoyaltyDb db, CancellationToken cancel)
        {
            try
            {
                var existed = await db.Customers.AnyAsync(x => x.Mobile == cusInf.Mobile);
                if (existed)
                {
                    return TypedResults.Conflict("The Mobile number already exists in the database. Please use a different number.");
                }
                else
                {
                    var count = (await db.Customers.MaxAsync(x => x.ID, cancellationToken: cancel) ?? 0) + 1;
                    await db.Customers.AddAsync(new Customer()
                    {
                        ID = count,
                        //CardNumber = count,
                        Address = cusInf.Address,
                        DOB = cusInf.DOB,
                        Email = cusInf.Email,
                        Mobile = cusInf.Mobile,
                        Name = cusInf.Name,
                        Phone = cusInf.Phone,
                        //Password = cusInf.Password,
                        PostCode = cusInf.PostCode,
                    }, cancel);
                    await db.SaveChangesAsync(cancel);
                    return TypedResults.Ok();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                Log.Logger.Error(ex.StackTrace);
                return TypedResults.BadRequest(ex.Message);
            }
        }
    }
}
