using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Models
{
    public class ConfirmModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int HSKId { get; set; }
        public int Time { get; set; }
        public string Content { get; set; }
        public string Level { get; set; }
        public string Class { get; set; }
    }
}
