using Core.Interfaces.Factories;
using Core.Models;
using DAL.Entity;

namespace Core.Factories;

public class BoxFactory : IBoxFactory
{
    /// <summary>
    /// Определяет, какой фабричный метод использовать для создания коробки на основе дат
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Если не указана ни одна из дат
    /// </exception>
    public Box CreateBox(BoxDto dto)
    {
        switch (dto.ProductionDate, dto.ExpirationDate)
        {
            case (null, not null):
                return BoxFactory.CreateBoxWithExpirationDate(dto);
            case (not null, null):
                return BoxFactory.CreateBoxWithProductDate(dto);
            case (not null,not null):
                return BoxFactory.CreateBoxWithBothDates(dto);
            default:
                throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Создание коробки с передачей только даты производства (ProductDate)
    /// </summary>
    private static Box CreateBoxWithProductDate(BoxDto dto)
    {
        if (dto.ProductionDate is null)
            throw new InvalidOperationException("ProductionDate должен быть указан.");

        var productionDate = DateTime.SpecifyKind(dto.ProductionDate.Value, DateTimeKind.Utc);
        var expirationDate = productionDate.AddDays(100);
        
        return new Box
        {
            Width = dto.Width,
            Weight = dto.Weight,
            Height = dto.Height,
            Depth = dto.Depth,
            Volume = dto.Volume,
            PalletId = dto.PalletId,
            ProductionDate = productionDate, 
            ExpirationDate = expirationDate
        };
    }
    
    /// <summary>
    /// Создание коробки с передачей только срока годности (ExpirationDate)
    /// </summary>
    private static Box CreateBoxWithExpirationDate(BoxDto dto)
    {
        var expirationDate = DateTime.SpecifyKind(dto.ExpirationDate.Value.Date, DateTimeKind.Utc);
        
        return new Box
        {
            Width = dto.Width,
            Weight = dto.Weight,
            Height = dto.Height,
            Depth = dto.Depth,
            Volume = dto.Volume,
            PalletId = dto.PalletId,
            ProductionDate = dto.ProductionDate, 
            ExpirationDate = expirationDate,
        };
    }
    
    /// <summary>
    /// Создание коробки имея обе даты (ProductDate и ExpirationDate)
    /// </summary>
    private static Box CreateBoxWithBothDates(BoxDto dto)
    {
        var expirationDate = DateTime.SpecifyKind(dto.ExpirationDate.Value.Date, DateTimeKind.Utc);
        var productionDate = DateTime.SpecifyKind(dto.ProductionDate.Value.Date, DateTimeKind.Utc);
        
        return new Box
        {
            Width = dto.Width,
            Weight = dto.Weight,
            Height = dto.Height,
            Depth = dto.Depth,
            Volume = dto.Volume,
            PalletId = dto.PalletId,
            ProductionDate = productionDate, 
            ExpirationDate = expirationDate,
        };
    }
}