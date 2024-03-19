using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models.Models.CountryModel
{
    public class CountryModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
    }
}
