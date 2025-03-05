namespace API_Payments.Autentication.DTO
{
        public class TokenDTO
        {
            public string? token { get; set; }
            public string? tokenType { get; set; }
            public int exoiresIn { get; set; }
        }
}
