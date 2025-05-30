using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using DAL.Entity;

namespace Core.Services;

/// <summary>
/// Сервис для управления паллетами
/// </summary>
public class PalletService : IPalletService
{
    private readonly IBoxRepository _boxRepository;
    private readonly IPalletRepository _palletRepository;
    public PalletService(IBoxRepository boxRepository, IPalletRepository palletRepository)
    {
        _boxRepository = boxRepository;
        _palletRepository = palletRepository;
    }

    /// <summary>
    /// Создает новую паллету
    /// </summary>
    public async Task<Guid> CreateAsync(PalletDto dto)
    {
        return await _palletRepository.CreateAsync(dto);
    }
    
    /// <summary>
    /// Обновляет информацию о паллете, пересчитывая ее общий вес, объем и минимальный срок годности на основе связанных коробок
    /// </summary>
    public async Task UpdateAsync(Guid id)
    {
        var box = await _boxRepository.GetListByPalletIdAsync(id);
        var pallet = await _palletRepository.GetByIdAsync(id);
        
        pallet.TotalWeight = box.Sum(b => b.Weight) + 30;
        
        pallet.TotalVolume = box.Sum(b => b.Volume) + (pallet.Width * pallet.Height * pallet.Depth);
        
        pallet.ExpirationDate = box
            .Select(b => b.ExpirationDate.Value)
            .Min();
        
        await _palletRepository.UpdateAsync(pallet);
    }
    
    /// <summary>   
    /// Выводит в консоль список паллет, сгруппированных по сроку годности, отсортированных по дате и весу.
    /// </summary>
    public async Task<List<PalletGroupDto>> GetSortedPalletAsync()
    {
        return await _palletRepository.GetSortedPalletAsync();
    }

    /// <summary>
    /// Возвращает список из трёх паллет с определёнными критериями
    /// </summary>
    /// <returns>Список из 3 паллет</returns>
    public async Task<List<PalletDto>> GetThreePalletAsync()
    {
        return await _palletRepository.GetThreePalletAsync();
    }
}