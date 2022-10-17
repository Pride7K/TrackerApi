using FluentValidation;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Services.ActorService.ViewModel.FluentValidation
{
    public class CreateActorFluentValidation : AbstractValidator<CreateActorViewModel>
    {
        public CreateActorFluentValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must have at least one character");
            RuleFor(x => x.Name).MinimumLength(1).WithMessage("Name must have at least one character");
            RuleFor(x => x.Name).MaximumLength(255).WithMessage("Name must be lower than 255 characters");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description must have at least one character");
            RuleFor(x => x.Description).MinimumLength(1).WithMessage("Description must have at least one character");
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("Description must be lower than 255 characters");
        }
    }
}
