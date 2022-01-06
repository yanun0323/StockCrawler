using System.Diagnostics;

namespace StockTextCrawler.Crawler;

public static class InstitutionCrawler
{
    private static readonly HttpClient client = new();

    public static void CrawlDate(DateTime target, Queue<DateTime> error)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        try
        {
            Console.WriteLine($"=========={target:yyyy/MM/dd}==========");
            string url = $"https://www.twse.com.tw/fund/T86?response=json&date=" + $"{target:yyyyMMdd}" + "&selectType=ALL";
            Console.WriteLine($"   - Send request: " + url);

            string? content = client.GetStringAsync(url).Result;

            if (content == null)
            {
                Console.WriteLine($"   - Error!!!");
                error.Enqueue(target);
                error.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution);
                Console.WriteLine($"   - Error Saved!");
                Trace.WriteLine($"[Error]{target:yyyyMMdd}");
            }
            else
            {
                content.SaveText(CrawlerPath.Path_Raw_Institution, $"{target:yyyyMMdd}");
                Console.WriteLine($"   - Data Saved!");
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"   - Error!!!");
            error.Enqueue(target);
            error.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution);
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
            target.SaveJson(CrawlerPath.Path_Raw_Root,CrawlerPath.Name_UpdateTimeName_Institution);

            Thread.Sleep(2500);
            target = target.AddDays(1);
        }
        Console.WriteLine($"========== Error ==========");
        Console.WriteLine($"   - Error Count:{error.Count()}");
        error.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution);
        Console.WriteLine($"   - Error Saved!");
    }


}