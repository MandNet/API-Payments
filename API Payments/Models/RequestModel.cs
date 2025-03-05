using Microsoft.EntityFrameworkCore;

namespace API_Payments.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Status), nameof(ProcessorCode))]
    public class RequestModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Card { get; set; }
        public decimal Value { get; set; }
        public int Status { get; set; }
        public string? Motive { get; set; }
        public string? ProcessorCode { get; set; }
    }
}
