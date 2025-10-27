using Microsoft.EntityFrameworkCore;
using P2_AP1_SAMILDELACRUZ.DAL;
using P2_AP1_SAMILDELACRUZ.Models;
using System.Linq.Expressions;
namespace P2_AP1_SAMILDELACRUZ.Services;

public class RegistroServices(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<List<Registro>> Listar(Expression<Func<Registro, bool>> criterio)
    {
        using var context = await DbFactory.CreateDbContextAsync();
        return await context.Registro.
            Where(criterio).
            AsNoTracking().
            ToListAsync();
    }
}
