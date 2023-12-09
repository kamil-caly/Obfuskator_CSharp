using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Logic.Entities
{
    public class CompilationFailure
    {
        public readonly string Topic = "---------------------- Compilation Error ----------------------";
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
        public string? Severity { get; set; }
        public int? StartLine { get; set; }
        public int? StartColumn { get; set; }
    }
}
