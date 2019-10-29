using System.Threading.Tasks;

namespace Blog.FK.Domain.Interfaces
{
    public interface IIORepository
    {
        void CreateFile(byte[] content, string path, string name, string extension);

        Task<string> ReadFileAsync(string fullPath);
    }
}
