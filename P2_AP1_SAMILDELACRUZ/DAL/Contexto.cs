namespace P2_AP1_SAMILDELACRUZ.DAL;
using Microsoft.EntityFrameworkCore;
using P2_AP1_SAMILDELACRUZ.Models;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

    public DbSet<Registro> Registro { get; set; }
}
