using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTO.CinemaDTO;
using FilmesApi.Data.DTO.EnderecoDTO;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EnderecoController : ControllerBase
{
  private readonly FilmeContext _context;
  private readonly IMapper _autoMapper;

  public EnderecoController(FilmeContext context, IMapper autoMapper)
  {
    _context = context;
    _autoMapper = autoMapper;
  }

  [HttpPost]
  [ProducesResponseType(typeof(ReadEnderecoDTO), StatusCodes.Status201Created)]
  public IActionResult AdicionarEndereco([FromBody] CreateEnderecoDTO enderecoDto)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Endereco endereco = _autoMapper.Map<Endereco>(enderecoDto);
      _context.Enderecos.Add(endereco);
      _context.SaveChanges();
      transaction.Commit();
      return CreatedAtAction(nameof(RetornaEndereco), new { id = endereco.Id }, endereco);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<ReadEnderecoDTO>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IEnumerable<ReadEnderecoDTO> RetornaEnderecos()
  {
    return _autoMapper.Map<List<ReadEnderecoDTO>>(
      _context.Enderecos.AsNoTracking().ToList()
      );
  }

  [HttpGet("{id}")]
  [ProducesResponseType(typeof(ReadEnderecoDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult RetornaEndereco([FromRoute] int id)
  {
    Endereco endereco = _context.Enderecos.AsNoTracking().FirstOrDefault(e => e.Id == id);
    if (endereco == null) return NotFound();

    var enderecoDto = _autoMapper.Map<ReadEnderecoDTO>(endereco);
    return Ok(enderecoDto);
  }

  [HttpPut("{id}")]
  [ProducesResponseType(typeof(ReadEnderecoDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AtualizaEndereco([FromRoute] int id, UpdateEnderecoDTO enderecoDto)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Endereco endereco = _context.Enderecos.FirstOrDefault(e => e.Id == id);
      if (endereco == null) return NotFound();

      _autoMapper.Map(enderecoDto, endereco);
      _context.Enderecos.Update(endereco);
      _context.SaveChanges();

      transaction.Commit();
      return Ok(enderecoDto);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

  [HttpPatch("{id}")]
  [ProducesResponseType(typeof(ReadEnderecoDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult AtualizaEnderecoParcial([FromRoute] int id, JsonPatchDocument<UpdateEnderecoDTO> path)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Endereco endereco = _context.Enderecos.FirstOrDefault(e => e.Id == id);
      if (endereco == null) return NotFound();

      var enderecoParaAtualizar = _autoMapper.Map<UpdateEnderecoDTO>(endereco);
      if (!TryValidateModel(enderecoParaAtualizar)) return ValidationProblem(ModelState);

      _autoMapper.Map(enderecoParaAtualizar, endereco);
      _context.Enderecos.Update(endereco);
      _context.SaveChanges();
      transaction.Commit();
      return Ok(endereco);
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }

    [HttpDelete("{id}")]
  [ProducesResponseType(typeof(ReadEnderecoDTO), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult Deletaendereco([FromRoute] int id)
  {
    var transaction = _context.Database.BeginTransaction();
    try
    {
      Endereco endereco = _context.Enderecos.FirstOrDefault(e => e.Id == id);
      if (endereco == null) return NotFound();

      _context.Enderecos.Remove(endereco);
      _context.SaveChanges();

      transaction.Commit();
      return Ok(_autoMapper.Map<ReadEnderecoDTO>(endereco));
    }
    catch (Exception ex)
    {
      transaction.Rollback();
      throw ex;
    }
  }
}
