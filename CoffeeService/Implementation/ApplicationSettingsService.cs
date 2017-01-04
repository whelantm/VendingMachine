using System.Configuration;

namespace Service
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public decimal SmallCoffeePrice {  get { return GetdecimalAppSetting("SmallCoffeePrice", 1.75m); } }
        
        public decimal MediumCoffeePrice { get { return GetdecimalAppSetting("MediumCoffeePrice", 2.00m); } }
        
        public decimal LargeCoffeePrice { get { return GetdecimalAppSetting("LargeCoffeePrice", 2.25m); } }
        
        public decimal CreamPrice { get { return GetdecimalAppSetting("CreamPrice", 0.50m); } }
        
        public decimal SugarPrice { get { return GetdecimalAppSetting("SugarCoffeePrice", 0.25m); } }
        
        public int MaxSugar { get { return GetIntAppSetting("MaxSugar", 3); } }

        public int MaxCream { get {return GetIntAppSetting("MaxCream", 3); } }


        private int GetIntAppSetting(string key, int defaultValue)
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return int.Parse(value);
        }

        private decimal GetdecimalAppSetting(string key, decimal defaultValue)
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return decimal.Parse(value);
        }

    }
}
