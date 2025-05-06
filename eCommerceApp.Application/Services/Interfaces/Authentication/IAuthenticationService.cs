using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceApp.Application.Services.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse> CreateUser(CreateUser user);
        Task<LoginResponse> LoginUser(LoginUser user);
        Task<LoginResponse> ReviveUser(string refreshToken);

    }
}
