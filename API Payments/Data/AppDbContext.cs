using Microsoft.EntityFrameworkCore;
using API_Payments.Models;

namespace API_Payments.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<CardModel> TCards { get; set; }
        public DbSet<FeeModel> TFees { get; set; }
        public DbSet<RequestModel> TRequests { get; set; }
        public DbSet<TransactionModel> TTransactions { get; set; }
        public DbSet<LogModel> TLog { get; set; }
    }
}



