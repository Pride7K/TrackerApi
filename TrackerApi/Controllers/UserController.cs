using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.UserService;
using TrackerApi.Services.UserService.ViewModel;

namespace TrackerApi.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("skip/{skip:int}/take/{take:int}")]
        [Authorize]
        public async Task<IActionResult> GetAsync([FromRoute] int skip = 0
            , [FromRoute] int take = 25)
        {
            if (take > 1000)
                return BadRequest();

            var data = await _service.GetAll(skip, take);

            return Ok(data);
        }


        [HttpGet("{id:int}")]
        [Authorize]

        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var user = await _service.GetById(id);

            if (user == null)
                return NotFound();


            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync( [FromBody] CreateUserViewModel model)
        {


            if (!ModelState.IsValid)
                return BadRequest();

            var userDb = await _service.GetByEmail(model.Email);

            if (userDb != null)
                return BadRequest(userDb);

            try
            {
                var user =  _service.Create(model);

                return Created($"v1/users/{user.Id}", user);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
