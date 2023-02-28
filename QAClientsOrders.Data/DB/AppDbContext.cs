using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QAClientsOrders.Data.Models;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace QAClientsOrders.Data.DB;

public class AppDbContext : DbContext
{
    public Microsoft.EntityFrameworkCore.DbSet<Client> Clients { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        string? connectionString = config
            .GetConnectionString("ConnectionString");

        optionsBuilder
            .UseSqlServer(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasMany(c => c.Orders)
            .WithOne(o => o.Client)
            .HasForeignKey(o => o.ClientID)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Seed()
    {
        if (Clients.Any() || Orders.Any())
            return;

        Clients.Add(new Client()
        {
            FirstName = "Ivan",
            SecondName = "Ivanov",
            PhoneNum = "+996 700 123456",
            DateAdd = DateTime.Now,
            OrderAmount = 1
        });
        
        Clients.Add(new Client()
        {
            FirstName = "Vasya",
            SecondName = "Petrov",
            PhoneNum = "+996 500 741258",
            DateAdd = DateTime.Now,
            OrderAmount = 2
        });

        Orders.Add(new Order()
        {
            OrderDate = new DateTime(2022, 10, 01),
            ClientID = 1,
            Description = "Test1",
            OrderPrice = (float)100.20,
            CloseDate = DateTime.Now,
           
        });

        Orders.Add(new Order()
        {
            OrderDate = new DateTime(2022, 11, 02),
            ClientID = 2,
            Description = "Test2",
            OrderPrice = (float)100.20,
            CloseDate = DateTime.Now,
        });
        
        Orders.Add(new Order()
        {
            OrderDate = new DateTime(2022, 12, 03),
            ClientID = 2,
            Description = "Test3",
            OrderPrice = (float)100.20,
            CloseDate = DateTime.Now,
        });

        SaveChanges();
    }
}