using Dapper;
using MySql.Data.MySqlClient;
using PruebaAPI.Model;
using RecipeAPIModel.DAL.Common;
using RecipeAPIModel.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAPIModel.DAL.Implementations
{
    public class DALUser : IDALUser
    {
        private MySQLConfiguration _connectionString;
        public DALUser(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        async public Task<bool> InsertUser(User user) 
        {
            var db = dbConnection();
            var sql = @"INSERT INTO users (mail, password) VALUES (@Mail, @Password)";
            var password = GlobalMethods.ConvertToEncrypt(user.password);
            var result = await db.ExecuteAsync(sql, new {user.mail, password});
            return result > 0;
        }

        async public Task<User> GetUser(string mail) {
            var db = dbConnection();
            var sql = @"SELECT mail, password FROM users WHERE mail = @Mail";
            var user = await db.QueryFirstOrDefaultAsync<User>(sql, new { Mail = mail});
            if (user != null)
            {
                user.password = GlobalMethods.ConvertToDecrypt(user.password);
            }
            return user;
        }
    }
}
