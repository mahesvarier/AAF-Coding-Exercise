﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyApp.Models;

namespace LegacyApp.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUser(string firstName, string surname, string email, DateTime dateOfBirth, int clientId);
    }
}
