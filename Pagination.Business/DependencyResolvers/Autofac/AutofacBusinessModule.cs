using Autofac;
using Autofac.Extras.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.Utilities.Interceptor;
using Pagination.Business.Abstract;
using Pagination.Business.Concrete;
using Pagination.DataAccess.Abstract;
using Pagination.DataAccess.Concrete.Dapper;

namespace Pagination.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            

            #region Business
            builder.RegisterType<PersonManager>().As<IPersonService>();
            #endregion

            #region DataAccess
            builder.RegisterType<DpPersonDal>().As<IPersonDal>();
            #endregion

            #region Cache
            builder.RegisterType<RedisCacheManager>().As<ICacheManager>();
            #endregion

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                });
        }
    }
}
