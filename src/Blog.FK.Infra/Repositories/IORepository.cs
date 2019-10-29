using Blog.FK.Domain.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.FK.Infra.Repositories
{
    public class IORepository : IIORepository, IDisposable
    {
        #region "  IIORepository  "

        public void CreateFile(byte[] content, string path, string name, string extension)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var fileStream = File.Create($"{path}/{name}.{extension}"))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public async Task<string> ReadFileAsync(string fullPath)
        {
            using (var streamReader = new StreamReader(fullPath))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        #endregion

        #region "  IDisposable  "

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
