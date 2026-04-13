using ClientesApi.Models;
using ClientesApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClientesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _repository;

    public ClientesController(IClienteRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cliente>> GetAll() =>
        Ok(_repository.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<Cliente> GetById(int id)
    {
        var cliente = _repository.GetById(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public ActionResult<Cliente> Create(Cliente cliente)
    {
        var created = _repository.Add(cliente);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Cliente> Update(int id, Cliente cliente)
    {
        var updated = _repository.Update(id, cliente);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _repository.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
