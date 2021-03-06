﻿using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.BLL.Util;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Interfaces;
using System;

namespace TobaccoShop.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await db.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                //создаем пользователя
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.UserName };
                var result = await db.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                //добавляем пользователю роль
                await db.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                //создаём профиль пользователя
                ShopUser shopUser = new ShopUser
                {
                    Id = user.Id,
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    Role = userDto.Role,
                    RegisterDate = DateTime.Now
                };
                db.Users.Add(shopUser);
                await db.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно завершена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким email уже существует", "Email");
            }
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            //находим пользователя в бд по email
            ApplicationUser user = await db.UserManager.FindByEmailAsync(userDto.Email);
            if (user != null)
            {
                user = await db.UserManager.FindAsync(user.UserName, userDto.Password);
            }
            else //находим по UserName
            {
                user = await db.UserManager.FindAsync(userDto.Email, userDto.Password);
            }
            //ApplicationUser user = await db.UserManager.FindAsync(userDto.Email, userDto.Password);
            //авторизуем пользователя и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await db.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        //инициализация БД
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await db.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await db.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        //инициализация ролей и админа в Identity
        public async Task Init()
        {
            ApplicationRole userRole = new ApplicationRole { Name = "User" };
            ApplicationRole modRole = new ApplicationRole { Name = "Moderator" };
            ApplicationRole adminRole = new ApplicationRole { Name = "Admin" };
            await db.RoleManager.CreateAsync(userRole);
            await db.RoleManager.CreateAsync(modRole);
            await db.RoleManager.CreateAsync(adminRole);
            ApplicationUser admin = new ApplicationUser { Email = "asdqt@gmail.com", UserName = "Admin" };
            await db.UserManager.CreateAsync(admin, "123456");
            await db.UserManager.AddToRoleAsync(admin.Id, "Admin");
            ShopUser adminProfile = new ShopUser
            {
                Id = admin.Id,
                UserName = admin.UserName,
                Email = admin.Email,
                RegisterDate = DateTime.Now,
                Role = "Admin"
            };
            db.Users.Add(adminProfile);
            await db.SaveAsync();
        }

        public async Task<OperationDetails> ChangeUserRole(string id, string oldRole, string newRole)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    await db.UserManager.RemoveFromRoleAsync(id, oldRole);
                    await db.UserManager.AddToRoleAsync(id, newRole);
                    await db.SaveAsync();
                    transaction.Commit();
                    return new OperationDetails(true, "Роль пользователя обновлена", "");
                }
                catch
                {
                    transaction.Rollback();
                    return new OperationDetails(false, "Ошибка при изменении роли пользователя", "");
                }
            }
        }

        public UserDTO FindUserById(string id)
        {
            ShopUser user = db.Users.FindById(id);
            Mapper.Initialize(cfg => { cfg.AddProfile<AutomapperProfile>(); });
            return Mapper.Map<ShopUser, UserDTO>(user);
        }

        public async Task<UserDTO> FindUserByIdAsync(string id)
        {
            ShopUser user = await db.Users.FindByIdAsync(id);
            Mapper.Initialize(cfg => { cfg.AddProfile<AutomapperProfile>(); });
            return Mapper.Map<ShopUser, UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            List<ShopUser> users = await db.Users.GetUsersAsync();
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<ShopUser>, List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetUsersByNameAsync(string userName)
        {
            List<ShopUser> users = await db.Users.GetUsersAsync(p => p.UserName.Contains(userName));
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<ShopUser>, List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetUsersByRoleAsync(string role)
        {
            List<ShopUser> users = await db.Users.GetUsersAsync(p => p.Role == role);
            Mapper.Initialize(cfg => cfg.AddProfile<AutomapperProfile>());
            return Mapper.Map<List<ShopUser>, List<UserDTO>>(users);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
