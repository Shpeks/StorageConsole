using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPalletRepository
{
    Task<List<PalletDto>> GetAllAsync();
    Task<PalletDto> GetByIdAsync(Guid id);
    Task CreateAsync(PalletDto dto);
    Task UpdateAsync(PalletDto dto);
    Task<List<PalletDto>> Get3PalletAsync();
}