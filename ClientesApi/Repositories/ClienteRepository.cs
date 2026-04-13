using ClientesApi.Models;

namespace ClientesApi.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly List<Cliente> _clientes = new();
    private int _nextId = 1;

    public IEnumerable<Cliente> GetAll() => _clientes;

    public Cliente? GetById(int id) =>
        _clientes.FirstOrDefault(c => c.Id == id);

    public Cliente Add(Cliente cliente)
    {
        cliente.Id = _nextId++;
        _clientes.Add(cliente);
        return cliente;
    }

    public Cliente? Update(int id, Cliente cliente)
    {
        var existing = _clientes.FirstOrDefault(c => c.Id == id);
        if (existing is null) return null;

        existing.Nombre = cliente.Nombre;
        existing.Apellido = cliente.Apellido;
        existing.Direccion = cliente.Direccion;
        return existing;
    }

    public bool Delete(int id)
    {
        var cliente = _clientes.FirstOrDefault(c => c.Id == id);
        if (cliente is null) return false;

        _clientes.Remove(cliente);
        return true;
    }
}
