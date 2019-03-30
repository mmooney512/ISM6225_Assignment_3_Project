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
                    str = $"<option value = \"{iex_api_companies[counter].symbol}\" selected>{str} </ option >";
                }
                else
                {
                    str = $"<option value = \"{iex_api_companies[counter].symbol}\" >{str} </ option >";
                }
                iex_api_companies[counter].userOption = str;
            }
        }



        // -------------------------------------------------------------------
        // pricing data
        // -------------------------------------------------------------------
        private List<iex_api_pricing> getPricing(string stockSymbol)
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

            // check if in query results we have yesterday's data
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

            ViewBag.stockChart = iex_FormatPricing(stockSymbol, Prices);
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
        public IActionResult Index()
        {
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


            // format the drop down box; and if the ?get was passed
            // mark as selected in the drop down box
            iex_FormatSymbols("None");

            // return the companies info
            return View(iex_api_companies);

        }

        public IActionResult Stocks(string getSymbol)
        {
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

            // format the drop down box 
            iex_FormatSymbols(getSymbol);

            // switch if the symbol was posted
            if (getSymbol != null)
            {
                PrepHttpClient();
                // go get the pricing of the speciefed symbol
                stockPrices = getPricing(getSymbol);
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
