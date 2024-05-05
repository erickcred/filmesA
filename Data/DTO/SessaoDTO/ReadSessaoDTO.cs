using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTO.SessaoDTO
{
  public class ReadSessaoDTO
  {
    public int FilmeId { get; set; }
    public int CinemaId { get; set; }
  }
}
