using System;
using Model;

namespace Service
{
    public class CondimentServiceFactory : ICondimentServiceFactory
    {
        private readonly IApplicationSettingsService _appSettings;

        public CondimentServiceFactory(IApplicationSettingsService appSettings)
        {
            _appSettings = appSettings;
        }

        public ICondimentService Create(CondimentType type)
        {
            switch(type)
            {
                case CondimentType.Cream:
                    return new CreamService(_appSettings);
                case CondimentType.Sugar:
                    return new SugarService(_appSettings);
                default:
                    throw new Exception("Invalid condiment type");            
            }
        }
    }
}
