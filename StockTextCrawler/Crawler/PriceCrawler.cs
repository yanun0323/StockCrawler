using System.Diagnostics;

namespace StockTextCrawler.Crawler;
public static class PriceCrawler
{
    private static readonly HttpClient client = new HttpClient();

    public static void CrawlDate(DateTime target, Queue<DateTime> error)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        try
        {
            Console.WriteLine($"=========={target:yyyy/MM/dd}==========");
            string url = $"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=json&date=" + $"{ target:yyyyMMdd}" + "&type=ALLBUT0999";
            Console.WriteLine($"   - Send request: " + url);

            string? content = client.GetStringAsync(url).Result;

            if (content == null)
            {
                RefreshError(target, error);
                SaveError(error);
            }
            else
            {
                content.SaveText(CrawlerPath.Path_Raw_Price, $"{target:yyyyMMdd}");
                Console.WriteLine($"   - Data Saved!");
            }
        }
        catch (Exception)
        {
            RefreshError(target, error);
            SaveError(error);
        }

        static void RefreshError(DateTime target, Queue<DateTime> error)
        {
            Console.WriteLine($"   - Error!!!");
            error.Enqueue(target);
            Trace.WriteLine($"   [Error] Can't catch data on {target:yyyy/MM/dd}");
        }
    }
    public static void Crawl(Queue<DateTime> error, DateTime? begin = null, DateTime? end = null)
    {
        DateTime target = begin ?? new(2004, 2, 13);
        DateTime now = end ?? DateTime.Now;

        while (target.AddHours(17) < now)
        {
            CrawlDate(target, error);
            target.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_UpdateTime_Price);

            Thread.Sleep(2500);
            target = target.AddDays(1);
        }

        Console.WriteLine($"========== Error ==========");
        Console.WriteLine($"   - Error Count:{error.Count()}");

        SaveError(error); ;
    }
    private static void SaveError(Queue<DateTime> error)
    {
        error.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Price);
        Console.WriteLine($"   - Error Saved!");
    }
}
