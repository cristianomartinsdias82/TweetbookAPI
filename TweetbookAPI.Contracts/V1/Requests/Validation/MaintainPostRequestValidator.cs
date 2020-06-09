using FluentValidation;
using System;

namespace TweetbookAPI.Contracts.V1.Requests.Validation
{
    /// <summary>
    /// The Asp.Net infrastructure automatically invokes the validation logic implemented here
    /// everytime an instance of MaintainPostRequest is received in all application endpoints
    /// (See MvcInstaller -> Filter registration logic)
    /// </summary>
    public class MaintainPostRequestValidator : AbstractValidator<MaintainPostRequest>
    {
        public MaintainPostRequestValidator()
        {
            RuleFor(x => x.Author)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$");

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty();

            RuleFor(x => x.DisplayUntil)
                .Must(x =>
                {
                    if (x.HasValue)
                        return x.Value > DateTime.Now;

                    return true;
                }).WithMessage("Display until must be greater than current date");
        }
    }
}
