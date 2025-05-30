using Core.Models;
using DAL.Entity;

namespace Core.Interfaces.Factories;

public interface IBoxFactory
{
    /// <summary>
    /// Определяет, какой фабричный метод использовать для создания коробки на основе дат
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Если не указана ни одна из дат
    /// </exception>
    Box CreateBox(BoxDto dto);
}