using Core.Models;
using DAL.Entity;

namespace Core.Factories;

public static class BoxFactory
{
    /// <summary>
    /// Создание коробки с передачей только даты производства (ProductDate)
    /// </summary>
    public static BoxDto CreateBoxWithProductDate(BoxDto dto)
    {
        if (dto.ProductionDate is null)
            throw new InvalidOperationException("ProductionDate должен быть указан.");

        var productionDate = DateTime.SpecifyKind(dto.ProductionDate.Value, DateTimeKind.Utc);
        var expirationDate = productionDate.AddDays(100);
        
        return new BoxDto
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
    public static BoxDto CreateBoxWithExpirationDate(BoxDto dto)
    {
        var expirationDate = DateTime.SpecifyKind(dto.ExpirationDate.Value, DateTimeKind.Utc);
        
        return new BoxDto
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
    public static BoxDto CreateBoxWithBothDates(BoxDto dto)
    {
        var expirationDate = DateTime.SpecifyKind(dto.ExpirationDate.Value, DateTimeKind.Utc);
        var productionDate = DateTime.SpecifyKind(dto.ProductionDate.Value, DateTimeKind.Utc);
        
        return new BoxDto
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