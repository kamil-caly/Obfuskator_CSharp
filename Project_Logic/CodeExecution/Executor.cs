using Project_Logic.Entities;
using System.Reflection;

namespace Project_Logic.CodeExecution
{
    public class Executor
    {
        private const string MainClassName = "Program";
        private const string MainMethodName = "Main";
        public static ExecutionInfo Execute(MemoryStream compiledCodeMS)
        {
            using (var ms = new MemoryStream(compiledCodeMS.GetBuffer()))
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                Type type = assembly.GetType(MainClassName)!;
                object obj = Activator.CreateInstance(type)!;

                MethodInfo? mainMethod = type.GetMethods().FirstOrDefault(m => m.Name == MainMethodName);

                if (mainMethod is null)
                {
                    throw new Exception("Code does not have any 'Main' method");
                }

                try
                {
                    object? result = mainMethod.Invoke(obj, null);
                    return new ExecutionInfo { Result = result };   
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException targetEx && targetEx.InnerException != null)
                    {
                        return new ExecutionInfo
                        {
                            Failure = new ExecutionFailure
                            {
                                ExecException = targetEx.InnerException.GetType().FullName,
                                Message = targetEx.InnerException.Message,
                                StackTrace = targetEx.InnerException.StackTrace
                            }
                        };
                    }
                    
                    return new ExecutionInfo
                    {
                        Failure = new ExecutionFailure
                        {
                            ExecException = ex.GetType().FullName,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        }
                    };
                }
            }
        }
    }
}
