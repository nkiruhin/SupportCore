using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    //Type=0 Аuto Comment
    //Type=1 Manual Comment
    // Events:
    // 0 - Add Comment
    // 1 - Open Ticket
    // 2 - Close Ticket
    // 3 - Late Ticket
    // 4 - Change Ticket
    // 5 - Completed Ticket

    public class TicketThread
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { set; get; }
        [HiddenInput(DisplayValue = false)]
        public int Type { set; get; }
        [ScaffoldColumn(false)]
        public string Poster { set; get; }
        [ScaffoldColumn(false)]
        public int SourceId { set; get; }
        [ScaffoldColumn(false)]
        public string Title {set; get;}
        [DataType(DataType.MultilineText)]
        [Required]
        [Display(Name ="Комментарий")]
        public string Body { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateCreate { set; get; }
        [ScaffoldColumn(false)]
        public DateTime DateUpdate { set; get; }
        //foreign key and navigation property
        [HiddenInput(DisplayValue = false)]
        public int TicketId { set; get; }
        [ScaffoldColumn(false)]
        public Ticket Ticket { set; get; }
        [HiddenInput(DisplayValue = false)]
        public string PersonId { set; get; }
        [ScaffoldColumn(false)]
        public Person Person { set; get; }
        public List<File> Files { set; get; }
    }
}
