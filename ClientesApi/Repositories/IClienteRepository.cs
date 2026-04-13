using ClientesApi.Models;

namespace ClientesApi.Repositories;

public interface IClienteRepository
{
    IEnumerable<Cliente> GetAll();
    Cliente? GetById(int id);
    Cliente Add(Cliente cliente);
    Cliente? Update(int id, Cliente cliente);
    bool Delete(int id);
}
