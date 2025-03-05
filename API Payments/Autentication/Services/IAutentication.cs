using API_Payments.Autentication.DTO;

namespace API_Payments.Autentication.Services
{
    public interface IAutentication
    {
        public TokenDTO GenerateToken(string id, string senha);
        public bool Autenticate(string id, string senha);
    }
}
