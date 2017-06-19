﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;

namespace TobaccoShop.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        Task Init();

        Task<UserDTO> FindUser(string id);
        string GetCurrentUserName(string id);

        Task<List<UserDTO>> GetUsersAsync();
        Task<List<UserDTO>> GetUsersByNameAsync(string userName);
        Task<List<UserDTO>> GetUsersByRoleAsync(string role);
    }
}
