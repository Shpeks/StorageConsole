using Core.Factories;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Core.Services;

/// <summary>
/// Сервис для работы с коробками
/// </summary>
public class BoxService : IBoxService
{
    private readonly IBoxRepository _boxRepository;
    private readonly IPalletRepository _palletRepository;

    public BoxService(IBoxRepository boxRepository, IPalletRepository palletRepository)
    {
        _boxRepository = boxRepository;
        _palletRepository = palletRepository;
    }
    
    /// <summary>
    /// Создает коробку с учетом размеров и даты производства/годности
    /// Вычисляет объем, валидирует размеры и вызывает соответствующий фабричный метод
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если паллета не найдена, размеры превышают допустимые или не указана ни одна дата
    /// </exception>
    public async Task CreateAsync(BoxDto dto)
    {
        dto.Volume = dto.Width * dto.Height * dto.Depth;
        
        var pallet = await _palletRepository.GetByIdAsync(dto.PalletId);
        
        if (pallet == null)
            throw new InvalidOperationException("Указанная палета не найдена.");

        if (dto.Width > pallet.Width || dto.Depth > pallet.Depth)
            throw new InvalidOperationException("Размеры коробки превышают размеры паллеты по ширине или глубине");

        var box = CreateBoxFromDto(dto);
        
        await _boxRepository.CreateAsync(box);
    }
    
    /// <summary>
    /// Определяет, какой фабричный метод использовать для создания коробки на основе дат
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Если не указана ни одна из дат
    /// </exception>
    private BoxDto CreateBoxFromDto(BoxDto dto)
    {
        if (dto.ExpirationDate is not null && dto.ProductionDate is null)
            return BoxFactory.CreateBoxWithExpirationDate(dto);
    
        if (dto.ProductionDate is not null && dto.ExpirationDate is null)
            return BoxFactory.CreateBoxWithProductDate(dto);
    
        if (dto.ExpirationDate is not null && dto.ProductionDate is not null)
            return BoxFactory.CreateBoxWithBothDates(dto);

        throw new InvalidOperationException("Нужно указать хотя бы одну дату.");
    }
}