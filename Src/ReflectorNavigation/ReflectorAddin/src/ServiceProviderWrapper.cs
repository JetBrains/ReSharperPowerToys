using System;

namespace JetBrains.ReSharper.PowerToys.ReflectorNavigation.ReflectorAddin
{
  public class ServiceProviderWrapper
  {
    private readonly IServiceProvider myServiceProvider;

    public ServiceProviderWrapper(IServiceProvider serviceProvider)
    {
      myServiceProvider = serviceProvider;
    }

    public T GetService<T>()
    {
      object o = myServiceProvider.GetService(typeof (T));
      if (o == null)
        throw new InvalidOperationException("Can't find implementation for " + typeof (T).FullName);

      return (T) o;
    }
  }
}