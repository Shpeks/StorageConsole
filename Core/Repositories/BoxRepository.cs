using Core.Factories;
using Core.Interfaces.Factories;
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
    private readonly IBoxFactory _boxFactory;

    public BoxRepository(ApplicationDbContext context, IBoxFactory boxFactory)
    {
        _context = context;
        _boxFactory = boxFactory;
    }

    /// <summary>
    /// Получает список коробок, привязанных к указанной паллете
    /// </summary>
    /// <param name="id">Идентификатор паллеты</param>
    /// <returns>Список коробок, относящихся к паллету</returns>
    public async Task<List<BoxDto>> GetListByPalletIdAsync(Guid id)
    {
        try
        {
            var boxEntity = await _context.Boxes
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
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Ошибка {e.Message} \nЗапись {id} не найдена");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка {e.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Создает новую коробку и сохраняет в бд
    /// </summary>
    public async Task CreateAsync(BoxDto dto)
    {
        try
        {
            var boxEntity = _boxFactory.CreateBox(dto);

            await _context.Boxes.AddAsync(boxEntity);
            await _context.SaveChangesAsync();
        }
        catch (InvalidOperationException oe)
        {
            Console.WriteLine($"Ошибка {oe.Message} \nНужно указать хотя бы одну дату.");
            throw;
        }
        catch (DbUpdateException de) 
        {
            Console.WriteLine($"Ошибка {de.Message} \nОшибка при добавлении/обновлении записи");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка {e}");
            throw;
        }
    }
}