using Domain.Models;
using FluentValidation;

namespace Domain.Validation
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Title is required.");
            //RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Category).NotNull().NotEmpty().WithMessage("Category is required.");
            RuleFor(x => x.City).NotNull().NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Date).NotNull().NotEmpty().WithMessage("Date is required.")
                                 .Must(date => date > DateTime.Now).WithMessage("Date cannot be in the past.");
            RuleFor(x => x.Venue).NotNull().NotEmpty().WithMessage("Venue is required.");
        }
    }
}