using Core.Models;

namespace Core.Interfaces.Services;

public interface IPalletService
{
    Task<Guid> CreateAsync(PalletDto dto);
    Task UpdateAsync(Guid id);
    Task<List<PalletGroupDto>> GetSortedPalletAsync();
    Task <List<PalletDto>> GetThreePalletAsync();
}