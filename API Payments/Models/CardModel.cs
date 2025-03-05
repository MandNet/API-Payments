using Microsoft.EntityFrameworkCore;

namespace API_Payments.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Number), IsUnique = true)]
    public class CardModel
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public decimal Balance { get; set; }
        public decimal Limit { get; set; }
        public bool Active { get; set; }
        public bool Elegible { get; set; }
        public int Status { get; set; }
    }
}
