using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Logic.Entities
{
    public class ExecutionInfo
    {
        // Na razie jako ogólny typ (można zcastować na typ wartościowy, string)
        public object? Result { get; set; }
        public ExecutionFailure? Failure { get; set; }
    }
}
