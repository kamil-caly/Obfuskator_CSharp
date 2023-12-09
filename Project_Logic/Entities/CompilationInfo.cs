namespace Project_Logic.Entities
{
    public class CompilationInfo
    {
        public required MemoryStream CompilationMS { get; set; }
        public IEnumerable<CompilationFailure>? CompilationFailures { get; set; }
    }
}
