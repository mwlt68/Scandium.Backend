using FastEndpoints;
using FluentValidation;

namespace Scandium.Features.User.Insert
{
    public class Request
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            // Alphanumeric
            string usernameRegex = "^[a-zA-Z][a-zA-Z0-9]*$";
            // At least one uppercase letter, one lowercase letter and one number:
            string passwordRegex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d].*$";

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required!")
                .MinimumLength(7)
                .MaximumLength(25)
                .Matches(usernameRegex).WithMessage("Username can only alphanumeric!");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required!")
                .MinimumLength(7)
                .MaximumLength(25)
                .Matches(passwordRegex).WithMessage("Passsword contain at least one uppercase letter, one lowercase letter and one number");
            RuleFor(x => x.PasswordConfirm)
                .Equal(y => y.Password).WithMessage("Password confirm must equal to pasword!");
        }
    }
    
    public class Response
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}