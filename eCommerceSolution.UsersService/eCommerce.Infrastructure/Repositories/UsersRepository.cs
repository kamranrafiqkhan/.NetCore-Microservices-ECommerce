using eCommerce.Core.DTO;
using eCommerce.Core.Entities;
using eCommerce.Core.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.Repositories;

internal class UsersRepository : IUsersRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        //Generate a new unique user ID for the user
        user.UserID = Guid.NewGuid();
        return user;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        return new ApplicationUser()
        {
            UserID = Guid.NewGuid(),
            Email = email,
            Password = password,
            PersonName = "Person Name",
            Gender = GenderOptions.Male.ToString()
        };
    }
}

