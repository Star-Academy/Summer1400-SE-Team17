using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    class Database : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Station> Stations { get; set; }
        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=SchoolDB;Trusted_Connection=True;");
        }
    }

    class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Station WorkStation { get; set; }
        public Employee Manager { get; set; }
    }

     class Station
    {
        [Key]
        public int StationId { get; set; }
        public string Name { get; set; }
        public Employee Manager { get; set; }
        
    }
     
     
     
     
}