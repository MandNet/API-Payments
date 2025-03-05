using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Payments.Models
{
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Card_Id), nameof(Date), IsDescending = [false, true])]
    public class TransactionModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Card_Id { get; set; }
        public string? AuthorizationCode { get; set; }
        public decimal Value { get; set; }
        public int Fee_Id { get; set; }
        public decimal Total { get; set; }
        public int Request_Id { get; set; }
    }
}

