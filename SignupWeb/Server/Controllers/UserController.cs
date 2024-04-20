using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using SignupWeb.Server.DatabaseContext;
using SignupWeb.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace SignupWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        readonly EgbLoyaltyDb db;

        public UserController(ILogger<UserController> logger, EgbLoyaltyDb database)
        {
            _logger = logger;
            db = database;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Signup))]
        public async Task<IResult?> Signup(Customer cusInf, CancellationToken cancel)
        {
            try
            {
                var count = (await db.Customers.MaxAsync(x => x.ID, cancellationToken: cancel) ?? 0) + 1;
                //await db.Customers.AddAsync(new Customer()
                //{
                //    CardNumber = count,
                //    Address = cusInf.Address,
                //    DOB = cusInf.DOB,
                //    Email = cusInf.Email,
                //    Mobile = cusInf.Mobile,
                //    Name = cusInf.Name,
                //    Phone = cusInf.Phone,
                //    Password = cusInf.Password,
                //    PostCode = cusInf.PostCode,
                //}, cancel);
                //await db.SaveChangesAsync(cancel);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem();
            }
        }
    }
}