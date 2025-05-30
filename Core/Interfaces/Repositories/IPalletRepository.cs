using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPalletRepository
{
    Task<PalletDto> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(PalletDto dto);
    Task UpdateAsync(PalletDto dto);
    Task<List<PalletDto>> GetThreePalletAsync();
    Task<List<PalletGroupDto>> GetSortedPalletAsync();
}