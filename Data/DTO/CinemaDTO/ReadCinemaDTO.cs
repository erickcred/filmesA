using FilmesApi.Data.DTO.EnderecoDTO;

namespace FilmesApi.Data.DTO.CinemaDTO;

public class ReadCinemaDTO
{
  public int Id { get; set; }
  public string Nome { get; set; }
  public ReadEnderecoDTO ReadEnderecoDto { get; set;  }
}
