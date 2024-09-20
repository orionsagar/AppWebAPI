using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using PVSAPI.DAL;
using PVSAPI.Models;
using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace PVSAPI.App_Start
{
    internal class ApplicationAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserAccountRepository _repo = new UserAccountRepository())
            {
                var user = _repo.GetUserAccounts(context.UserName, context.Password);
                if (user != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    foreach (UserAccount ua in user)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, ua.Name));
                        //identity.AddClaim(new Claim(ClaimTypes.Email, ua.Email == null ? "" : ua.Email));
                        identity.AddClaim(new Claim(ClaimTypes.Role, ua.RoleName));


                        //var props = new AuthenticationProperties(new Dictionary<string, string>
                        //            {
                        //                {
                        //                    "Name", context.UserName
                        //                }
                        //            });

                        //var ticket = new AuthenticationTicket(identity, props);
                        //context.Validated(ticket);

                        context.Validated(identity);
                    }
                    //identity.AddClaim(new Claim(ClaimTypes.Role, user.));

                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    context.Rejected();
                    return;
                }


            }
        }

        //public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        //{
        //    //validate your client
        //    //var currentClient = context.ClientId;

        //    //if (Client does not match)
        //    //{
        //    //    context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
        //    //    return Task.FromResult<object>(null);
        //    //}


        //    // Change authentication ticket for refresh token requests
        //    var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
        //    newIdentity.AddClaim(new Claim("newClaim", "newValue"));

        //    var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
        //    context.Validated(newTicket);

        //    return Task.FromResult<object>(null);
        //}

        //public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        //{
        //    foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
        //    {
        //        context.AdditionalResponseParameters.Add(property.Key, property.Value);
        //    }
        //    return Task.FromResult<object>(null);
        //}
    }
}