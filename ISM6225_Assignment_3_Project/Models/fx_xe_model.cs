using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISM6225_Assignment_3_Project.Models
{
    public class xeSymbol
    {
        public string baseXe { get; set; }
        public string convertXe { get; set; }
        public string userOption { get; set; }
        public xeSymbol(string baseXe_Value, string convertXe_value)
        {
            baseXe = baseXe_Value;
            convertXe = convertXe_value;
            userOption = $"{baseXe}-{convertXe}";
        }
    }

    public class fx_XeModel
    {
        private List<xeSymbol> xeSymbols = new List<xeSymbol>();
        public List<xeSymbol> XeSymbols { get => xeSymbols; set => XeSymbols = value; }

        public fx_XeModel()
        {
            xeSymbols.Add(new xeSymbol("USD", "GBP"));
            xeSymbols.Add(new xeSymbol("GBP", "USD"));

            xeSymbols.Add(new xeSymbol("USD", "EUR"));
            xeSymbols.Add(new xeSymbol("EUR", "USD"));

            xeSymbols.Add(new xeSymbol("USD", "JPY"));
            xeSymbols.Add(new xeSymbol("JPY", "USD"));
        }
    }
}
