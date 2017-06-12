using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TobaccoShop.BLL.DTO;
using TobaccoShop.BLL.Infrastructure;
using TobaccoShop.BLL.Interfaces;
using TobaccoShop.DAL.Entities.Identity;
using TobaccoShop.DAL.Interfaces;

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
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, UserName = userDto.UserName };
                db.ClientManager.Create(clientProfile);
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
                user = await db.UserManager.FindAsync(user.Email, userDto.Password);
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
            ClientProfile adminProfile = new ClientProfile { Id = admin.Id, UserName = admin.UserName };
            db.ClientManager.Create(adminProfile);
            await db.SaveAsync();
        }

        public async Task<UserDTO> GetCurrentUser(string id)
        {
            ClientProfile user = await db.ClientManager.FindByIdAsync(id);
            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return userDTO;
        }

        public string GetCurrentUserName(string id)
        {
            var user = db.ClientManager.FindById(id);
            return user.UserName;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
