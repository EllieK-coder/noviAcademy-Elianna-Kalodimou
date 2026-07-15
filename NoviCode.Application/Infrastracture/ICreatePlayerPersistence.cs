

namespace NoviCode.Infrastracture
{
    public interface ICreatePlayerPersistence
    {
        Task Persist(Player player);
    }
}
