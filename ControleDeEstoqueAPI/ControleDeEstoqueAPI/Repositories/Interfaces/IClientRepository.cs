using ControleDeEstoqueAPI.Data.DTOs.Clients;
using ControleDeEstoqueAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IClientRepository
{
    Task<Client> AddClientAsync(ClientDTO clientDto, string userInclusion);
    Task<Client> UpdateClientAsync(int id, ClientDTO clientDto, string userChange);
    Task<Client> GetClientByIdAsync(int id);
    Task<IEnumerable<ClientDTO>> GetAllInactiveClientsAsync();
    Task<bool> DeleteClientAsync(int id);
    Task<bool> DisableClientAsync(int id, string userChange);
}
