﻿using Microsoft.EntityFrameworkCore;

namespace Phone_Forecast.Models.DbContexts
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
