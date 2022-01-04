namespace StockCrawler.Crawler;
public class MainCrawler
{
    public static void Run()
    {
        CrawlPrice();
        //CrawlInstitution();

        //ReCrawlPrice();
        //ReCrawlInstitution();


        static void CrawlPrice()
        {
            Console.WriteLine($"========== CrawlPrice ==========");
            Queue<DateTime> error = Extention.LoadJson<Queue<DateTime>>(FilePath.PathRoot, FilePath.NamePriceError) ?? new();

            DateTime? beginPrice = Extention.LoadJson<DateTime?>(FilePath.PathRoot, FilePath.NamePriceUpdateTime);
            if(beginPrice != null) beginPrice = beginPrice.Value.AddDays(1);
            //PriceCrawler.Crawl(begin: beginPrice, end: new(2012, 5, 2), error);
            PriceCrawler.Crawl(error, begin: new(2006, 12, 1), end: new(2012, 5, 2));
        }
        static void CrawlInstitution()
        {
            Console.WriteLine($"========== CrawlInstitution ==========");
            Queue<DateTime> error = Extention.LoadJson<Queue<DateTime>>(FilePath.PathRoot, FilePath.NameInstitutionUpdateTime) ?? new();

            DateTime? beginInstitution = Extention.LoadJson<DateTime?>(FilePath.PathRoot, FilePath.NameInstitutionUpdateTime);
            if (beginInstitution != null) beginInstitution = beginInstitution.Value.AddDays(1);

            InstitutionCrawler.Crawl(error, begin: beginInstitution);
        }
        static void ReCrawlPrice()
        {
            Console.WriteLine($"========== ReCrawlPrice ==========");
            Queue<DateTime> error = new();
            Queue<DateTime> errorPrice = Extention.LoadJson<Queue<DateTime>>(FilePath.PathRoot, FilePath.NamePriceError) ?? new();
            while (errorPrice.Any())
            {
                PriceCrawler.CrawlDate(errorPrice.Dequeue(), error);
                Thread.Sleep(2500);
            }
            Console.WriteLine($"========== Error ==========");
            Console.WriteLine($"   - Error Count:{error.Count()}");
            error.SaveJson(FilePath.PathRoot, FilePath.NamePriceError);
            Console.WriteLine($"   - Error Saved!");
        }
        static void ReCrawlInstitution()
        {
            Console.WriteLine($"========== ReCrawlInstitution ==========");
            Queue<DateTime> error = new();
            Queue<DateTime> errorInstitutionError = Extention.LoadJson<Queue<DateTime>>(FilePath.PathRoot, FilePath.NameInstitutionUpdateTime) ?? new();
            while (errorInstitutionError.Any())
            {
                PriceCrawler.CrawlDate(errorInstitutionError.Dequeue(), error);
                Thread.Sleep(2500);
            }
            Console.WriteLine($"========== Error ==========");
            Console.WriteLine($"   - Error Count:{error.Count()}");
            error.SaveJson(FilePath.PathRoot, FilePath.NameInstitutionError);
            Console.WriteLine($"   - Error Saved!");
        }
    }
}

public static class FilePath
{
    public static string PathRoot { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StockViewer\\RawData"); }
    public static string NamePriceError = "PriceError";
    public static string NameInstitutionError = "InstitutionError";
    public static string NamePriceUpdateTime = "PriceUpdateTime";
    public static string NameInstitutionUpdateTime = "InstitutionUpdateTime";
    public static string PathPrice { get => Path.Combine(PathRoot, "Price"); }
    public static string PathInstitution { get => Path.Combine(PathRoot, "Institution"); }

}
