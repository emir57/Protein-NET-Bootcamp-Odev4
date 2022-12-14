using Castle.DynamicProxy;
using Core.Aspect.Autofac.Exception;
using Core.Aspect.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using System.Reflection;

namespace Core.Utilities.Interceptor
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes
                <MethodInterceptionBaseAttribute>(false).ToList();
            var methodAttributes = type.GetMethod(method.Name)
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(false);

            classAttributes.AddRange(methodAttributes);
            classAttributes.Add(new LogAspect(typeof(FileLogger)));
            classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger)));

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
