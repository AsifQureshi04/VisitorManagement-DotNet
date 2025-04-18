﻿using MFL_VisitorManagement.Dtos;
using MFL_VisitorManagement.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MFL_VisitorManagement.Interface
{
    public interface IAuthenticateRepository
    {
        Task<UserLoginResponse> UserLoginRepo(UserPayload userPayload);
        Task<string> UserRegistrationRepo(UserRegistration userRegistration);
        Task<int> UpdatePasswordRepo(UpdatePasswordPayload UpdatePasswordPayload);

    }
}
