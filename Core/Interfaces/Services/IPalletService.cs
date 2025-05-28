using Core.Models;

namespace Core.Interfaces.Services;

public interface IPalletService
{
    Task CreateAsync(PalletDto dto);
    Task UpdateAsync(PalletDto dto);
    Task<List<PalletDto>> GetSecondMethodAsync();
    Task GetFirstMethodAsync();
}