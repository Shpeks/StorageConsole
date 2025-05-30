using Core.Interfaces.Repositories;
using Core.Models;
using DAL.Data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

/// <summary>
/// Репозиторий для управления паллетами и получения связанных данных из бд
/// </summary>
public class PalletRepository : IPalletRepository
{
    private readonly ApplicationDbContext _context;

    public PalletRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Возвращает 3 паллеты с коробками, у которых есть срок годности,
    /// отсортированные по убыванию максимальной даты и затем по возрастанию объема
    /// </summary>
    /// <returns>Список из 3 паллет</returns>
    public async Task<List<PalletDto>> GetThreePalletAsync()
    {
        try
        {
            var topPallets = await _context.Pallets
                .Include(p => p.Boxes)
                .Select(p => new
                {
                    Pallet = p,
                    MaxExpiration = p.Boxes.Max(b => b.ExpirationDate)
                })
                .OrderByDescending(p => p.MaxExpiration)
                .Take(3)
                .OrderBy(p => p.Pallet.TotalVolume)
                .Select(p => new PalletDto
                {
                    Id = p.Pallet.Id,
                    Width = p.Pallet.Width,
                    Height = p.Pallet.Height,
                    Depth = p.Pallet.Depth,
                    TotalWeight = p.Pallet.TotalWeight,
                    TotalVolume = p.Pallet.TotalVolume,
                    ExpirationDate = p.Pallet.ExpirationDate
                })
                .ToListAsync();

            return topPallets;
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Ошибка {e.Message} \nЗапись не найдена");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Обновляет паллету в базе данных по данным из DTO.
    /// </summary>
    public async Task UpdateAsync(PalletDto dto)
    {
        try
        {
            var palletEntity = await _context.Pallets.FindAsync(dto.Id);
            if (palletEntity == null) return;

            palletEntity.Width = dto.Width;
            palletEntity.Height = dto.Height;
            palletEntity.Depth = dto.Depth;
            palletEntity.TotalWeight = dto.TotalWeight;
            palletEntity.TotalVolume = dto.TotalVolume;
            palletEntity.ExpirationDate = dto.ExpirationDate;

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine($"Ошибка при обновлении записи бд \n{e.Message}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка {e.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Получает паллету по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор паллеты.</param>
    public async Task<PalletDto> GetByIdAsync(Guid id)
    {
        try
        {
            var palletEntity = await _context.Pallets.FirstOrDefaultAsync(p => p.Id == id);

            return new PalletDto
            {
                Id = palletEntity.Id,
                Width = palletEntity.Width,
                Height = palletEntity.Height,
                Depth = palletEntity.Depth,
                TotalWeight = palletEntity.TotalWeight,
                TotalVolume = palletEntity.TotalVolume,
                ExpirationDate = palletEntity.ExpirationDate,
            };
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Ошибка: {e.Message} \nПаллета не найдена");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Создаёт новую паллету в базе данных.
    /// </summary>
    public async Task<Guid> CreateAsync(PalletDto dto)
    {
        try
        {
            var palletEntity = new Pallet
            {
                Width = dto.Width,
                Height = dto.Height,
                Depth = dto.Depth,
                TotalWeight = dto.TotalWeight,
                TotalVolume = dto.TotalVolume,
                ExpirationDate = dto.ExpirationDate,
            };
            
            await _context.Pallets.AddAsync(palletEntity);
            await _context.SaveChangesAsync();
            
            return palletEntity.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка {e.Message}");
            throw;
        }
    }

    public async Task<List<PalletGroupDto>> GetSortedPalletAsync()
    {
        var pallets = await _context.Pallets
            .Include(p => p.Boxes)
            .ToListAsync();

        var grouped = pallets
            .GroupBy(p => p.ExpirationDate.Date)
            .OrderBy(g => g.Key)
            .Select(g => new PalletGroupDto
            {
                ExpirationDate = g.Key,
                Pallets = g.OrderBy(p => p.TotalWeight)
                    .Select(p => new Pallets
                    {
                        Id = p.Id,
                        TotalWeight = p.TotalWeight,
                    }).ToList()
            })
            .ToList();

        return grouped;
    }
}