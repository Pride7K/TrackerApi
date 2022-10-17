using FluentValidation;

namespace TrackerApi.Services.TvShowService.ViewModel.FluentValidation
{
    public class PutTvShowFluentValidation : AbstractValidator<PutTvShowViewModel>
    {
        public PutTvShowFluentValidation()
        {
            RuleFor(x => x.Title).MinimumLength(1).WithMessage("Title must have at least one character");
            RuleFor(x => x.Title).MaximumLength(255).WithMessage("Title must be lower than 255 characters");

            RuleFor(x => x.Description).MinimumLength(1).WithMessage("Description must have at least one character");
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("Description must be lower than 255 characters");
        }
    }
}
