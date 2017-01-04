using Model;

namespace Service
{
    public interface ICondimentService
    {
        Condiment OrderCondiment(int quantity);

        bool IsCondimentValid(int quantity);
    }
}
