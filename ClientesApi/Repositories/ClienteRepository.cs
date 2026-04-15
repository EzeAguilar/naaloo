using ClientesApi.Models;

namespace ClientesApi.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClienteStore _store;

    public ClienteRepository(ClienteStore store)
    {
        _store = store;
    }

    public IEnumerable<Cliente> GetAll() => _store.Clientes;

    public Cliente? GetById(int id) =>
        _store.Clientes.FirstOrDefault(c => c.Id == id);

    public Cliente Add(Cliente cliente)
    {
        cliente.Id = _store.NextId++;
        _store.Clientes.Add(cliente);
        return cliente;
    }

    public Cliente? Update(int id, Cliente cliente)
    {
        var existing = _store.Clientes.FirstOrDefault(c => c.Id == id);
        if (existing is null) return null;

        existing.Nombre = cliente.Nombre;
        existing.Apellido = cliente.Apellido;
        existing.Direccion = cliente.Direccion;
        return existing;
    }

    public bool Delete(int id)
    {
        var cliente = _store.Clientes.FirstOrDefault(c => c.Id == id);
        if (cliente is null) return false;

        _store.Clientes.Remove(cliente);
        return true;
    }
}
