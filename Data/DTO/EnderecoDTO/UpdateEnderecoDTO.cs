using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Data.DTO.EnderecoDTO;

public class UpdateEnderecoDTO
{
  [Required(ErrorMessage = "O Cep deve ser preenchido")]
  public string Cep { get; set; }

  [Required(ErrorMessage = "O Logradouro exp(Rua, Praça) deve ser preenchido")]
  public string Logradouro { get; set; }

  [Required(ErrorMessage = "O Bairro deve ser preenchido")]
  public string Bairro { get; set; }

  [Required(ErrorMessage = "A Localidade deve ser preenchido")]
  public string Localidade { get; set; }

  [Required(ErrorMessage = "O UF deve ser preenchido")]
  public string UF { get; set; }

  [Required(ErrorMessage = "O Número deve ser preenchido")]
  public string Numero { get; set; }
}
