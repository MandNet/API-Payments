using System.Text.Json.Serialization;

namespace API_Payments.DTO
{
    public class ApiSettingsDTO
    {
        public string? Ambiente { get; set; }
        public string? BaseUrl { get; set; }
        public int Timeout { get; set; }
        public int Generator { get; set; }
        public Fraud fraud { get; set; }
    }

    public class ConnectionStrings
    {
        public string? Default { get; set; }
    }

    public class Logging
    {
        public LogLevel? LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string? Default { get; set; }

        [JsonPropertyName("Microsoft.AspNetCore")]
        public string? MicrosoftAspNetCore { get; set; }
    }

    public class Fraud
    {
        public int Interval { get; set; }
    }

    public class jwtToken
    {
        public string? secretkey { get; set; }
        public string? issuer { get; set; }
        public string? audience { get; set; }
    }

    public class ApiSettingsRoot
    {
        public Logging? Logging { get; set; }
        public string? AllowedHosts { get; set; }
        public ApiSettingsDTO? ApiSettings { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }
        public jwtToken? jwt { get; set; }
        public List<Security>? Security { get; set; }
        public Fraud? fraud { get; set; }
    }

    public class Security
    {
        public string? id { get; set; }
        public string? token { get; set; }
    }
}
