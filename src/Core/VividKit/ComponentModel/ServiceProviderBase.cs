namespace Toolkit.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Toolkit.PresentationModel;

    public abstract class ServiceProviderBase : IServiceProvider
    {
        public void Initialize()
        {
            ViewModel.RegisterServiceProvider(this);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetServiceOverride(serviceType);
        }

        protected abstract object GetServiceOverride(Type serviceType);
    }
}