using ControleDeEstoqueAPI.Data;
using ControleDeEstoqueAPI.Data.DTOs.Clients;
using ControleDeEstoqueAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client> AddClientAsync(ClientDTO clientDto, string userInclusion)
    {
        var client = new Client
        {
            Name = clientDto.Name,
            Email = clientDto.Email,
            UserInclusion = userInclusion,
            UserChange = userInclusion
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client> UpdateClientAsync(int id, ClientDTO clientDto, string userChange)
    {
        var existingClient = await _context.Clients.FindAsync(id);
        if (existingClient == null) return null;

        existingClient.Name = clientDto.Name;
        existingClient.Email = clientDto.Email;
        existingClient.DateTimeChange = DateTime.UtcNow;
        existingClient.UserChange = userChange;

        _context.Clients.Update(existingClient);
        await _context.SaveChangesAsync();
        return existingClient;
    }

    public async Task<Client> GetClientByIdAsync(int id)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsActive);
    }

    public async Task<IEnumerable<ClientDTO>> GetAllInactiveClientsAsync()
    {
        return await _context.Clients
            .Where(c => !c.IsActive)
            .Select(c => new ClientDTO
            {
                ClientId = c.Id,
                Name = c.Name,
                Email = c.Email
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DisableClientAsync(int id, string userChange)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return false;

        client.IsActive = true;
        client.DateTimeChange = DateTime.UtcNow;
        client.UserChange = userChange;

        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        return true;
    }
}
