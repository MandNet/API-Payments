using API_Payments.Autentication.DTO;
using API_Payments.DTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Payments.Autentication.Services
{
    public class AutenticationService : IAutentication
    {
        private readonly ApiSettings? _apiSet;
        private readonly jwtToken _jwt;
        private readonly List<Security> _security;
        private readonly int tempoExpiracao = 3600;

        public AutenticationService(IOptions<jwtToken> jwt, IOptions<List<Security>> security)
        {
            _security = security.Value;
            _jwt = jwt.Value;
        }
        public bool Autenticate(string id, string senha)
        {
            bool ret = false;

            for (int i = 0; i < _security.Count; i++)
            {
                if (_security[i].id.ToLower() == id.ToLower() && _security[i].token == senha)
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        public TokenDTO GenerateToken(string id, string senha)
        {
            var claims = new[]
            {
                new Claim("id", id),
                new Claim("senha", senha),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privatekey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.secretkey));

            var credentials = new SigningCredentials(privatekey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddSeconds(tempoExpiracao);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwt.issuer,
                audience: _jwt.audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            TokenDTO ret = new TokenDTO();
            ret.token = new JwtSecurityTokenHandler().WriteToken(token);
            ret.exoiresIn = tempoExpiracao;
            ret.tokenType = "Bearer";

            return ret;
        }
    }
}
