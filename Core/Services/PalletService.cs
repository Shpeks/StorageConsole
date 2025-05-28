using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using DAL.Entity;

namespace Core.Services;

public class PalletService : IPalletService
{
    private readonly IBoxRepository _boxRepository;
    private readonly IPalletRepository _palletRepository;
    public PalletService(IBoxRepository boxRepository, IPalletRepository palletRepository)
    {
        _boxRepository = boxRepository;
        _palletRepository = palletRepository;
    }

    public async Task CreateAsync(PalletDto dto)
    {
        await _palletRepository.CreateAsync(dto);
    }

    public async Task UpdateAsync(PalletDto dto)
    {
        var boxes = await _boxRepository.GetByPalletIdAsync(dto.Id);
        
        dto.TotalWeight = boxes.Sum(b => b.Weight) + 30;
        
        dto.TotalVolume = boxes.Sum(b => b.Volume) + (dto.Width + dto.Height + dto.Depth);
        
        dto.ExpirationDate = boxes
            .Where(b => b.ExpirationDate.HasValue)
            .Select(b => b.ExpirationDate.Value)
            .Min();
        
        await _palletRepository.UpdateAsync(dto);
    }
    
    public async Task GetFirstMethodAsync()
    {
        var pallets = await _palletRepository.GetAllAsync();
        var groupPallets = pallets
            .Where(p => p.ExpirationDate.HasValue)
            .GroupBy(p => p.ExpirationDate.Value)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                ExpirationDate = g.Key,
                Pallets = g.OrderBy(p => p.TotalWeight).ToList()
            })
            .ToList();

        foreach (var group in groupPallets)
        {
            Console.WriteLine($"Срок годности:  {group.ExpirationDate.ToString("dd.MM.yyyy")}");

            foreach (var pallet in group.Pallets)
            {
                Console.WriteLine($"Вес: {pallet.TotalWeight} кг. | Объем: {pallet.TotalVolume} куб. см.");
            }
        }
    }

    public async Task<List<PalletDto>> GetSecondMethodAsync()
    {
        return await _palletRepository.Get3PalletAsync();
    }
}