using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Logic.Entities
{
    public class ExecutionFailure
    {
        public const string Topic = "---------------------- Execution Error ----------------------";
        public string? ExecException { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
}
