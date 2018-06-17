using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    public class Configuration
    {
        [Key]
        public string Name { set; get; }
        public string Value { set; get; }
        public string Section { set; get; }
        public string Title { set; get; }
        public string Type { set; get; }
    }
}
