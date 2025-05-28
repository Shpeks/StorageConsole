using System.Text.Json;
using Core.Models;

namespace Core.Services;

/// <summary>
/// Сервис для чтения дынных о палетах из json
/// </summary>
public class FileReaderService
{
    /// <summary>
    /// Считывает список паллет из json-файла по указанному пути
    /// </summary>
    /// <param name="filePath">Путь к json-файлу</param>
    /// <exception cref="FileNotFoundException">
    /// Если файл не найдет
    /// </exception>
    public async Task<List<PalletDto>> ReadPalletsFromJsonAsync(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл {filePath} не найден.");

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        var pallets = JsonSerializer.Deserialize<List<PalletDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return pallets ?? new List<PalletDto>();
    }
}