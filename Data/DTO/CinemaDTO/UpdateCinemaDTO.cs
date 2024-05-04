using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTO.CinemaDTO
{
  public class UpdateCinemaDTO
  {
    [Required(ErrorMessage = "O campo de nome é obrigatório")]
    public string Nome { get; set; }
  }
}
