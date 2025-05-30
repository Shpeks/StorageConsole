using Core.Factories;
using Core.Interfaces.Factories;
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
    private readonly IBoxFactory _boxFactory;

    public BoxService(IBoxRepository boxRepository, IPalletRepository palletRepository, IBoxFactory boxFactory)
    {
        _boxRepository = boxRepository;
        _palletRepository = palletRepository;
        _boxFactory = boxFactory;
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
        
        await _boxRepository.CreateAsync(dto);
    }
}