using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.ViewModels
{
    public class TicketView
    {
        public int Id { set; get; }
        public string dateCreate { set; get; }
        public string Name { set; get; }
        public string PersonName { set; get; }
        public string StaffName { set; get; }
        public int Status { set; get; }
        public int SourceId { set; get; }
        public string Closed { set; get; }
        public string CategoryName { set; get; }
        public bool IsOverdue { set; get; }
        public bool IsCoAthors  { set; get; }
        public bool withFiles { set; get; }
        public bool withTasks { set; get; }
        public string Priority { set; get; }
    }
}
