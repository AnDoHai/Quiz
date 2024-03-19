using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models
{
    public class ConfigurationModel
    {
        public decimal ExchangeRate { get; set; }
        public int AdminAcceptanceBalancePercentage { get; set; }
        public int ClientAcceptanceBalancePercentage { get; set; }
    }
}
