using System.ComponentModel.DataAnnotations;

namespace P2_AP1_SAMILDELACRUZ.Models;

public class Pedidos
{
    [Key]
    public int PedidoId { get; set; }

    [Required(ErrorMessage ="Fecha requerida")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "Nombre requerido")]
    public string NombreCliente { get; set; }
    [Required(ErrorMessage ="Total requerido")]
    public double Total {  get; set; }

    public ICollection<PedidosDetalle> PedidosDetalle = new List<PedidosDetalle>();

}
