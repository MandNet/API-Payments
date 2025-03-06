using API_Payments.Autentication.DTO;
using API_Payments.Autentication.Services;
using API_Payments.DTO;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API_Payments.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("v{version:apiVersion}")]
    public class TokenController : ControllerBase
    {
        private readonly IAutentication _autentication;
        public TokenController(IAutentication autentication)
        {
            _autentication = autentication;
        }

        public class Login
        {
            public string? Id { get; set; }
            public string? token { get; set; }
        }

        [MapToApiVersion(1)]
        [HttpPost("Token")]
        public async Task<ActionResult<ResponseDTO<TokenDTO>>> Token([FromBody] Login? param = null)
        {
            var headers = Request.Headers;
            string id = "";
            string senha = "";
            ResponseDTO<TokenDTO> token = new ResponseDTO<TokenDTO>();
            token.Data = new TokenDTO();

            try
            {
                if (headers.ContainsKey("x-id"))
                {
                    id = headers["x-id"].ToString();
                }
                else
                {
                    if (param != null && param.Id != null)
                        id = param.Id;
                }
                if (headers.ContainsKey("x-token"))
                {
                    senha = headers["x-token"].ToString();
                }
                else
                {
                    if (param != null && param.token != null)
                        senha = param.token;
                }

                if (_autentication.Autenticate(id, senha))
                {
                    token.Data = _autentication.GenerateToken(id, senha);
                    token.Message = "Token Generated";
                }
                else
                {
                    token.Message = "User not authorized";
                    token.Success = false;
                }
            }
            catch (Exception ex)
            {
                token.Message = ex.Message;
                token.Success = false;
            }

            return Ok(token);
        }
    }
}

   