using System.Threading.Tasks;

namespace Photos.Shared.Services
{
    public interface IStorageService
    {
        Task SaveData(object data);

        Task<string> GetData(string key);
    }
}
