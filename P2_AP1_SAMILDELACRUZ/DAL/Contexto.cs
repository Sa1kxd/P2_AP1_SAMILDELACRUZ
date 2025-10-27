namespace P2_AP1_SAMILDELACRUZ.DAL;
using Microsoft.EntityFrameworkCore;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }

}
