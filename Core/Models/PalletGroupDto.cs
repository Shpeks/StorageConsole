namespace Core.Models;

public class PalletGroupDto
{
    /// <summary>
    /// Срок годности: минимальный срок среди всех коробок
    /// </summary>
    public DateTime ExpirationDate { get; set; }
    
    public List<Pallets> Pallets { get; set; }
}

public class Pallets
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Общий вес: сумма весов всех коробок + вес палеты (30 кг)
    /// </summary>
    public double TotalWeight { get; set; }
}