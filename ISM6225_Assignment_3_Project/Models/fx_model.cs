using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ISM6225_Assignment_3_Project.Models
{
    public class fxSymbol
    {
        public string currencySymbol { get; set; }
        public string currencyName { get; set; }
        public string userOption { get; set; }

        public fxSymbol(string currSymbol, string currName)
        {
            currencySymbol = currSymbol;
            currencyName = currName;
            userOption = $"{currencySymbol} { currencyName}";
        }
    }
    public class fxModel
    {
        private List<fxSymbol> fxSymbols = new List<fxSymbol>();
        public List<fxSymbol> FxSymbols { get => fxSymbols; set => fxSymbols = value; }

        public fxModel()
        {
            fxSymbols.Add(new fxSymbol("$", "USD"));
            fxSymbols.Add(new fxSymbol("£", "GBP"));
            fxSymbols.Add(new fxSymbol("€", "EUR"));
            fxSymbols.Add(new fxSymbol("¥", "JPY"));
        }
    }

    public class fx_api_Rates
    {
        [Key]
        public string date { get; set; }
        public float USD { get; set; }
        public float GBP { get; set; }
        public float EUR { get; set; }
        public float JPY { get; set; }
    }

    public class fx_api_json_Rates
    {
        public fx_api_Rates[] rates { get; set; }
    }

}
