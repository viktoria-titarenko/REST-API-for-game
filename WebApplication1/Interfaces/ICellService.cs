using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface ICellService
{
    Task<string> ProcessAsync(Cell cell, CancellationToken cancellationToken);
    Task<string> IsKeyCorrectAsync(Cell cell, CancellationToken cancellationToken);
}