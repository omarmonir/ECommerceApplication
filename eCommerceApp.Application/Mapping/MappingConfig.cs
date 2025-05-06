using AutoMapper;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Entities.Identity;

namespace eCommerceApp.Application.Mapping
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<CreateCategory, Category>();
			CreateMap<CreateProduct, Product>();

			CreateMap<Category, GetCategory>();
			CreateMap<Product, GetProduct>();

			CreateMap<CreateUser, AppUser>();
			CreateMap<LoginUser, AppUser>();

			CreateMap<PaymentMetod, GetPaymentMethod>();


			CreateMap<CreateAchiveDto, Achive>();
	
		}
	}
}
