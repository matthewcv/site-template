using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using mywebsite.backend.Entity;

namespace mywebsite.backend.Validation
{
    public class ProfileValidator:AbstractValidator<Profile>
    {
        public ProfileValidator()
        {


            RuleFor(p => p.DisplayName).NotEmpty();
            RuleFor(p => p.EmailAddress).EmailAddress();
        }
    }
}
