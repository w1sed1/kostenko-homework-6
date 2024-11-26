using System.Text.Json;
using System.Text.Json.Serialization;

public class Book
{
    [JsonIgnore]
    public int PublishingHouseId { get; set; }

    [JsonPropertyName("Title")]
    public string Title { get; set; }

    public PublishingHouse PublishingHouse { get; set; }
}

public class PublishingHouse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}

internal class Program
{
    // Зміна назви файлу
    private const string FilePath = @"C:\Users\ahoro\source\repos\kostenko homework 6\jscon999.json";

    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            var books = await LoadBooksFromFileAsync(FilePath);

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("Книги не знайдено у файлі.");
                return;
            }

            foreach (var book in books)
            {
                Console.WriteLine($"Назва: {book.Title}, Видавництво: {book.PublishingHouse?.Name}, Адреса: {book.PublishingHouse?.Address}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
    }

    /// <summary>
    /// Асинхронно завантажує список книг із JSON-файлу.
    /// </summary>
    private static async Task<List<Book>> LoadBooksFromFileAsync(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Файл {path} не знайдено.");
        }

        await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        return await JsonSerializer.DeserializeAsync<List<Book>>(fileStream, options) ?? new List<Book>();
    }
}
