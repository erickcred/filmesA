using AutoMapper;
using FilmesApi.Data.DTO.SessaoDTO;
using FilmesApi.Models;

namespace FilmesApi.Profiles;

public class SessaoProfile : Profile
{
  public SessaoProfile()
  {
    CreateMap<CreateSessaoDTO, Sessao>();
    CreateMap<UpdateSessaoDTO, Sessao>();
    CreateMap<Sessao, UpdateSessaoDTO>();
    CreateMap<Sessao, ReadSessaoDTO>();
  }
}
