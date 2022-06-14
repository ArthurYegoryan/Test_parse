using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestParse
{
    public class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadKey();
        }

        private static async void GetHtmlAsync()
        {            
            var url = "https://www.ebay.com/sch/i.html?_from=R40&_nkw=playstation+5&_sacat=0&Country%252FRegion%2520of%2520Manufacture=United%2520States&LH_ItemCondition=1000&_udlo=800&_udhi=1000&rt=nc&Storage%2520Capacity=1%2520TB&_dcat=139971";

            HttpClient httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var ProductsHtml = htmlDocument.DocumentNode.Descendants("ul").
                Where(node => node.GetAttributeValue("class", "").
                Equals("srp-results srp-list clearfix")).ToList();

            var ProductListItems = ProductsHtml[0].Descendants("li").
                Where(node => node.GetAttributeValue("data-view", "").
                Contains("mi:1686")).ToList();

            foreach (var ProductListItem in ProductListItems)
            {
                // id
                Console.Write("Product id: ");
                Console.WriteLine(ProductListItem.GetAttributeValue("data-view", ""));

                // Product name
                Console.Write("Product name: ");
                Console.WriteLine(ProductListItem.Descendants("h3").
                    Where(node => node.GetAttributeValue("class", "").
                    Equals("s-item__title")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // Price
                Console.Write("Product price: ");
                Console.WriteLine(ProductListItem.Descendants("span").
                    Where(node => node.GetAttributeValue("class", "").
                    Equals("s-item__price")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // Shipping price
                Console.Write("Product shipping price: ");
                Console.WriteLine(ProductListItem.Descendants("span").
                   Where(node => node.GetAttributeValue("class", "").
                   Equals("s-item__shipping s-item__logisticsCost")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // From where
                Console.Write("Product coming place: ");
                Console.WriteLine(ProductListItem.Descendants("span").
                   Where(node => node.GetAttributeValue("class", "").
                   Equals("s-item__location s-item__itemLocation")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // Url
                Console.Write("Product url: ");
                Console.WriteLine(ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", ""));

                Console.WriteLine();
            }
        }
    }
}
