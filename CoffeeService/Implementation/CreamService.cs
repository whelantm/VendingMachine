using Model;

namespace Service
{
    public class CreamService : ICondimentService
    {
        private IApplicationSettingsService _appSettings;

        public CreamService(IApplicationSettingsService appSettings)
        {
            _appSettings = appSettings;
        }

        public bool IsCondimentValid(int quantity)
        {
            if (quantity >= 0 && quantity <= _appSettings.MaxCream)
                return true;

            return false;
        }

        public Condiment OrderCondiment(int quantity)
        {
            Condiment condiment = new Condiment()
            {
                Quantity = quantity,
                CondimentType = CondimentType.Cream,
                Price = quantity * _appSettings.CreamPrice
            };
            return condiment;
        }
      
    }
}
