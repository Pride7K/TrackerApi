using FluentValidation;

namespace TrackerApi.Services.TvShowService.ViewModel.FluentValidation
{
    public class CreateTvShowFluentValidation : AbstractValidator<CreateTvShowViewModel>
    {
        public CreateTvShowFluentValidation()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title must have at least one character");
            RuleFor(x => x.Title).MinimumLength(1).WithMessage("Title must have at least one character");
            RuleFor(x => x.Title).MaximumLength(255).WithMessage("Title must be lower than 255 characters");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Title must have at least one character");
            RuleFor(x => x.Description).MinimumLength(1).WithMessage("Title must have at least one character");
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("Title must be lower than 255 characters");


            RuleFor(x => x.StillGoing).NotNull().WithMessage("Still going is required");

        }
    }
}
