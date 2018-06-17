using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class FormEntry
    {
        public int Id { set; get; }
        public int FormId {set;get;}
        public int TicketId { set; get; }
        public int Type { set; get; }
        public DateTime DateCreate { set; get; }
        public DateTime DateUpdate { set; get; }
    }
}
