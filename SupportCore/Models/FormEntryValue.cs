using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class FormEntryValue
    {
        public string Id { set; get; }
        public string Value { set; get; }
        //foreign key and navigation property on
        public int TicketId { set; get; }
        public Ticket Ticket { set; get; }
        public int FieldId { set; get; }
        public Field Field { set; get; }
    }
}
