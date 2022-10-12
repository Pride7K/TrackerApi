﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrackerApi.Data;
using TrackerApi.Models;
using TrackerApi.Services.Erros;
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
                return NotFound(new { ErrorMessage = "Not found", model = user });


            return Ok(user);
        }

        [HttpPost]
        public IActionResult PostAsync( [FromBody] CreateUserViewModel model)
        {


            if (!ModelState.IsValid)
                return BadRequest(new { ErrorMessage = "Must provide a valid object", model = model });

            try
            {
                var user =  _service.Create(model);

                return Created($"v1/users/{user.Id}", user);
            }
            catch (AlreadyExistsException e)
            {
                return Problem($"An error occured: {e.Message}");
            }
            catch (Exception e)
            {
                return Problem($"An error occured: {e.Message}");
            }

        }
    }
}
