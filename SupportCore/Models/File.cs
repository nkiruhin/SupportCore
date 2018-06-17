using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class File
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string ContentType { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string TicketThreadId { get; set; }
        public TicketThread TicketThread { get; set; }
    }
}
