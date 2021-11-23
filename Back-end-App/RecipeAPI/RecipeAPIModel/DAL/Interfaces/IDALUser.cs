using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAPIModel.DAL.Interfaces
{
    public interface IDALUser
    {
        Task<bool> InsertUser(User user);

        Task<User> GetUser(string mail);
    }
}
