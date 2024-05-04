using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
  private readonly FilmeContext _context;
  private readonly SerieContext _serieContext;
  private readonly IMapper _autoMapper;

  public FilmeController(FilmeContext context, IMapper autoMapper, SerieContext serieContext)
  {
    _context = context;
    _autoMapper = autoMapper;
    _serieContext = serieContext;
  }

  /// <summary>
  /// Adiciona um filme ao banco de dados
  /// </summary>
  /// <param name="filmeDto">Objeto com os campos necessário para a criação de um filme</param>
  /// <returns>IActionResult</returns>
  /// <response code="201">Caso inserção seja feita com sucesso</response>
  [HttpPost]
  [ProducesResponseType(typeof(Filme), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AdicionaFilme([FromBody] CreateFilmeDTO filmeDto)
  {
    using var transaction = _context.Database.BeginTransaction();
    try
    {
      Filme filme = _autoMapper.Map<Filme>(filmeDto);
      _context.Filmes.Add(filme);
      _context.SaveChanges();
      transaction.Commit();
      return CreatedAtAction(nameof(PegarFilme), new { id = filme.Id }, filme);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      return BadRequest(ex.Message);
    }
  }

  [HttpGet]
  [ProducesResponseType(typeof(Filme), StatusCodes.Status200OK)]
  public IEnumerable<ReadFilmeDTO> PegarFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
  {
    return _autoMapper
      .Map<List<ReadFilmeDTO>>(
        _context.Filmes
          .AsNoTracking()
          .Skip(skip)
          .Take(take)
          .ToList()
        );
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(Filme), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public IActionResult PegarFilme([FromRoute] int id)
  {
    Filme filme = _context.Filmes.AsNoTracking().FirstOrDefault(f => f.Id == id);
    var filmeDto = _autoMapper.Map<ReadFilmeDTO>(filme);
    return filme != null ? Ok(filmeDto) : NotFound();
  }

  [HttpPut("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public IActionResult AtualizaFilme([FromRoute] int id, [FromBody] UpdateFilmeDTO filmeDTO)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Filme filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
      if (filme == null) NotFound();

      filme = _autoMapper.Map(filmeDTO, filme);

      _context.Filmes.Update(filme);
      _context.SaveChanges();
      transaction.Commit();
      //return NoContent();
      return Ok(filme);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public IActionResult AtualizaFilmeParcial([FromRoute] int id, JsonPatchDocument<UpdateFilmeDTO> patch)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Filme filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
      if (filme == null) NotFound();

      var filmeParaAtualizar = _autoMapper.Map<UpdateFilmeDTO>(filme);
      patch.ApplyTo(filmeParaAtualizar, ModelState);

      if (!TryValidateModel(filmeParaAtualizar)) return ValidationProblem(ModelState);

      _autoMapper.Map(filmeParaAtualizar, filme);
      _context.SaveChanges();
      transaction.Commit();
      return Ok(filme);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public IActionResult DeletaFilme([FromRoute] int id)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Filme filme = _context.Filmes.FirstOrDefault(f => f.Id == id);
      if (filme == null) NotFound();

      _context.Filmes.Remove(filme);
      _context.SaveChanges();
      transaction.Commit();
      //return NoContent();
      return Ok(filme);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }















  //[HttpPost("Serie")]
  //[ProducesResponseType(typeof(Filme), StatusCodes.Status201Created)]
  //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
  //public IActionResult AdicionaSerie([FromBody] Serie serie)
  //{
  //  using var transaction = _context.Database.BeginTransaction();
  //  try
  //  {
  //    _serieContext.Serie.Add(serie);
  //    _context.SaveChanges();
  //    transaction.Commit();
  //    return CreatedAtAction(nameof(PegarFilme), new { id = serie.Id }, serie);
  //  }
  //  catch (Exception ex)
  //  {
  //    transaction.Rollback();
  //    throw ex;
  //  }
  //}
}
