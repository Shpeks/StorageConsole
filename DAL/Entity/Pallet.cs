namespace DAL.Entity;

public class Pallet
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Ширина в см
    /// </summary>
    public double Width { get; set; }
    
    /// <summary>
    /// Высота в см
    /// </summary>
    public double Height { get; set; }
    
    /// <summary>
    /// Глубина в см
    /// </summary>
    public double Depth { get; set; }
    
    /// <summary>
    /// Список коробок, размещенных на паллете
    /// </summary>
    public ICollection<Box> Boxes { get; set; }
    
    /// <summary>
    /// Общий вес: сумма весов всех коробок + вес палеты (30 кг)
    /// </summary>
    public double TotalWeight { get; set; }
    
    /// <summary>
    /// Общий объем: объем палеты + сумма объема всех коробок
    /// </summary>
    public double TotalVolume { get; set; } 
    
    /// <summary>
    /// Срок годности: минимальный срок среди всех коробок
    /// </summary>
    public DateTime ExpirationDate { get; set; }
}