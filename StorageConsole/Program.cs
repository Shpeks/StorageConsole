using Core;
using Core.Factories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Any(arg => arg.Contains("ef", StringComparison.OrdinalIgnoreCase)))
            return;
            
        var serviceProvider = DependencyInjection.ConfigureServices();

        var palletService = serviceProvider.GetRequiredService<IPalletService>();
        var boxService = serviceProvider.GetRequiredService<IBoxService>();
        
        Console.WriteLine("Выберите задачу введя ее номер. \n1. Вывод информации. \n2. Добавление данных из Json.");
        var key = Console.ReadLine();
        if (key == "1")
        {
            Console.WriteLine("Выберите что хотите вывести. " +
                              "\n1. Сгруппированные паллеты по сроку годности, отсортированные по возрастанию срока годности, в каждой группе отсортированные по весу. " +
                              "\n2. 3 палеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.");
            
            var key2 = Console.ReadLine();
            
            if (key2 == "1")
            {
                var pallets = await palletService.GetSortedPalletAsync();
                var groupNumber = 1;
                
                foreach (var group in pallets)
                {
                    Console.WriteLine(new string('-', 40));
                    Console.WriteLine($"Группа №{groupNumber++}");
                    Console.WriteLine($"Срок годности: {group.ExpirationDate.ToShortDateString()}\n");

                    foreach (var pallet in group.Pallets)
                    {
                        Console.WriteLine($"Паллета № '{pallet.Id}' \nВес: {pallet.TotalWeight} кг.\n");
                    }
                }
            }
            else if (key2 == "2")
            {
                var pallets = await palletService.GetThreePalletAsync();
                
                Console.WriteLine("Топ-3 паллеты с коробками, у которых наибольший срок годности:");
                foreach (var pallet in pallets)
                {
                    Console.WriteLine($"ID: {pallet.Id}");
                    Console.WriteLine($"Размер (ШxВxГ): {pallet.Width} x {pallet.Height} x {pallet.Depth} см");
                    Console.WriteLine($"Общий вес: {pallet.TotalWeight} кг");
                    Console.WriteLine($"Общий объем: {pallet.TotalVolume} куб. см");
                    Console.WriteLine($"Срок годности (мин.): {pallet.ExpirationDate.ToShortDateString()}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        else if (key == "2")
        {
            var reader = new FileReaderService();
            
            var filePath = Path.Combine(AppContext.BaseDirectory, "TestData", "pallets.json");
            
            var pallets = await reader.ReadPalletsFromJsonAsync(filePath);
            var palletNumber = 1;
            
            foreach (var pallet in pallets)
            {
                var palletId = await palletService.CreateAsync(pallet);
                
                foreach (var box in pallet.Boxes)
                {
                    box.PalletId = palletId;
                    await boxService.CreateAsync(box);
                }

                await palletService.UpdateAsync(palletId);
                
                Console.WriteLine($"Добавлена паллета №{palletNumber++}");
                Console.WriteLine($"\nВсего паллет добавлено = {pallets.Count}");
            }
        }
    }
}