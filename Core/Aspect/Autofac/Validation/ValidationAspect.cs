using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Exceptions;
using Core.Utilities.Interceptor;
using FluentValidation;

namespace Core.Aspect.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private readonly Type _validationType;
        public ValidationAspect(Type validationType)
        {
            if (typeof(IValidator).IsAssignableFrom(validationType) == false)
            {
                throw new WrongValidationTypeException();
            }
            _validationType = validationType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validationType);
            Type entityType = _validationType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(x => x.GetType() == entityType);
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
