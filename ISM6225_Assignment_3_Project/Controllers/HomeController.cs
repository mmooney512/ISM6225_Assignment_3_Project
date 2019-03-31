using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// added libaries
using System.Text;
using System.Text.RegularExpressions;

// Libaries for API's
using System.Net.Http;
using Newtonsoft.Json;

// database connection
using ISM6225_Assignment_3_Project.DataAccess;

// using my model
using ISM6225_Assignment_3_Project.Models;

namespace ISM6225_Assignment_3_Project.Controllers
{
    public class HomeController : Controller
    {
        // HttpClient to send api requests
        HttpClient http_client;

        // database connection object
        public ApplicationDbContext dbContext;

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
        // list of currencies to look up
        fxModel CurrencyList = new fxModel();


        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }


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


        /// <summary>
        /// formats the stock symbols so they can be put in the drop down list
        /// </summary>
        /// <param name="stockSymbol"></param>
        private void iex_FormatSymbols(string stockSymbol)
        {

            //
            for(int counter=0; counter < iex_api_companies.Count; counter++)
            {
                string str = $"{iex_api_companies[counter].symbol} | {iex_api_companies[counter].name}";
                //iex_api_companies[counter].name = str +"\n";
                iex_api_companies[counter].name_chart = str;

                if (stockSymbol == iex_api_companies[counter].symbol)
                {
                    str = $"<option value = \"{iex_api_companies[counter].symbol}\" selected>{str}</ option >";
                }
                else
                {
                    str = $"<option value = \"{iex_api_companies[counter].symbol}\" >{str}</ option >";
                }
                iex_api_companies[counter].userOption = str;
            }
        }

