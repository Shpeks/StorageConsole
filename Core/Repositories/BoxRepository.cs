using Core.Interfaces.Repositories;
using Core.Models;
using DAL.Data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

/// <summary>
/// Репозиторий для работы с коробками
/// </summary>
public class BoxRepository : IBoxRepository
{
    private readonly ApplicationDbContext _context;

    public BoxRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получает список коробок, привязанных к указанной паллете
    /// </summary>
    /// <param name="id">Идентификатор паллеты</param>
    /// <returns>Список коробок, относящихся к паллете</returns>
    public async Task<List<BoxDto>> GetByPalletIdAsync(Guid id)
    {   
        var boxEntity =  await _context.Boxes
            .Where(b => b.PalletId == id)
            .ToListAsync();
        return boxEntity.Select(b => new BoxDto
        {
            Width = b.Width,
            Height = b.Height,
            Depth = b.Depth,
            Weight = b.Weight,
            Volume = b.Volume,
            ProductionDate = b.ProductionDate,
            ExpirationDate = b.ExpirationDate,
            PalletId = b.PalletId,
        }).ToList();
    }
    
    /// <summary>
    /// Создает новую коробку и сохраняет в бд
    /// </summary>
    public async Task CreateAsync(BoxDto box)
    {
        await using var tr = await _context.Database.BeginTransactionAsync();
        try
        {
            var boxEntity = new Box
            {
                Width = box.Width,
                Height = box.Height,
                Depth = box.Depth,
                Weight = box.Weight,
                Volume = box.Volume,
                ProductionDate = box.ProductionDate,
                ExpirationDate = box.ExpirationDate,
                PalletId = box.PalletId,
            };
            
            await _context.Boxes.AddAsync(boxEntity);
            await _context.SaveChangesAsync();
            
            await tr.CommitAsync();
        }
        catch (Exception e)
        {
            await tr.RollbackAsync();
            Console.WriteLine($"Ошибка {e}");
            throw;
        }
    }
}