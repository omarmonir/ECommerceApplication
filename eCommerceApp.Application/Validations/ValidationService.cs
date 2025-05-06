using eCommerceApp.Application.DTOs;
using FluentValidation;

namespace eCommerceApp.Application.Validations
{
    public class ValidationService : IValidationService
    {
        public async Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator)
        {
           var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                string errorsToString = string.Join(", ", errors);  
                return new ServiceResponse { Message = errorsToString };
            }
            return new ServiceResponse { Success = true };
        }
    }
}
