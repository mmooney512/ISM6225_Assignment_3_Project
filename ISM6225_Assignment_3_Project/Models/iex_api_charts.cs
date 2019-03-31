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
