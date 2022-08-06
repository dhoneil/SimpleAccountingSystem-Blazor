using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Shared.GlobalVariables
{
    public class GlobalVariables
    {
        public static string CONNECTIONSTRING { get; set; } = string.Empty;
        public static string CONFIRMWINDOWTITLE { get; set; } = "Degalen Corp.";
        public static string TableClass {get;set;} = "table table-responsive-sm table-striped table-hover table-bordered";
    }
}
