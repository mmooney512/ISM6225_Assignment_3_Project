using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// added libaries
using System.Text;

// Libaries for API's
using System.Net.Http;
using Newtonsoft.Json;

// using my model
using ISM6225_Assignment_3_Project.Models;

namespace ISM6225_Assignment_3_Project.Controllers
{
    public class HomeController : Controller
    {
        // HttpClient to send api requests
        HttpClient http_client;

        // iex API Variables
        const string iex_url = "https://api.iextrading.com/1.0/";
        List<iex_api_Company> iex_api_companies = null;
        List<iex_api_logo> iex_api_logos = null;
        List<iex_api_new> iex_api_news = null;

        // list of symbols to look up
        List<string> SymbolList = new List<string>
        {
            "AAL"   //  American Airlines
            ,"DAL"  //  Delta Airlines
            ,"HA"   //  Hawaiian Airlines
            ,"LUV"  //  Southwest Airlines
            ,"UAL"  //  United Airlines

            ,"HLT"  //  Hilton Hotels
            ,"H"    //  Hyatt Hotels
            ,"HST"  //  Host Hotel & Resorts
            ,"MAR"  //  Marriott International
            ,"WH"   //  Wyndham

            ,"HTZ"  //  Hertz Car Rental
            ,"CAR"  //  Avis Budget
        };

        //list of logos to look up

