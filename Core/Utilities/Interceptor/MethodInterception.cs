using Castle.DynamicProxy;

namespace Core.Utilities.Interceptor
{
    public class MethodInterception : MethodInterceptionBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnSuccess(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }

        public override void Intercept(IInvocation invocation)
        {
            bool success = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
                var task = invocation.ReturnValue as Task;
                if (task != null)
                    if(task.IsFaulted)
                    {
                        success = false;
                        OnException(invocation, task.Exception);
                    }
            }
            catch (System.Exception e)
            {
                success = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (success)
                    OnSuccess(invocation);
            }
            OnAfter(invocation);
        }
    }
}
