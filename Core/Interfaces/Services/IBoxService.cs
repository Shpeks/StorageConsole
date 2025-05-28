using Core.Models;

namespace Core.Interfaces.Services;

public interface IBoxService
{
    Task CreateAsync(BoxDto dto);
}