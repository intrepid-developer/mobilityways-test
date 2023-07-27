using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobilityWays.Application.Helpers;

//JWT Helper class to create and decode JWTs 
public static class JwtHelper
{
    //Static class to generate the Jwt with the name and email claims
    public static string GenerateJwt(string name, string email, string secretKey)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, name),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

        //Create Signing Key based on secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //Valid token for 1 day
        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

        //Create the token
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //return the jwt
        return jwt;
    }
}
