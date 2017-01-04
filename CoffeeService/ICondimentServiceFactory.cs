using Model;

namespace Service
{
    public interface ICondimentServiceFactory
    {
        ICondimentService Create(CondimentType type);
    }
}
