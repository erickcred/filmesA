using FilmesApi.Data.DTO.EnderecoDTO;
using FilmesApi.Data.DTO.SessaoDTO;

namespace FilmesApi.Data.DTO.CinemaDTO;

public class ReadCinemaDTO
{
  public int Id { get; set; }
  public string Nome { get; set; }
  public ReadEnderecoDTO Endereco { get; set; }
  public ICollection<ReadSessaoDTO> Sessoes { get; set; }
}
