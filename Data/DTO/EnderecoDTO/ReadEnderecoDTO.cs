using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTO.EnderecoDTO;

public class ReadEnderecoDTO
{
  public int Id { get; set; }
  public string Cep { get; set; }
  public string Logradouro { get; set; }
  public string Bairro { get; set; }
  public string Localidade { get; set; }
  public string UF { get; set; }
  public string Numero { get; set; }
}
