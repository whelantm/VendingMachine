using Model;

namespace Service
{
    public class SugarService : ICondimentService
    {
        private IApplicationSettingsService _appSettings;

        public SugarService(IApplicationSettingsService appSettings)
        {
            _appSettings = appSettings;
        }

        public bool IsCondimentValid(int quantity)
        {
            if (quantity >= 0 && quantity <= _appSettings.MaxSugar)
                return true;

            return false;
        }

        public Condiment OrderCondiment(int quantity)
        {
            Condiment condiment = new Condiment()
            {
                Quantity = quantity,
                CondimentType = CondimentType.Sugar,
                Price = quantity * _appSettings.SugarPrice
            };
            return condiment;
        }
    }
}
