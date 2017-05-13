using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Interfaces;

namespace TobaccoShop.BLL.Services
{
    public class UserService: IUserService
    {
        private IUnitOfWork db { get; set; }

        public UserService(IUnitOfWork uow)
        {
            db = uow;
        }

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
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Address = userDto.Address, Name = userDto.Name };
                db.ClientManager.Create(clientProfile);
                await db.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно завершена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким e-mail уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate (UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            //находим пользователя в бд
            ApplicationUser user = await db.UserManager.FindAsync(userDto.Email, userDto.Password);
            //авторизуем пользователя и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await db.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        //инициализация БД
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach(string roleName in roles)
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
