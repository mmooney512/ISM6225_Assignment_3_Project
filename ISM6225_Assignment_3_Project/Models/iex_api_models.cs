using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISM6225_Assignment_3_Project.Models
{
    public class iex_api_Company
    {
        [Key]
        public string symbol { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public bool isEnabled { get; set; }
        public string type { get; set; }
        public string iexId { get; set; }
        public string userOption { get; set; }
        public string name_chart { get; set; }
        public List<iex_api_pricing> pricingData { get; set; }
    }

    public class iex_api_pricing
    {
        [Key]
        public string symbol { get; set; }
        [Key]
        public string date { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public float change { get; set; }
        public float changePercent { get; set; }
        public float vwap { get; set; }
        public string label { get; set; }
        public float changeOverTime { get; set; }
        public int EquityId { get; set; }
    }




    public class iex_symbol_prices
    {
        public iex_api_pricing[] chart { get; set; }
    }
}
