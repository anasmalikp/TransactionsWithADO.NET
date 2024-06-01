using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transactions.Models;
using Transactions.Services;

namespace Transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;
        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = services.GetAll();
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult transfer(TransferFundDto transferFund)
        {
            var result = services.TransferFund(transferFund);
            if(result)
            {
                return Ok("successfully transferred");
            }
            return BadRequest("couldn't complete the transaction");
        }
    }
}
