using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Interfaces;

public interface IConnectService
{
    Task<Key> GetKeyAsync(Connect connect, CancellationToken cancellationToken);
    public Task<Key> IsViewCorrectAsync(Connect connect, CancellationToken cancellationToken);
    public Task<List<Keys>> GetCountKeyGameAsync(string connect, CancellationToken cancellationToken);
}