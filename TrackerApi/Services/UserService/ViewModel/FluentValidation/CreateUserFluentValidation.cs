using FluentValidation;
using System.Text.RegularExpressions;

namespace TrackerApi.Services.UserService.ViewModel.FluentValidation
{
    public class CreateUserFluentValidation : AbstractValidator<CreateUserViewModel>
    {
        public CreateUserFluentValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must have at least one character");
            RuleFor(x => x.Name).MinimumLength(1).WithMessage("Name must have at least one character");
            RuleFor(x => x.Name).MaximumLength(255).WithMessage("Name must be lower than 255 characters");

            RuleFor(x => x.Email).MinimumLength(1).WithMessage("Email must have at least one character");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email must have at least one character");
            RuleFor(x => x.Email).MaximumLength(255).WithMessage("Email must be lower than 255 characters");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Inform a valid email");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password must have at least four character");
            RuleFor(x => x.Password).MinimumLength(4).WithMessage("Password must have at least four character");
            RuleFor(x => x.Password).MaximumLength(255).WithMessage("Password must be lower than 255 characters");
        }
    }
}
