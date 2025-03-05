using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_Payments.Models
{
    [PrimaryKey(nameof(Id))]
    public class LogModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string Before { get; set; }
        public string After { get; set; }

    }
}

