﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrackerApi.Models;

namespace TrackerApi.Services.UserService.ViewModel
{
    public class GetUsersViewModel
    {
        public int TotalUsers { get; set; }
        public List<User> Users { get; set; }
    }
}
