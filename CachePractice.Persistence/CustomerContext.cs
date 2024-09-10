using CachePractice.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CachePractice.Persistence;

public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { }
    
    public DbSet<Customer> Customers { get; set; }
}