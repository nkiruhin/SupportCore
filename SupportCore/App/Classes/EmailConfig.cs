using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.App.Classes
{
    public class EmailConfig
    {
        public String FromName { get; set; }
        public String FromAddress { get; set; }

        public String LocalDomain { get; set; }
        public String Signature { get; set; }
        public String MailServerAddress { get; set; }
        public String MailServerPort { get; set; }
        public String ImapServerAddress { get; set; }
        public String ImapServerPort { get; set; }
        public String UserId { get; set; }
        public String UserPassword { get; set; }
    }
}
