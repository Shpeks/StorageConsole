namespace StorageConsole.Models;

public class BoxViewModel
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
    /// Вес в кг
    /// </summary>
    public double Weight { get; set; }

    public DateTime ExpirationDate { get; set; }
    public DateTime? ProductionDate { get; set; }
}