        // -------------------------------------------------------------------
        // fx data
        // -------------------------------------------------------------------
        private void getFxData()
        {
            string ExchangeRates_API_PATH = "https://api.exchangeratesapi.io/history?base=USD&symbols=USD,GBP,EUR,JPY";
            DateTime isToday = DateTime.Today;
            string date_start   = $"&start_at={isToday.AddDays(-45).ToString("yyyy-MM-dd")}";
            string date_end     = $"&end_at={isToday.ToString("yyyy-MM-dd")}";
            string maxDateTime  = "2001-01-01";
            string fxList = "";
            List<fx_api_Rates> fx_Rates = new List<fx_api_Rates>();
            ExchangeRates_API_PATH = $"{ExchangeRates_API_PATH}{date_start}{date_end}";
            

            // run the query to get historical rates
            fx_Rates = getHistoricalRates();

            // check if in query results we have today's data
            // if not then go get it from the API
            Boolean getFxRatesFromAPI = true;
            

            if(fx_Rates != null && fx_Rates.Count > 2)
            {
                maxDateTime = fx_Rates.Last().date;
                if(Convert.ToDateTime(maxDateTime) >= isToday )
                {
                    getFxRatesFromAPI = false;
                }
            }


            if (getFxRatesFromAPI == true)
            {
                http_client.BaseAddress = new Uri(ExchangeRates_API_PATH);
                // get pricing data from the API
                HttpResponseMessage http_response = http_client.GetAsync(ExchangeRates_API_PATH).GetAwaiter().GetResult();
                if (http_response.IsSuccessStatusCode)
                {
                    fxList = http_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                // if the fxList is not empty 
                if (!fxList.Equals(""))
                {
                    //fix json error
                    fxList = fxList.Replace(",\"2019", ",{\"date\":\"2019");
                    fxList = fxList.Replace("\"rates\":{", "\"rates\":[{\"date\":");
                    fxList = fxList.Replace(":{\"USD", ",\"USD");
                    fxList = Regex.Replace(fxList, "}},.*", "}]}");

                    fx_api_json_Rates root = JsonConvert.DeserializeObject<fx_api_json_Rates>(fxList
                        , new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    fx_Rates = root.rates.ToList();
                }

                // write historical fx rates
                writeHistoricalRates(fx_Rates , maxDateTime);

            }
        }

        private List<fx_api_Rates> getHistoricalRates()
        {
            List<fx_api_Rates> historicalRates = new List<fx_api_Rates>();

            DateTime dateStart = DateTime.Today;
            dateStart = dateStart.AddDays(-45);

            // get the historical rates sorted by date
            historicalRates = dbContext.db_fx.Where(w => Convert.ToDateTime(w.date) > dateStart)
                                                    .OrderBy(o => Convert.ToDateTime(o.date))
                                                    .ToList();
            return historicalRates;
        }

        private void writeHistoricalRates(List<fx_api_Rates> fx_Rates , string maxDateTime)
        {
            foreach(fx_api_Rates histRate in fx_Rates)
            {
                if (Convert.ToDateTime(maxDateTime) < Convert.ToDateTime(histRate.date))
                {
                    dbContext.db_fx.Add(histRate);
                }
            }
            // commit the transaction
            dbContext.SaveChanges();
        }

        private void fx_FormatCurrency(string currencySymbol)
        {
            foreach(fxSymbol f in CurrencyList.FxSymbols)
            {
                string str = $"{f.currencySymbol} {f.currencyName}";
                if (f.currencyName == currencySymbol)
                {
                    f.userOption = $"<option value =\"{f.currencyName}\" selected>{str}</option>";
                }
                else
                {
                    f.userOption = $"<option value =\"{f.currencyName}\" >{str}</option>";
                }
            }
        }

        // -------------------------------------------------------------------
        // logo data
        // -------------------------------------------------------------------
        private bool predefinedQuery(iex_api_logo l)
        {
            return (LogoList.Contains(l.url));
        }
        private void iex_GetLogos(string symbol)
        {
            string IEXTrading_API_PATH = iex_url + "stock/" + symbol + "/logo";
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
                logoList = logoList.Substring(8, logoList.Length - 10);
                ViewBag.logo = logoList;
            }
        }
        // -------------------------------------------------------------------
        // news data
        // -------------------------------------------------------------------
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
        private List<iex_api_pricing> getPricing(string stockSymbol, string getFx)
        {
            string IEXTrading_API_PATH = iex_url + "stock/" + stockSymbol + "/batch?types=chart&range=3m";
            string priceList = "";
            // will use to check what is the latest pricing data we have for the stock
            string maxDateTime = "2001-01-01";
            DateTime checkDateTime = new DateTime(2001, 1, 1);
            // hold pricing data
            List<iex_api_pricing> Prices = new List<iex_api_pricing>();
            
            // run the query
            Prices = getHistoricalPrices(stockSymbol);

            // check if in query results we have today's data
            // if not then go get it from the API
            Boolean getPricingFromAPI = true;
            DateTime isToday =  DateTime.Today;

            if (Prices != null && Prices.Count > 2)
            {
                maxDateTime = Prices.Last().date;
                checkDateTime = Convert.ToDateTime(maxDateTime);
                if (checkDateTime >= isToday)
                {
                    getPricingFromAPI = false;
                }
            }

            // if there was no data in the database
            if (getPricingFromAPI == true)
            {
                http_client.BaseAddress = new Uri(IEXTrading_API_PATH);

                // get pricing data from the API
                HttpResponseMessage http_response = http_client.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();

                if (http_response.IsSuccessStatusCode)
                {
                    priceList = http_response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                // if the priceList is not empty 
                if (!priceList.Equals(""))
                {
                    iex_symbol_prices root = JsonConvert.DeserializeObject<iex_symbol_prices>(priceList
                        , new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                    Prices = root.chart.ToList();
                    
                }
                // need to append the symbol to the JSON data
                foreach (iex_api_pricing Price in Prices)
                {
                    Price.symbol = stockSymbol;
                }

                // save the data we just got
                writeHistoricalPrices(Prices , maxDateTime);
            }

            ViewBag.stockChart = iex_FormatPricing(stockSymbol, getFx);
            return (Prices);
        }

        /// <summary>
        /// query the data base and get the pricing for the last 45 days
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns></returns>
        private List<iex_api_pricing> getHistoricalPrices(string stockSymbol)
        {
            List<iex_api_pricing> historicalPrices = new List<iex_api_pricing>();
            DateTime dateStart = DateTime.Today;
            dateStart = dateStart.AddDays(-45);
            
            // get the historical prices sorted by date
            historicalPrices = dbContext.db_prices.Where(w => w.symbol.Equals(stockSymbol)
                                                               && Convert.ToDateTime(w.date) > dateStart)
                                                        .OrderBy(o => Convert.ToDateTime(o.date))
                                                        .ToList();
            return historicalPrices;
        }

        private void writeHistoricalPrices(List<iex_api_pricing> histPrices, string maxDateTime)
        {

            // write the data to the table
            foreach (iex_api_pricing histPrice in histPrices)
            {
                if (Convert.ToDateTime(maxDateTime) < Convert.ToDateTime(histPrice.date))
                {
                    // insert the rows
                    dbContext.db_prices.Add(histPrice);
                }
            }
            // commit the transaction
            dbContext.SaveChanges();
        }


        public iex_api_chart_Stock_Prices iex_FormatPricing(string stockSymbol
                                                            , string getFx)
        {
            
            iex_api_Company iac = new iex_api_Company();
            iac = iex_api_companies.Where(stock => stock.symbol == stockSymbol).First();

            List<iex_fx_chart_Stock_Prices> fxPrices = new List<iex_fx_chart_Stock_Prices>();

            DateTime dateStart = DateTime.Today.AddDays(-45);

            // get the historical prices sorted by date
            fxPrices = dbContext.view_fx_pricing.Where(w=> w.symbol.Equals(stockSymbol)
                                                    && Convert.ToDateTime(w.date) > dateStart)
                                                    .OrderBy(o => Convert.ToDateTime(o.date))
                                                    .ToList();
            string pc = "";
            switch(getFx)
            {
                case "EUR":
                    pc = string.Join(",", fxPrices.Select(stock => stock.close_EUR));
                    break;
                case "JPY":
                    pc = string.Join(",", fxPrices.Select(stock => stock.close_JPY));
                    break;
                case "GBP":
                    pc = string.Join(",", fxPrices.Select(stock => stock.close_GBP));
                    break;
                case "USD":
                    pc = string.Join(",", fxPrices.Select(stock => stock.close_USD));
                    break;
                default:
                    pc = string.Join(",", fxPrices.Select(stock => stock.close));
                    break;
            }

            iex_api_chart_Stock_Prices chartData = new iex_api_chart_Stock_Prices
            {
                Symbol = stockSymbol
                , CompanyName = iac.name_chart
                , PriceClosing = pc
                , Dates = string.Join(",", fxPrices.Select(stock=> stock.date))

            };

            return chartData;
        }

        // -------------------------------------------------------------------
        // web pages
        // -------------------------------------------------------------------
        public IActionResult Index()
        {
            // format the currency selection
            fx_FormatCurrency("USD");
            ViewBag.CurrencyList = CurrencyList;

            // prep the HttpClient, set it to accept a JSON response 
            PrepHttpClient();

            List<iex_api_pricing> stockPrices = new List<iex_api_pricing>();

            // load the companies list from the database
            iex_api_companies = dbContext.db_companies.ToList();


            // we should have our 12 stocks loaded in the database
            if (iex_api_companies is null || iex_api_companies.Count < 12)
            {
                // rest API call to IEX; 
                // store the values in a custom model iex_api_company
                iex_GetSymbols();
            }


            // format the drop down box; and if the ?get was passed
            // mark as selected in the drop down box
            iex_FormatSymbols("None");

            // return the companies info
            return View(iex_api_companies);

        }

        public IActionResult Stocks(string getSymbol, string getFx)
        {
            // format the currency selection
            fx_FormatCurrency(getFx);
            ViewBag.CurrencyList = CurrencyList;
            // prep the HttpClient, set it to accept a JSON response 
            PrepHttpClient();

            //ViewBag.dbSuccessComp = 0;
            List<iex_api_pricing> stockPrices = new List<iex_api_pricing>();

            // load the companies list from the database
            iex_api_companies = dbContext.db_companies.ToList();

            // we should have our 12 stocks loaded in the database
            if (iex_api_companies is null || iex_api_companies.Count < 12)
            {
                // rest API call to IEX; 
                // store the values in a custom model iex_api_company
                iex_GetSymbols();
            }

            // switch to get the currency
            if (true)
            {
                PrepHttpClient();
                getFxData();
            }


            // format the drop down box 
            iex_FormatSymbols(getSymbol);

            // store the values in a custom model iex_api_logo
            iex_GetLogos(getSymbol);

            // store the values in a custom model iex_api_news
            iex_GetNews(getSymbol);

            // switch if the symbol was posted
            if (getSymbol != null)
            {
                PrepHttpClient();
                // go get the pricing of the speciefed symbol
                stockPrices = getPricing(getSymbol ,getFx);

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
