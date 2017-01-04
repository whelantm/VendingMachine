namespace Service
{
    public interface IApplicationSettingsService
    {
        decimal CreamPrice { get; }
        decimal LargeCoffeePrice { get; }
        int MaxCream { get; }
        int MaxSugar { get; }
        decimal MediumCoffeePrice { get; }
        decimal SmallCoffeePrice { get; }
        decimal SugarPrice { get; }
    }
}