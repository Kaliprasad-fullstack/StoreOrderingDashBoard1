using DbLayer.Repository;
using DbLayer.Service;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace StoreOrderingDashBoard.Models
{
    public class WebApiAuthorization : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();//validate client
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var _usersRepository = new UsersRepository();
            if (context.UserName != "" && context.Password != "")
            {
                DAL.Users user = _usersRepository.GetUserToken(context.UserName, context.Password);
                if(user!=null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, (user.RoleId+"-"+user.CustomerPlanId)));
                    //identity.AddClaim(new Claim("username", "password"));
                    identity.AddClaim(new Claim("UserName", user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.EmailAddress));
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid grant", "Provided username and password is incorrect");
                    return;
                }
            }
            else
            {
                context.SetError("invalid grant", "Provided username and password is incorrect");
                return;
            }
        }
    }
}