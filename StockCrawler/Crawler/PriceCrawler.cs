using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace StockCrawler;
public class PriceCrawler
{
    private static readonly HttpClient client = new HttpClient();
    public List<List<string>>? data8 { get; set; }
    public List<List<string>>? data9 { get; set; }
    public List<string>? fields9 { get; set; }

    public static PriceCrawler? Crawl(DateTime target)
    {
        PriceCrawler? crawler;
        try
        {
            string url = $"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=json&date=" + $"{ target:yyyyMMdd}" + "&type=ALLBUT0999";
            Trace.WriteLine($"   - Send request: " + url);
            crawler = HttpClientJsonExtensions.GetFromJsonAsync<PriceCrawler?>(client, url).Result;
        }
        catch (Exception)
        {
            Trace.WriteLine($"   [Error] Can't load website {target:yyyy/MM/dd}");
            return null;
        }
        return crawler;
    }
}