        List<string> LogoList = new List<string>
        {
           "AAL"   //  American Airlines
            ,"DAL"  //  Delta Airlines
            ,"HA"   //  Hawaiian Airlines
            ,"LUV"  //  Southwest Airlines
            ,"UAL"  //  United Airlines

            ,"HLT"  //  Hilton Hotels
            ,"H"    //  Hyatt Hotels
            ,"HST"  //  Host Hotel & Resorts
            ,"MAR"  //  Marriott International
            ,"WH"   //  Wyndham

            ,"HTZ"  //  Hertz Car Rental
            ,"CAR"  //  Avis Budget
        };
        //list of news to look up
        List<string> NewList = new List<string>
        {
           "AAL"   //  American Airlines
            ,"DAL"  //  Delta Airlines
            ,"HA"   //  Hawaiian Airlines
            ,"LUV"  //  Southwest Airlines
            ,"UAL"  //  United Airlines

            ,"HLT"  //  Hilton Hotels
            ,"H"    //  Hyatt Hotels
            ,"HST"  //  Host Hotel & Resorts
            ,"MAR"  //  Marriott International
            ,"WH"   //  Wyndham

            ,"HTZ"  //  Hertz Car Rental
            ,"CAR"  //  Avis Budget
        };
        private void PrepHttpClient()
        {
            http_client = new HttpClient();
            http_client.DefaultRequestHeaders.Accept.Clear();
            http_client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void iex_GetSymbols()
        {
            const string IEXTrading_API_PATH = iex_url + "ref-data/symbols";
            string companyList = "";

                http_client.BaseAddress = new Uri(IEXTrading_API_PATH);
                HttpResponseMessage http_response = http_client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

                if (http_response.IsSuccessStatusCode)
                {
                    companyList = http_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!companyList.Equals(""))
                {
                    iex_api_companies = JsonConvert.DeserializeObject<List<iex_api_Company>>(companyList);
                    iex_api_companies = iex_api_companies.Where(predefinedQuery).ToList();
                }
        }


        private bool predefinedQuery(iex_api_Company c)
        {
            return(SymbolList.Contains(c.symbol));
        }


        private void StockSymbolSearch(String s)
        {
            
        }


        private void iex_FormatSymbols(string stockSymbol)
        {

            //
            for(int counter=0; counter < iex_api_companies.Count; counter++)
            {
                string str = $"{iex_api_companies[counter].symbol} | {iex_api_companies[counter].name}";
                iex_api_companies[counter].name = str +"\n";
                iex_api_companies[counter].name_chart = str;

                if (stockSymbol == iex_api_companies[counter].symbol)
                {
                    str = $"<option value = \"{iex_api_companies[counter].symbol}\" selected>{iex_api_companies[counter].name} </ option >";
                }
                else
                {
                    str = $"<option value = \"{iex_api_companies[counter].symbol}\" >{iex_api_companies[counter].name} </ option >";
                }
                iex_api_companies[counter].userOption = str;
            }
        }

        private bool predefinedQuery(iex_api_logo l)
        {
            return (LogoList.Contains(l.url));
        }
        private void iex_GetLogos(string symbol)
        {
            string IEXTrading_API_PATH = iex_url + "stock/"+ symbol + "/logo";
            string logoList = "";

            // connect to the IEXTrading API and retrieve information
            HttpResponseMessage http_response = http_client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            
            // read the Json objects in the API response
            if (http_response.IsSuccessStatusCode)
            {
                logoList = http_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!logoList.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                logoList = logoList.Substring(8,logoList.Length-10);
                ViewBag.logo = logoList;
            }
        }
 
        private void iex_GetNews(string symbol)
        {
            string IEXTrading_API_PATH = iex_url + "stock/" + symbol + "/news/last/5";
            string newList = "";

            // connect to the IEXTrading API and retrieve information
            HttpResponseMessage http_response = http_client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            // read the Json objects in the API response
            if (http_response.IsSuccessStatusCode)
            {
                newList = http_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // now, parse the Json strings as C# objects
            if (!newList.Equals(""))
            {
                // https://stackoverflow.com/a/46280739
                iex_api_news = JsonConvert.DeserializeObject<List<iex_api_new>>(newList);
                ViewBag.news = iex_api_news;

            }
    
        }

        // -------------------------------------------------------------------
        // pricing data
        // -------------------------------------------------------------------
        private List<iex_api_pricing> getPricing(string stockSymbol)
        {
            string IEXTrading_API_PATH = iex_url + "stock/" + stockSymbol + "/batch?types=chart&range=3m";
            string priceList = "";
            // hold pricing data
            List<iex_api_pricing> Prices = new List<iex_api_pricing>();
            http_client.BaseAddress = new Uri(IEXTrading_API_PATH);

            // get pricing data from the API
            HttpResponseMessage http_response = http_client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

            if(http_response.IsSuccessStatusCode)
            {
                priceList = http_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            // if the priceList is not empty 
            if(!priceList.Equals(""))
            {
                iex_symbol_prices root = JsonConvert.DeserializeObject<iex_symbol_prices>(priceList
                    , new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                Prices = root.chart.ToList();
            }

            ViewBag.stockChart = iex_FormatPricing(stockSymbol, Prices);
            return (Prices);
        }


        public iex_api_chart_Stock_Prices iex_FormatPricing(string stockSymbol, List<iex_api_pricing> Prices)
        {
            
            iex_api_Company iac = new iex_api_Company();
            iac = iex_api_companies.Where(stock => stock.symbol == stockSymbol).First();

            iex_api_chart_Stock_Prices chartData = new iex_api_chart_Stock_Prices
            {
                Symbol = stockSymbol
                , CompanyName = iac.name_chart
                , PriceClosing = string.Join(",", Prices.Select(stock => stock.close))
                , Dates = string.Join(",", Prices.Select(stock => stock.date))
            };

            return chartData;
        }

        // -------------------------------------------------------------------
        // web pages
        // -------------------------------------------------------------------
        public IActionResult Index(string getSymbol)
        {
            // prep the HttpClient, set it to accept a JSON response 
            PrepHttpClient();

            //ViewBag.dbSuccessComp = 0;
            List<iex_api_pricing> stockPrices = new List<iex_api_pricing>();

            // rest API call to IEX; 
            // store the values in a custom model iex_api_company
            iex_GetSymbols();

            // format the drop down box 
            iex_FormatSymbols(getSymbol);
        
            // store the values in a custom model iex_api_logo
            iex_GetLogos(getSymbol);

            // store the values in a custom model iex_api_news
            iex_GetNews(getSymbol);

            // return the companies info
            return View(iex_api_companies);

            //return View();

        }

        public IActionResult Stocks(string getSymbol)
        {
            // prep the HttpClient, set it to accept a JSON response 
            PrepHttpClient();

            //ViewBag.dbSuccessComp = 0;
            List<iex_api_pricing> stockPrices = new List<iex_api_pricing>();

            // rest API call to IEX; 
            // store the values in a custom model iex_api_company
            iex_GetSymbols();

            // format the drop down box 
            iex_FormatSymbols(getSymbol);

            // switch if the symbol was posted
            if (getSymbol != null)
            {
                PrepHttpClient();
                // go get the pricing of the specified symbol
                stockPrices = getPricing(getSymbol);

                // go get the logo of the specified symbol
                iex_GetLogos(getSymbol);

                // go get the logo of the specified symbol
                iex_GetNews(getSymbol);
            }

            return View(iex_api_companies);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
