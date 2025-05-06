using System.Security.Claims;
namespace eCommerceApp.Domain.Interfaces.Authentication
{
	public interface ITokenMangement
	{
		string GetRefreshToken();
		List<Claim> GetUserClaimsFromToken(string token);
		Task<bool> ValidateRefrechToken(string refrechToken);
		Task<string> GetUserIdByRefreshToken(string refreshToken);
		Task<int> AddRefreshToken(string userId, string refreshToken);
		Task<int> UpdateRefreshToken(string userId, string refreshToken);
		string GenerateToken(List<Claim> claims);	
	}
}
