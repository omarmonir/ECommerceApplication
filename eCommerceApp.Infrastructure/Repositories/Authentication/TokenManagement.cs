using eCommerceApp.Domain.Entities.Identity;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace eCommerceApp.Infrastructure.Repositories.Authentication
{
    public class TokenManagement(AppDbContext context, IConfiguration config) : ITokenMangement
	{
		public async Task<int> AddRefreshToken(string userId, string refreshToken)
		{
			context.RefreshToken.Add(new RefreshToken
			{
				UserId = userId,
				Token = refreshToken
			});
			return await context.SaveChangesAsync();
		}

		public string GenerateToken(List<Claim> claims)
		{
			var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!));
			var cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
			var expiration = DateTime.Now.AddHours(2);
			var token = new JwtSecurityToken(
				issuer: config["JWT:Issuer"],
				audience: config["JWT:Audience"],
				claims: claims,
				expires: expiration,
				signingCredentials: cred);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GetRefreshToken()
		{
			const int byteSize = 64;
				byte[] randomBytes = new byte[byteSize];
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomBytes);	
			}
			string token =  Convert.ToBase64String(randomBytes);
			return WebUtility.UrlEncode(token);
		}

		public List<Claim> GetUserClaimsFromToken(string token)
        {
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			if (jwtToken != null)
				return jwtToken.Claims.ToList();
			else
				return [];
		}

		public async Task<string> GetUserIdByRefreshToken(string refreshToken)
		=> (await context.RefreshToken.FirstOrDefaultAsync(_ => _.Token == refreshToken))!.UserId;

		public async Task<int> UpdateRefreshToken(string userId, string refreshToken)
		{
          
			var user = await context.RefreshToken.FirstOrDefaultAsync(_ => _.Token == refreshToken);
			if (user == null) return -1;
			user.Token = refreshToken;
			return await context.SaveChangesAsync();

        }

		public async Task<bool> ValidateRefrechToken(string refrechToken)
		{
			var user = await context.RefreshToken.FirstOrDefaultAsync(_ => _.Token == refrechToken);
			return user != null;

        }
	}
}
