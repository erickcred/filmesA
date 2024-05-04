using AutoMapper;
using FilmesApi.Data.DTO.EnderecoDTO;
using FilmesApi.Models;

namespace FilmesApi.Profiles;

public class EnderecoProfile : Profile
{
  public EnderecoProfile()
  {
    // quanto estamos criando mapeamos do dto para o model
    CreateMap<CreateEnderecoDTO, Endereco>();
    // quando estamos atualizando mapeamos do dto para o model
    CreateMap<UpdateEnderecoDTO, Endereco>();
    // quando estamos buscando mapeamos do model para o dto
    CreateMap<Endereco, ReadEnderecoDTO>();
    // quando vamos atualizar parcialmente mapeamos do model para o dto
    CreateMap<Endereco, UpdateEnderecoDTO>();
  }
}
