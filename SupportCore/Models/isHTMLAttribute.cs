using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportCore.Models
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
    public class IsHTML:Attribute
    {
    }
}
