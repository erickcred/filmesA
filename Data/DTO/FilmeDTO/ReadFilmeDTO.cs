﻿using FilmesApi.Data.DTO.SessaoDTO;

namespace FilmesApi.Data.DTO.FilmeDTO;

public class ReadFilmeDTO
{
  public int Id { get; set; }
  public string Titulo { get; set; }
  public string Genero { get; set; }
  public int Duracao { get; set; }
  public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
  public ICollection<ReadSessaoDTO> Sessoes { get; set; }
}
