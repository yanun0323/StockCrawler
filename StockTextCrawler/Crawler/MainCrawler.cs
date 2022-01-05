namespace StockTextCrawler.Crawler;
public class MainCrawler
{
    public static void Run()
    {
        CrawlPrice();
        //CrawlInstitution();

        ReCrawlPrice();
        //ReCrawlInstitution();


        static void CrawlPrice()
        {
            Console.WriteLine($"========== CrawlPrice ==========");
            Queue<DateTime> error = Extention.LoadJson<Queue<DateTime>>(FilePath.Path_Raw_Root, FilePath.Name_Error_Price) ?? new();

            DateTime? beginPrice = Extention.LoadJson<DateTime?>(FilePath.Path_Raw_Root, FilePath.Name_UpdateTime_Price);
            if(beginPrice != null) beginPrice = beginPrice.Value.AddDays(1);
            PriceCrawler.Crawl(error, begin: beginPrice);
        }
        static void CrawlInstitution()
        {
            Console.WriteLine($"========== CrawlInstitution ==========");
            Queue<DateTime> error = Extention.LoadJson<Queue<DateTime>>(FilePath.Path_Raw_Root, FilePath.Name_Error_Institution) ?? new();

            DateTime? beginInstitution = Extention.LoadJson<DateTime?>(FilePath.Path_Raw_Root, FilePath.Name_UpdateTimeName_Institution);
            if (beginInstitution != null) beginInstitution = beginInstitution.Value.AddDays(1);

            InstitutionCrawler.Crawl(error, begin: beginInstitution);
        }
        static void ReCrawlPrice()
        {
            Console.WriteLine($"========== ReCrawlPrice ==========");
            Queue<DateTime> error = new();
            Queue<DateTime> errorPrice = Extention.LoadJson<Queue<DateTime>>(FilePath.Path_Raw_Root, FilePath.Name_Error_Price) ?? new();
            while (errorPrice.Any())
            {
                PriceCrawler.CrawlDate(errorPrice.Dequeue(), error);
                Thread.Sleep(2500);
                errorPrice.SaveJson(FilePath.Path_Raw_Root, FilePath.Name_Error_Price);
            }
            Console.WriteLine($"========== Error ==========");
            Console.WriteLine($"   - Error Count:{error.Count()}");
            error.SaveJson(FilePath.Path_Raw_Root, FilePath.Name_Error_Price);
            Console.WriteLine($"   - Error Saved!");
        }
        static void ReCrawlInstitution()
        {
            Console.WriteLine($"========== ReCrawlInstitution ==========");
            Queue<DateTime> error = new();
            Queue<DateTime> errorInstitutionError = Extention.LoadJson<Queue<DateTime>>(FilePath.Path_Raw_Root, FilePath.Name_Error_Institution) ?? new();
            while (errorInstitutionError.Any())
            {
                PriceCrawler.CrawlDate(errorInstitutionError.Dequeue(), error);
                Thread.Sleep(2500);
                errorInstitutionError.SaveJson(FilePath.Path_Raw_Root, FilePath.Name_Error_Institution);
            }
            Console.WriteLine($"========== Error ==========");
            Console.WriteLine($"   - Error Count:{error.Count()}");
            error.SaveJson(FilePath.Path_Raw_Root, FilePath.Name_Error_Institution);
            Console.WriteLine($"   - Error Saved!");
        }
    }
}

public static class FilePath
{
    public static string Path_Raw_Root { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StockViewer\\RawData"); }
    public static string Path_Raw_Price { get => Path.Combine(Path_Raw_Root, "Price"); }
    public static string Name_Error_Price = "Price_Error";
    public static string Name_UpdateTime_Price = "Price_UpdateTime";
    public static string Path_Raw_Institution { get => Path.Combine(Path_Raw_Root, "Institution"); }
    public static string Name_Error_Institution = "Institution_Error";
    public static string Name_UpdateTimeName_Institution = "Institution_UpdateTime";

}
