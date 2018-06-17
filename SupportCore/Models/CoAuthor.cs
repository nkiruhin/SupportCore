using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class CoAuthor
    {
        public int Id { set; get; }
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }
        public string PersonId { set; get; }
        public Person Person { set; get; }
        public int TicketId { set; get; }
    }
}
