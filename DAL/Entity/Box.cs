namespace DAL.Entity;

public class Box 
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
    /// Дата производства (если указана, используется для расчёта срока годности)
    /// </summary>
    public DateTime? ProductionDate { get; init; }
    
    /// <summary>
    /// Срок годности
    /// </summary>
    public required DateTime ExpirationDate { get; init; }
    
    /// <summary>
    /// Вес в кг
    /// </summary>
    public double Weight { get; set; }
    
    /// <summary>
    /// Объем в куб. см
    /// </summary>
    public double Volume { get;set; }
    
    public Guid PalletId { get; set; }
    public Pallet Pallet { get; set; }
}