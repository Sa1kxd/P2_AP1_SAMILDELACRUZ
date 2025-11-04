using Microsoft.EntityFrameworkCore;
using P2_AP1_SAMILDELACRUZ.DAL;
using P2_AP1_SAMILDELACRUZ.Models;
using System.Linq.Expressions;


namespace P2_AP1_SAMILDELACRUZ.Services
{
    public class ComponenteService(IDbContextFactory<Contexto> DbFactory)
    {
       

        private enum TipoOperacion
        {
            Suma = 1,
            Resta = 2
        }


        public async Task<bool> Existe(int ComponentesId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Componentes.AnyAsync(C => C.ComponenteId == ComponentesId);
        }

        public async Task<bool> Insertar(Pedidos pedido)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Pedidos.Add(pedido);
            await AfectarExistencia(pedido.PedidosDetalle.ToArray(), TipoOperacion.Suma);
            return await contexto.SaveChangesAsync() > 0;
        }

        private async Task AfectarExistencia(PedidosDetalle[] detalles, TipoOperacion tipoOperacion)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            foreach (var item in detalles)
            {
                var componente = await contexto.Componentes.SingleAsync(c => c.ComponenteId == item.ComponenteId);

                if (tipoOperacion == TipoOperacion.Suma)
                {
                    componente.Existencia += item.Cantidad;
                }
                else
                {
                    componente.Existencia -= item.Cantidad;
                }
            }

            await contexto.SaveChangesAsync();
        }

        private async Task<bool> Modificar(Pedidos pedido)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            var detallesViejos = await contexto.pedidosDetalles
                .AsNoTracking()
                .Where(p => p.PedidoId == pedido.PedidoId)
                .ToArrayAsync();

            await AfectarExistencia(detallesViejos, TipoOperacion.Resta);

            await contexto.pedidosDetalles
                .Where(p => p.PedidoId == pedido.PedidoId)
                .ExecuteDeleteAsync();

            contexto.Pedidos.Update(pedido);

            await AfectarExistencia(pedido.PedidosDetalle.ToArray(), TipoOperacion.Suma);

            return await contexto.SaveChangesAsync() > 0;
        }


        public async Task<bool> Guardar(Pedidos pedido)
        {
            if (!await Existe(pedido.PedidoId))
            {
                return await Insertar(pedido);
            }
            else
            {
                return await Modificar(pedido);
            }
        }

        public async Task<bool> Eliminar(int ComponenteId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var componente = await contexto.Componentes
                .Include(e => e.ComponentesDetalle)
                .FirstOrDefaultAsync(e => e.ComponenteId == ComponenteId);

            if (componente == null) return false;

            await AfectarExistencia(componente.ComponentesDetalle.ToArray(), TipoOperacion.Resta);

            contexto.pedidosDetalles.RemoveRange(componente.ComponentesDetalle);
            contexto.Componentes.Remove(componente);

            var cantidad = await contexto.SaveChangesAsync();
            return cantidad > 0;
        }
    /*
        public async Task<PedidosDetalle> Buscar(int componenteId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Componentes.Include(c => c.ComponentesDetalle)
                    .ThenInclude(p => p.Pedidos)
                    .AsNoTracking().FirstOrDefaultAsync(c => c.ComponenteId == componenteId);
        }
    */
        public async Task<List<Componente>> Listar(Expression<Func<Componente, bool>> criterio)
        {
            using var context = await DbFactory.CreateDbContextAsync();
            return await context.Componentes.
                Where(criterio).
                AsNoTracking().
                ToListAsync();
        }

        public async Task<List<Pedidos>> ListarPedidos(Expression<Func<Pedidos, bool>> criterio)
        {
            using var context = await DbFactory.CreateDbContextAsync();
            return await context.Pedidos.
                Where(criterio).
                AsNoTracking().
                ToListAsync();
        }
    }
}
