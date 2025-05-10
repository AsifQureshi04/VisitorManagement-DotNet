using MFL_VisitorManagement.Data;
using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using MFL_VisitorManagement.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Cryptography;
using System.Text;


namespace MFL_VisitorManagement.Repositories
{
    public class AuthenticateRepository(DataContext dataContext) : IAuthenticateRepository
    {

        public async Task<UserLoginResponse> UserLoginRepo(UserPayload userPayload)
        {

            string computedHashString = string.Empty;
            var passwordSalt = new OracleParameter("p_PasswordSalt", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };

            await dataContext.Database.ExecuteSqlRawAsync(
                @"BEGIN Sp_GetPasswordSalt(:p_UserId, :p_PasswordSalt); END;",
                new OracleParameter("p_UserId", userPayload.UserId),
                passwordSalt
            );

            if (passwordSalt.Value.ToString() == "null") return null!;

            using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt.Value.ToString()!)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userPayload.Password));
                computedHashString = Convert.ToBase64String(computedHash);
            }

            var firstName = new OracleParameter("p_FirstName", OracleDbType.Varchar2, 1000) { Direction = ParameterDirection.Output };
            var lastName = new OracleParameter("p_LastName", OracleDbType.Varchar2, 1000) { Direction = ParameterDirection.Output };
            var email = new OracleParameter("p_EmailId", OracleDbType.Varchar2, 1000) { Direction = ParameterDirection.Output };
            var role = new OracleParameter("p_UserRole", OracleDbType.Varchar2, 1000) { Direction = ParameterDirection.Output };
            var Id = new OracleParameter("p_Id", OracleDbType.Varchar2, 1000) { Direction = ParameterDirection.Output };


            var res = await dataContext.Database.ExecuteSqlRawAsync("BEGIN Sp_UserLogin(:UserId, :PasswordHash, :p_FirstName, :p_LastName, :p_EmailId, :p_UserRole, :p_Id); END;",
                                                            new OracleParameter("UserId", userPayload.UserId),
                                                            new OracleParameter("PasswordHash", computedHashString),
                                                            firstName, lastName, email, role, Id);

            if (Id.Value is DBNull) return null!;

            var result = new UserLoginResponse
            {
                FirstName = firstName.Value?.ToString()!,
                LastName = lastName.Value?.ToString()!,
                EmailId = email.Value?.ToString()!,
                UserRole = role.Value?.ToString()!,
                Id = Id.Value?.ToString()!
            };

            return result;
        }

        public async Task<string> UserRegistrationRepo(UserRegistration userRegistration)
        {
            var UserIdParameter = new OracleParameter("p_Result", OracleDbType.Varchar2, 50)
            {
                Direction = ParameterDirection.Output
            };

            await dataContext.Database.ExecuteSqlRawAsync(
                "BEGIN Sp_UserRegistration(:p_FirstName, :p_LastName, :p_EmailId, :p_UserRole, :p_PasswordHash, :p_PasswordSalt, :p_Result);END;",
                new OracleParameter("p_FirstName", userRegistration.FirstName),
                new OracleParameter("p_LastName", userRegistration.LastName),
                new OracleParameter("p_EmailId", userRegistration.EmailId),
                new OracleParameter("p_UserRole", userRegistration.UserRole),
                new OracleParameter("p_PasswordHash", userRegistration.PasswordHash),
                new OracleParameter("p_PasswordSalt", userRegistration.PasswordSalt),
                UserIdParameter
                );
            return UserIdParameter.Value.ToString()!;
        }
        public async Task<int> UpdatePasswordRepo(UpdatePasswordPayload UpdatePasswordPayload)
        {
            string CurrentPasswordHash = string.Empty;
            string CurrentPasswordSalt = string.Empty;

            string NewPasswordHash = string.Empty;
            string NewPasswordSalt = string.Empty;

            var passwordSalt = new OracleParameter("p_PasswordSalt", OracleDbType.Varchar2, 4000)
            {
                Direction = ParameterDirection.Output
            };

            await dataContext.Database.ExecuteSqlRawAsync(
                @"BEGIN Sp_GetPasswordSalt(:p_UserId, :p_PasswordSalt); END;",
                new OracleParameter("p_UserId", UpdatePasswordPayload.UserId),
                passwordSalt
            );

            if (passwordSalt.Value is DBNull) return 0!;

            using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt.Value.ToString()!)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(UpdatePasswordPayload.CurrentPassword));
                CurrentPasswordHash = Convert.ToBase64String(computedHash);
            }

            using var hmac1 = new HMACSHA512();
            NewPasswordHash = Convert.ToBase64String(hmac1.ComputeHash(Encoding.UTF8.GetBytes(UpdatePasswordPayload.NewPassword)));
            NewPasswordSalt = Convert.ToBase64String(hmac1.Key);

            var parameters = new[]
            {
               new OracleParameter("p_NewPasswordHash",NewPasswordHash),
               new OracleParameter("p_NewPasswordSalt",NewPasswordSalt), 
               new OracleParameter("p_CurrentPasswordHash",CurrentPasswordHash),
               new OracleParameter("p_UserId",UpdatePasswordPayload.UserId),
               new OracleParameter("p_IsUpdated",OracleDbType.Int32){Direction = ParameterDirection.Output}
            };

            await dataContext.Database.ExecuteSqlRawAsync(
                @"BEGIN Sp_UpdatePassword(:p_UserId,:p_NewPasswordHash,:p_NewPasswordSalt,:p_CurrentPasswordHash,:p_IsUpdated);END;",
                parameters);

            return (int)(Oracle.ManagedDataAccess.Types.OracleDecimal)parameters[4].Value; 
        }
    }
}


      