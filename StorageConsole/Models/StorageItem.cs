namespace StorageConsole.Models;

public abstract class StorageItem
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
}