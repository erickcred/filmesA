using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO.CinemaDTO;
using FilmesApi.Data.DTO.SessaoDTO;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SessaoController : ControllerBase
{
  private readonly FilmeContext _context;
  private readonly IMapper _autoMapper;

  public SessaoController(FilmeContext context, IMapper autoMapper)
  {
    _context = context;
    _autoMapper = autoMapper;
  }

  [HttpPost]
  [ProducesResponseType(typeof(ReadSessaoDTO), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AdicionarSessao([FromBody] CreateSessaoDTO sessaoDto)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Sessao sessao = _autoMapper.Map<Sessao>(sessaoDto);
      _context.Sessoes.Add(sessao);
      _context.SaveChanges();
      transaction.Commit();
      return CreatedAtAction(nameof(RetornaSessao), new { id = sessao.Id }, sessao);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<ReadSessaoDTO>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IEnumerable<ReadSessaoDTO> RetornaSessoes()
  {
    var sessao = _context.Sessoes.ToList();

    return _autoMapper.Map<List<ReadSessaoDTO>>(sessao);
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(ReadSessaoDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult RetornaSessao([FromRoute] int id)
  {
    Sessao sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
    if (sessao == null) return NotFound();

    var sessaoDto = _autoMapper.Map<ReadSessaoDTO>(sessao);
    return Ok(sessaoDto);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(typeof(ReadCinemaDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AtualizarSessao([FromRoute] int id, [FromBody] UpdateSessaoDTO sessaoDto)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Sessao sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
      if (sessao == null) return NotFound();

      _autoMapper.Map(sessao, sessaoDto);
      _context.Sessoes.Update(sessao);
      _context.SaveChanges();
      transaction.Commit();

      return Ok(_autoMapper.Map<ReadSessaoDTO>(sessao));
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpPatch("{id}")]
  [ProducesResponseType(typeof(ReadCinemaDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AtualizarSessaoParcial([FromRoute] int id, JsonPatchDocument<UpdateSessaoDTO> path)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Sessao sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
      if (sessao == null) return NotFound();

      var sessaoParaAtualizar = _autoMapper.Map<UpdateSessaoDTO>(sessao);
      path.ApplyTo(sessaoParaAtualizar, ModelState);

      if (!TryValidateModel(sessaoParaAtualizar)) return ValidationProblem();

      _autoMapper.Map(sessaoParaAtualizar, sessao);
      _context.Sessoes.Update(sessao);
      _context.SaveChanges();
      transaction.Commit();
      return Ok(_autoMapper.Map<ReadSessaoDTO>(sessao));
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpDelete("{id}")]
  [ProducesResponseType(typeof(ReadCinemaDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult DeletarSessao([FromRoute] int id)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Sessao sessao = _context.Sessoes.FirstOrDefault(s => s.Id == id);
      if (sessao == null) return NotFound();

      _context.Sessoes.Remove(sessao);
      _context.SaveChanges();
      transaction.Commit();
      var sessaoDto = _autoMapper.Map<ReadSessaoDTO>(sessao);
      return Ok(sessaoDto);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }
}
