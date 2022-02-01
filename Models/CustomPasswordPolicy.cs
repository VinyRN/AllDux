using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alldux_plataforma.Models;
using Microsoft.AspNetCore.Identity;

namespace alldux_plataforma.Models
{
    public class CustomPasswordPolicy : PasswordValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            IdentityResult result = await base.ValidateAsync(manager, user, password);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();
 
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Description = "A senha não pode ser igual ao nome de usuário"
                });
            }
            if (password.Contains("123"))
            {
                errors.Add(new IdentityError
                {
                    Description = "A senha não pode conter a seqüencia numérica 123"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}