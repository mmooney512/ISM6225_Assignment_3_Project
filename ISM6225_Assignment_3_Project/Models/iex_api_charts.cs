using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISM6225_Assignment_3_Project.Models
{
    public class iex_api_chart_Stock_Prices
    {
        public string Symbol { get; set; }
        public string CompanyName { get; set; }
        public string Dates { get; set; }
        public string PriceClosing { get; set; }
    }

    public class fx_api_fx_rates
    {
        public string date { get; set; }
        public float GBP_USD { get; set; }
        public float USD_GBP { get; set; }
        public float EUR_USD { get; set; }
        public float USD_EUR { get; set; }
        public float JPY_USD { get; set; }
        public float USD_JPY { get; set; }
    }

    public class fx_api_chart_xe_rates
    {
        public string ExchangeRate { get; set; }
        public string Dates { get; set; }
        public string ClosingRate { get; set; }
    }



public class iex_fx_chart_Stock_Prices
    {
        public string symbol { get; set; }
        public string date { get; set; }
        public float close { get; set; }
        public float close_USD { get; set; }
        public float close_GBP { get; set; }
        public float close_EUR { get; set; }
        public float close_JPY { get; set; }

    }



}
