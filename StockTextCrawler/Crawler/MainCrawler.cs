namespace StockTextCrawler.Crawler;
public class MainCrawler
{
    public static void Run()
    {
        CrawlPrice();
        CrawlInstitution();

        ReCrawlPrice();
        ReCrawlInstitution();
    }
    private static void CrawlPrice()
    {
        Console.WriteLine($"========== CrawlPrice ==========");
        Queue<DateTime> error = Extention.LoadJson<Queue<DateTime>>(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Price) ?? new();

        DateTime? beginPrice = Extention.LoadJson<DateTime?>(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_UpdateTime_Price);
        if (beginPrice != null) beginPrice = beginPrice.Value.AddDays(1);
        PriceCrawler.Crawl(error, begin: beginPrice);
    }
    private static void CrawlInstitution()
    {
        Console.WriteLine($"========== CrawlInstitution ==========");
        Queue<DateTime> error = Extention.LoadJson<Queue<DateTime>>(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution) ?? new();

        DateTime? beginInstitution = Extention.LoadJson<DateTime?>(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_UpdateTimeName_Institution);
        if (beginInstitution != null) beginInstitution = beginInstitution.Value.AddDays(1);

        InstitutionCrawler.Crawl(error, begin: beginInstitution);
    }
    private static void ReCrawlPrice()
    {
        Console.WriteLine($"========== ReCrawlPrice ==========");
        Queue<DateTime> error = new();
        Queue<DateTime> errorPrice = Extention.LoadJson<Queue<DateTime>>(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Price) ?? new();
        while (errorPrice.Any())
        {
            PriceCrawler.CrawlDate(errorPrice.Dequeue(), error);
            Thread.Sleep(2500);
            errorPrice.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Price);
        }
        Console.WriteLine($"========== Error ==========");
        Console.WriteLine($"   - Error Count:{error.Count()}");
        error.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Price);
        Console.WriteLine($"   - Error Saved!");
    }
    private static void ReCrawlInstitution()
    {
        Console.WriteLine($"========== ReCrawlInstitution ==========");
        Queue<DateTime> error = new();
        Queue<DateTime> errorInstitutionError = Extention.LoadJson<Queue<DateTime>>(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution) ?? new();
        while (errorInstitutionError.Any())
        {
            PriceCrawler.CrawlDate(errorInstitutionError.Dequeue(), error);
            Thread.Sleep(2500);
            errorInstitutionError.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution);
        }
        Console.WriteLine($"========== Error ==========");
        Console.WriteLine($"   - Error Count:{error.Count()}");
        error.SaveJson(CrawlerPath.Path_Raw_Root, CrawlerPath.Name_Error_Institution);
        Console.WriteLine($"   - Error Saved!");
    }
}
