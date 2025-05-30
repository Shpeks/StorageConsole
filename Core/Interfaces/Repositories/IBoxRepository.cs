using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IBoxRepository
{
    Task CreateAsync(BoxDto dto);
    Task<List<BoxDto>> GetListByPalletIdAsync(Guid id);
}