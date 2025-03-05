using Microsoft.EntityFrameworkCore;

namespace API_Payments.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Date), IsDescending = [true])]
    public class FeeModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}
