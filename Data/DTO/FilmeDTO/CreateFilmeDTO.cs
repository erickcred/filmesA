using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTO.FilmeDTO;

public class CreateFilmeDTO
{
    [Required(ErrorMessage = "O Título deve ser preenchido")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O Genero deve ser preechido")]
    [StringLength(50, ErrorMessage = "O tamanho do genero não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "A Duração dever ser preechida")]
    [Range(70, 600, ErrorMessage = "A duração deve ter entre 70 e 600 minutos")]
    public int Duracao { get; set; }
}
