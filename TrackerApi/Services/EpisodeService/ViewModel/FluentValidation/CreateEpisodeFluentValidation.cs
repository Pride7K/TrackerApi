using FluentValidation;
using TrackerApi.Services.TvShowService.ViewModel;

namespace TrackerApi.Services.EpisodeService.ViewModel.FluentValidation
{
    public class CreateEpisodeFluentValidation : AbstractValidator<CreateEpisodeViewModel>
    {
        public CreateEpisodeFluentValidation()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("This field is required");
            RuleFor(x => x.TvShowId).NotEmpty().WithMessage("This field is required");
            RuleFor(x => x.ReleaseDate).NotEmpty().WithMessage("This field is required");
        }
    }
}
