using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;

namespace Scandium.Features.User.Authentication
{
    public class Request
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    public class Validator : Validator<Request>
    {
        public Validator()
        {

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required !");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required !");
        }
    }
    
    public class Response
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
    }
}