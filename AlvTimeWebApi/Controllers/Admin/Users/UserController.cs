﻿using AlvTimeWebApi.Authentication;
using AlvTimeWebApi.Dto;
using AlvTimeWebApi.HelperClasses;
using AlvTimeWebApi.Persistence.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AlvTimeWebApi.Controllers.Admin.Users
{
    [Route("api/admin")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AlvTime_dbContext _database;

        private CreatedObjectReturner returnObjects;
        private ExistingObjectFinder checkExisting;
        private readonly IUserStorage _userStorage;

        public UserController(AlvTime_dbContext database, IUserStorage userStorage)
        {
            _database = database;
            _userStorage = userStorage;
            returnObjects = new CreatedObjectReturner(_database);
            checkExisting = new ExistingObjectFinder(_database);
        }

        [HttpGet("Users")]
        [AuthorizeAdmin]
        public ActionResult<IEnumerable<UserResponseDto>> FetchUsers()
        {
            var users = _userStorage.GetUser(new UserQuerySearch());
            return Ok(users);
        }

        [HttpPost("CreateUser")]
        [AuthorizeAdmin]
        public ActionResult<IEnumerable<UserResponseDto>> CreateNewUser([FromBody] IEnumerable<CreateUserDto> usersToBeCreated)
        {
            List<UserResponseDto> response = new List<UserResponseDto>();

            decimal? flexiHours = 0;

            var calculator = new AlvHoursCalculator();

            foreach (var user in usersToBeCreated)
            {
                if (user.FlexiHours != null)
                {
                    flexiHours = user.FlexiHours;
                }
                else
                {
                    flexiHours = 187.5M + calculator.CalculateAlvHours();
                }

                if (checkExisting.UserDoesNotExist(user))
                {
                    var newUser = new User
                    {
                        Name = user.Name,
                        Email = user.Email,
                        StartDate = user.StartDate,
                        FlexiHours = flexiHours
                    };
                    _database.User.Add(newUser);
                    _database.SaveChanges();

                    response.Add(returnObjects.ReturnCreatedUser(user));
                }
            }
            return Ok(response);
        }
    }
}