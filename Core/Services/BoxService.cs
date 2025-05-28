using Core.Factories;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Core.Services;

public class BoxService : IBoxService
{
    private readonly IBoxRepository _boxRepository;
    private readonly IPalletRepository _palletRepository;

    public BoxService(IBoxRepository boxRepository, IPalletRepository palletRepository)
    {
        _boxRepository = boxRepository;
        _palletRepository = palletRepository;
    }
    
    public async Task CreateAsync(BoxDto dto)
    {
        dto.Volume = dto.Width * dto.Height * dto.Depth;
        
        var pallet = await _palletRepository.GetByIdAsync(dto.PalletId);
        if (pallet == null)
            throw new InvalidOperationException("Указанная палета не найдена.");

        if (dto.Width > pallet.Width || dto.Depth > pallet.Depth)
            throw new InvalidOperationException("Размеры коробки превышают размеры паллеты по ширине или глубине");
        
        BoxDto box;
        
        if (dto.ExpirationDate is not null && dto.ProductionDate is null)
        {
            box = BoxFactory.CreateBoxWithExpirationDate(dto);
        }
        else if (dto.ProductionDate is not null && dto.ExpirationDate is null)
        {
            box = BoxFactory.CreateBoxWithProductDate(dto);
        }
        else if (dto.ExpirationDate is not null && dto.ProductionDate is not null)
        {
            box = BoxFactory.CreateBoxWithBothDates(dto);
        }
        else
        {
            throw new InvalidOperationException("Нужно указать хотя бы одну дату.");
        }
        
        await _boxRepository.CreateAsync(box);
    }
}