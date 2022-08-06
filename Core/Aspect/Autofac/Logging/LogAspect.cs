using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.Exceptions;
using Core.Utilities.Interceptor;
using Newtonsoft.Json;

namespace Core.Aspect.Autofac.Logging
{
    public class LogAspect : MethodInterception
    {
        private readonly LoggerServiceBase _loggerServiceBase;
        public LogAspect(Type loggerType)
        {
            if (typeof(LoggerServiceBase).IsAssignableFrom(loggerType) == false)
            {
                throw new WrongLoggingTypeException();
            }
            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerType);
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _loggerServiceBase.Info(LogDetail(invocation));
        }

        private string LogDetail(IInvocation invocation)
        {
            List<LogParameter> parameters = invocation.Arguments.Select((p, i) => new LogParameter
            {
                Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                Type = invocation.Arguments[i].GetType().ToString(),
                Value = invocation.Arguments[i]
            }).ToList();
            LogDetail logDetail = new()
            {
                MethodName = invocation.GetConcreteMethod().Name,
                Parameters = parameters
            };
            return JsonConvert.SerializeObject(logDetail);
        }
    }
}
