using System.ComponentModel.DataAnnotations;

namespace ClientesApi.Models;

public class ClienteDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(50)]
    public required string Nombre { get; set; } 

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(50)]
    public required string Apellido { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [MaxLength(100)]
    public required string Direccion { get; set; }
}