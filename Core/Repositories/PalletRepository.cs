using Core.Interfaces.Repositories;
using Core.Models;
using DAL.Data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class PalletRepository : IPalletRepository
{
    private readonly ApplicationDbContext _context;

    public PalletRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<PalletDto>> Get3PalletAsync()
    {
        try
        {
            var topPallets = await _context.Pallets
                .Include(p => p.Boxes)
                .Where(p => p.Boxes.Any(b => b.ExpirationDate.HasValue))
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
        catch (Exception e)
        {
            Console.WriteLine("Ошибка: " + e.Message);
            throw;
        }
    }
    public async Task<List<PalletDto>> GetAllAsync()
    {
        try
        {
            var palletEntity = await _context.Pallets
                .Include(b => b.Boxes)
                .ToListAsync();

            return palletEntity.Select(p => new PalletDto
            {
                Id = p.Id,
                Width = p.Width,
                Height = p.Height,
                Depth = p.Depth,
                TotalWeight =  p.TotalWeight,
                TotalVolume = p.TotalVolume,
                ExpirationDate = p.ExpirationDate,
            }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e}");
            throw;
        }
    }

    public async Task UpdateAsync(PalletDto dto)
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
    public async Task<PalletDto> GetByIdAsync(Guid id)
    {
        try
        {
            var palletEntity = await _context.Pallets.FirstOrDefaultAsync(p => p.Id == id);

            return new PalletDto
            {
                Width = palletEntity.Width,
                Height = palletEntity.Height,
                Depth = palletEntity.Depth,
                TotalWeight = palletEntity.TotalWeight,
                TotalVolume = palletEntity.TotalVolume,
                ExpirationDate = palletEntity.ExpirationDate,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e}");
            throw;
        }
    }

    public async Task CreateAsync(PalletDto dto)
    {
        await using var tr = await _context.Database.BeginTransactionAsync();
        try
        {
            var palletEntity = new Pallet
            {
                Id = dto.Id,
                Width = dto.Width,
                Height = dto.Height,
                Depth = dto.Depth,
                TotalWeight = dto.TotalWeight,
                TotalVolume = dto.TotalVolume,
                ExpirationDate = dto.ExpirationDate,
            };
            
            await _context.Pallets.AddAsync(palletEntity);
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