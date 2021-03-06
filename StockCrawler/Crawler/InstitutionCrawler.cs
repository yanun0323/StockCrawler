using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StockCrawler.Crawler;

public class InstitutionCrawler
{
    [JsonIgnore]
    private static readonly HttpClient client = new();
    public string date { get; set; } = "";
    public List<string> fields { get; set; } = new();
    public List<List<string>> data { get; set; } = new();

    public static void CrawlDate(DateTime target, Queue<DateTime> error)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        InstitutionCrawler? WebsiteData;
        try
        {
            Console.WriteLine($"=========={target:yyyy/MM/dd}==========");
            string url = $"https://www.twse.com.tw/fund/T86?response=json&date=" + $"{target:yyyyMMdd}" + "&selectType=ALL";
            Console.WriteLine($"   - Send request: " + url);
            string json = client.GetStringAsync(url).Result;
            WebsiteData = JsonSerializer.Deserialize<InstitutionCrawler?>(json);

            if (WebsiteData == null || WebsiteData.date == "" || !WebsiteData.data.Any())
            {
                Console.WriteLine($"   - No Data");
                Console.WriteLine($"   - 沒有資料: {target:yyyy/MM/dd}");
            }
            else
            {
                string pathToSave = Path.Combine(FilePath.Path_Raw_Institution, WebsiteData.date.Remove(4));
                WebsiteData.SaveJson(pathToSave, WebsiteData.date);
                Console.WriteLine($"   - Data Saved!");
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"   - Error!!!");
            error.Enqueue(target);
            error.SaveJson(FilePath.Path_Raw_Root, FilePath.Name_Error_Institution);
            Console.WriteLine($"   - Error Saved!");
            Trace.WriteLine($"[Error]{target:yyyyMMdd}");
        }
    }
    public static void Crawl(Queue<DateTime> error, DateTime? begin = null, DateTime? end = null)
    {
        DateTime target = begin ?? new(2012, 5, 2);
        DateTime now = end ?? DateTime.Now;

        while (target.AddHours(17) < now)
        {
            CrawlDate(target, error);
            target.SaveJson(FilePath.Path_Raw_Root,FilePath.Name_UpdateTimeName_Institution);

            Thread.Sleep(2500);
            target = target.AddDays(1);
        }
        Console.WriteLine($"========== Error ==========");
        Console.WriteLine($"   - Error Count:{error.Count()}");
        error.SaveJson(FilePath.Path_Raw_Root, FilePath.Name_Error_Institution);
        Console.WriteLine($"   - Error Saved!");
    }


}