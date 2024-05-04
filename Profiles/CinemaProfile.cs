using AutoMapper;
using FilmesApi.Data.DTO.CinemaDTO;
using FilmesApi.Models;

namespace FilmesApi.Profiles;

public class CinemaProfile : Profile
{
  public CinemaProfile()
  {
    CreateMap<CreateCinemaDTO, Cinema>();
    CreateMap<UpdateCinemaDTO, Cinema>();
    CreateMap<Cinema, UpdateCinemaDTO>();
    CreateMap<Cinema, ReadCinemaDTO>();
  }
}
