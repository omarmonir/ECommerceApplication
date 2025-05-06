using eCommerceApp.Domain.Entities.Identity;
using System.Threading.Tasks;
namespace eCommerceApp.Domain.Interfaces.Authentication
{
	public interface IRoleMangement
	{
		Task<string?> GetUserRole(string userEmail);
		Task<bool> AddUserToRole(AppUser user, string roleName);

	}
}
