using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace StockCrawler;

public class InstitutionCrawler
{
    [JsonIgnore]
    private static readonly HttpClient client = new();
    public string date { get; set; } = "";
    public List<string> fields { get; set; } = new();
    public List<List<string>> data { get; set; } = new();

    private static InstitutionCrawler? Crawl(DateTime target, List<DateTime> error)
    {
        InstitutionCrawler? WebsiteData;
        try
        {
            string url = $"https://www.twse.com.tw/fund/T86?response=json&date=" + $"{target:yyyyMMdd}" + "&selectType=ALL";
            Console.WriteLine($"   - Send request: " + url);
            WebsiteData = HttpClientJsonExtensions.GetFromJsonAsync<InstitutionCrawler?>(client, url).Result;
            Console.WriteLine($"   - Request done");
            return WebsiteData;
        }
        catch (Exception e)
        {
            error.Add(target);
            Console.WriteLine($"   - Error!!!");
            Trace.WriteLine($"[Error]{target:yyyyMMdd}");
            Trace.WriteLine($"[Error]{e}");
            return null;
        }
    }
    public static Queue<InstitutionCrawler> CrawlDate(out List<DateTime> error, DateTime? begin = null, DateTime? end = null)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        DateTime target = begin ?? new(2012, 5, 2);
        DateTime now = end ?? DateTime.Now;

        InstitutionCrawler? WebsiteData;
        Queue<InstitutionCrawler> WebsiteDataQueue = new();
        error = new();

        while (target < now)
        {
            Console.WriteLine($"=========={target:yyyy/MM/dd}==========");
            WebsiteData = InstitutionCrawler.Crawl(target, error);
            if (WebsiteData == null || WebsiteData.date == "" || !WebsiteData.data.Any())
            {
                Console.WriteLine($"   - No Data");
                Console.WriteLine($"   - 沒有資料: {target:yyyy/MM/dd}");
                Trace.WriteLine($"   - 沒有資料: {target:yyyy/MM/dd}");
            }
            else 
            {
                WebsiteDataQueue.Enqueue(WebsiteData);
            }


            Thread.Sleep(2500);
            target = target.AddDays(1);

        }

        return WebsiteDataQueue;
    }


}