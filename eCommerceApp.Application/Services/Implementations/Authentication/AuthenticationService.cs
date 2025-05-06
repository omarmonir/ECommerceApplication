using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.Services.Interfaces.Authentication;
using eCommerceApp.Application.Services.Interfaces.Logging;
using eCommerceApp.Application.Validations;
using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using FluentValidation;
using System.Net;

namespace eCommerceApp.Application.Services.Implementations.Authentication
{
    public class AuthenticationService
        (ITokenMangement tokenMangement, IUserMangement userMangement,
        IRoleMangement roleMangement, IAppLogger<AuthenticationService> logger,
        IMapper mapper, IValidator<CreateUser> createUserValidator
        , IValidator<LoginUser> loginUserValidator, IValidationService validationService) : IAuthenticationService
    {
        public async Task<ServiceResponse> CreateUser(CreateUser user)
        {
            var _validationResult = await validationService.ValidateAsync(user, createUserValidator);
            if (!_validationResult.Success) return _validationResult;

            var mapperModel = mapper.Map<AppUser>(user);
            mapperModel.UserName = user.Email;
            mapperModel.PasswordHash = user.Password;

            var result = await userMangement.CreateUser(mapperModel);
            if (!result)
                return new ServiceResponse { Message = "Email Address might be already in use or unkown error occurred" };

            var _user = await userMangement.GetUserByEmail(user.Email);
            var users = await userMangement.GetAllUsers();
            bool assignedResult = await roleMangement.AddUserToRole(_user!, users!.Count() > 1 ? "User" : "Admin");

            if (!assignedResult)
            {
                int removeUserResult = await userMangement.RemoveUserByEmail(user!.Email);
                if (removeUserResult <= 0)
                {
                    logger.LogError(
                        new Exception($"User with Email as {_user.Email} failed to be remove as a result of role assigning issue"),
                        "User could not assigned Role");
                    return new ServiceResponse { Message = "Error occured in create account" };
                }
            }

            return new ServiceResponse { Success = true, Message= "Account created!" };
        }

        public async Task<LoginResponse> LoginUser(LoginUser user)
        {
           var _validationResult = await validationService.ValidateAsync(user, loginUserValidator);
            if (!_validationResult.Success)
                return new LoginResponse(Message: _validationResult.Message);

            var mapperModel = mapper.Map<AppUser>(user);
            mapperModel.PasswordHash = user.Password;

            bool loginResult = await userMangement.LoginUser(mapperModel);
            if (!loginResult)
                return new LoginResponse(Message: "Email not Found our invalid credentials");

            var _user = await userMangement.GetUserByEmail(user.Email);
            var claims = await userMangement.GetUserClaims(_user!.Email!);

            string jwtTokent = tokenMangement.GenerateToken(claims);
            string refreshToken = tokenMangement.GetRefreshToken();

            int saveTokenResult = 0;
            bool userTokenCheck = await tokenMangement.ValidateRefrechToken(refreshToken);
            if(userTokenCheck)
                saveTokenResult = await tokenMangement.UpdateRefreshToken(_user.Id, refreshToken);
            else
                saveTokenResult = await tokenMangement.AddRefreshToken(_user.Id, refreshToken);

            return saveTokenResult <= 0 ? new LoginResponse(Message: "Internal error occured while authenticating") : 
                new LoginResponse(Success: true, Token: jwtTokent, RefreshToken: refreshToken);
        }

        public async Task<LoginResponse> ReviveUser(string refreshToken)
        {
           bool validateTokenResult = await tokenMangement.ValidateRefrechToken(refreshToken);
            if (!validateTokenResult) 
                return new LoginResponse(Message: "Invalid Tokrn");
            string userId = await tokenMangement.GetUserIdByRefreshToken(refreshToken);
            AppUser? user = await userMangement.GetUserById<AppUser>(userId);
            var claims = await userMangement.GetUserClaims(user!.Email!);

            string newJwtToken = tokenMangement.GenerateToken(claims);  
            string newRefreshToken= tokenMangement.GetRefreshToken();
            await tokenMangement.UpdateRefreshToken(userId, newRefreshToken);
            return new LoginResponse(Success: true, Token:newJwtToken, RefreshToken: newRefreshToken);
        }
    }
}
