using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO.CinemaDTO;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemaController : ControllerBase
{
  private readonly FilmeContext _context;
  private readonly IMapper _autoMapper;

  public CinemaController(FilmeContext context, IMapper autoMapper)
  {
    _context = context;
    _autoMapper = autoMapper;
  }

  [HttpPost]
  [ProducesResponseType(typeof(ReadCinemaDTO), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AdicionarCinema([FromBody] CreateCinemaDTO cinemaDto)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Cinema cinema = _autoMapper.Map<Cinema>(cinemaDto);
      _context.Cinemas.Add(cinema);
      _context.SaveChanges();
      transaction.Commit();
      return CreatedAtAction(nameof(RetornaCinema), new { id = cinema.Id }, cinema);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<ReadCinemaDTO>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IEnumerable<ReadCinemaDTO> RetornaCinemas([FromQuery] int? enderecoId = null)
  {
    if (enderecoId == null)
    {
      return _autoMapper.Map<List<ReadCinemaDTO>>(
        _context.Cinemas.ToList()
        );
    }
    return _autoMapper.Map<List<ReadCinemaDTO>>(
        _context.Cinemas.Where(c => c.EnderecoId == enderecoId).ToList()
        );
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(ReadCinemaDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult RetornaCinema([FromRoute] int id)
  {
    Cinema cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
    if (cinema == null) return NotFound();

    var cinemaDto = _autoMapper.Map<ReadCinemaDTO>(cinema);

    return Ok(cinemaDto);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(typeof(ReadCinemaDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AtualizarCinema([FromRoute] int id, [FromBody] UpdateCinemaDTO cinemaDto)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Cinema cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
      if (cinema == null) return NotFound();

      _autoMapper.Map(cinemaDto, cinema);
      _context.Cinemas.Update(cinema);
      _context.SaveChanges();
      transaction.Commit();
      return Ok(cinemaDto);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpPatch("{id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AtualizarCinemaParcial([FromRoute] int id, JsonPatchDocument<UpdateCinemaDTO> path)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Cinema cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
      if (cinema == null) return NotFound();

      var cinemaParaAtualizar = _autoMapper.Map<UpdateCinemaDTO>(cinema);
      path.ApplyTo(cinemaParaAtualizar, ModelState);

      if (!TryValidateModel(cinemaParaAtualizar)) return ValidationProblem(ModelState);

      _autoMapper.Map(cinemaParaAtualizar, cinema);
      _context.Cinemas.Update(cinema);
      _context.SaveChanges();
      transaction.Commit();
      return Ok(cinema);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpDelete("{id}")]
  public IActionResult DeletaCinema([FromRoute] int id)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Cinema cinema = _context.Cinemas.FirstOrDefault(c => c.Id == id);
      if (cinema == null) return NotFound();

      _context.Cinemas.Remove(cinema);
      _context.SaveChanges();
      transaction.Commit();
      return Ok(cinema);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

}
