using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2_AP1_SAMILDELACRUZ.Models;

public class PedidosDetalle
{
    [Key]
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ComponenteId { get; set; }
    public int Cantidad { get; set; }
    public Double Precio { get; set; }

    [ForeignKey("PedidoId")]
    //[InverseProperty("PedidosDetalle")]
    public virtual Pedidos Pedidos { get; set; }

    [ForeignKey("ComponenteId")]
    //[InverseProperty("ComponentesDetalle")]
    public virtual Componente ComponenteDetalle { get; set; }

}
