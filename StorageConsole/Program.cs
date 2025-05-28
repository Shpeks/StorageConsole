using Core;
using Core.Interfaces.Services;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = DependencyInjection.ConfigureServices();

        var palletService = serviceProvider.GetRequiredService<IPalletService>();
        var boxService = serviceProvider.GetRequiredService<IBoxService>();
        
        Console.WriteLine("Выберите задачу введя ее номер. \n1. Вывод информации. \n2. Добавление данных из Json.");
        var key = Console.ReadLine();
        if (key == "1")
        {
            Console.WriteLine("Выберите что хотите вывести. \n1. Первый метод. \n2. Второй метод.");
            
            var key2 = Console.ReadLine();
            
            if (key2 == "1")
            {
                await palletService.GetFirstMethodAsync();
            }
            else if (key2 == "2")
            {
                var pallets = await palletService.GetSecondMethodAsync();
                
                Console.WriteLine("Топ-3 паллеты с коробками, у которых наибольший срок годности:");
                foreach (var pallet in pallets)
                {
                    Console.WriteLine($"ID: {pallet.Id}");
                    Console.WriteLine($"Размер (ШxВxГ): {pallet.Width} x {pallet.Height} x {pallet.Depth} см");
                    Console.WriteLine($"Общий вес: {pallet.TotalWeight} кг");
                    Console.WriteLine($"Общий объем: {pallet.TotalVolume} куб. см");
                    Console.WriteLine($"Срок годности (мин.): {pallet.ExpirationDate?.ToShortDateString() ?? "неизвестен"}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        else if (key == "2")
        {
            Console.WriteLine("Выберите файл... ");
            var reader = new FileReaderService();

            string? filePath = ShowOpenFileDialog();
            
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Файл не выбран. Завершение работы.");
                return; 
            }
            
            if (Path.GetExtension(filePath).ToLower() != ".json")
            {
                Console.WriteLine("Ошибка: выбранный файл не является JSON-файлом. Программа завершена.");
                return;
            }
            
            var pallets = await reader.ReadPalletsFromJsonAsync(filePath);

            foreach (var pallet in pallets)
            {
                await palletService.CreateAsync(pallet);

                foreach (var box in pallet.Boxes)
                {
                    await boxService.CreateAsync(box);
                }

                await palletService.UpdateAsync(pallet);
            }

            Console.WriteLine($"Добавлены паллеты = {pallets.Count} шт");
        }
    }
    
    private static string? ShowOpenFileDialog()
    {
        string? path = null;

        var t = new Thread(() =>
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Выберите JSON файл"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName;
            }
        });

        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();

        return path;
    }
}