﻿using System;
using System.Collections.Generic;

namespace TobaccoShop.BLL.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime RegisterDate { get; set; }

        public List<OrderDTO> Orders { get; set; }

        public UserDTO()
        {
            Orders = new List<OrderDTO>();
        }
    }
}
