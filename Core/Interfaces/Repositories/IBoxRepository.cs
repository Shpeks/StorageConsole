using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IBoxRepository
{
    Task<List<BoxDto>> GetAllAsync();
    Task<List<BoxDto>> GetByIdAsync(Guid id);
    Task CreateAsync(BoxDto dto);
    Task<List<BoxDto>> GetByPalletIdAsync(Guid id);
}