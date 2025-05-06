using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace eCommerceApp.Infrastructure.Repositories.Authentication
{
    public class UserManagement(IRoleMangement roleMangement, UserManager<AppUser> userManager, AppDbContext context) : IUserMangement
	{
		public async Task<bool> CreateUser(AppUser user)
		{
			var _user = await GetUserByEmail(user.Email!);
			if (_user != null) return false;

			return (await userManager.CreateAsync(user!, user!.PasswordHash!)).Succeeded;
		}

		public async Task<IEnumerable<AppUser?>> GetAllUsers() => await context.Users.ToListAsync();


		public async Task<AppUser?> GetUserByEmail(string email) =>
			await userManager.FindByEmailAsync(email);

		public async Task<AppUser?> GetUserById<T>(string id)
		{
			var user = await userManager.FindByIdAsync(id);
			return user!;

		}


		public async Task<List<Claim>> GetUserClaims(string email)
		{
			var _user = await GetUserByEmail(email);
			string? roleName = await roleMangement.GetUserRole(_user!.Email!);
			List<Claim> claims = [
				new Claim("FulName", _user!.FullName),
				new Claim(ClaimTypes.NameIdentifier, _user!.Id),
				new Claim(ClaimTypes.Email, _user!.Email!),
				new Claim(ClaimTypes.Role, roleName!)
				];
			return claims;
		}

		public async Task<bool> LoginUser(AppUser user)
		{
			var _user = await GetUserByEmail(user!.Email!);
			if (_user is null) return false;

			string? roleName = await roleMangement.GetUserRole(user!.Email!);
			if (string.IsNullOrEmpty(roleName)) return false;

			return await userManager.CheckPasswordAsync(_user, user.PasswordHash!);
		}

		public async Task<int> RemoveUserByEmail(string email)
		{
			var user = await context.Users.FirstOrDefaultAsync(_ => _.Email == email);
            context.Users.Remove(user);
            return await context.SaveChangesAsync();
		}
	}
}
