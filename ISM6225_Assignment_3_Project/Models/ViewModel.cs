using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISM6225_Assignment_3_Project.Models
{
    public class ViewModel
    {
        public IEnumerable<Models.iex_api_Company> iex_Api_Companies { get; set; }
        public IEnumerable<Models.iex_api_chart_Stock_Prices> iex_Api_Chart_Stock_Prices { get; set; }
        public IEnumerable<Models.fx_api_Rates> fx_Api_Rates { get; set; }
    }
}
