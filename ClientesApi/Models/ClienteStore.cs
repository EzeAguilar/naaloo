namespace ClientesApi.Models;

public class ClienteStore
{
    public List<Cliente> Clientes { get; } = new();
    public int NextId { get; set; } = 1;
}
