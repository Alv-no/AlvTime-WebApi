﻿using AlvTime.Business.Users;
using AlvTimeWebApi.Persistence.DatabaseModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AlvTimeWebApi.Controllers.Admin.Users.UserStorage
{
    public class UserStorage : IUserStorage
    {
        private readonly AlvTime_dbContext _context;

        public UserStorage(AlvTime_dbContext context)
        {
            _context = context;
        }

        public void AddUser(CreateUserDto user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                StartDate = user.StartDate,
                FlexiHours = user.FlexiHours,
            };

            _context.User.Add(newUser);
            _context.SaveChanges();
        }

        public IEnumerable<UserResponseDto> GetUser(UserQuerySearch criterias)
        {
            return _context.User.AsQueryable()
                .Filter(criterias)
                .Select(u => new UserResponseDto
                {
                    Email = u.Email,
                    Id = u.Id,
                    Name = u.Name,
                    FlexiHours = u.FlexiHours,
                    StartDate = u.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                }).ToList();
        }
    }
}