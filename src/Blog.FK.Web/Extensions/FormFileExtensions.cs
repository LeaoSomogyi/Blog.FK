using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.FK.Web.Extensions
{
    /// <summary>
    /// Useful FormFile methods
    /// </summary>
    public static class FormFileExtensions
    {
        /// <summary>
        /// Convert a Microsoft.AspNetCore.Http.IFormFile object to Base 64 string
        /// </summary>
        /// <param name="formFile">Microsoft.AspNetCore.Http.IFormFile received from form</param>
        /// <returns>A base 64 string from file bytes</returns>
        public static async Task<string> ConvertToBase64(this IFormFile formFile)
        {
            if (formFile == null) return string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                var fileBytes = memoryStream.ToArray();

                return Convert.ToBase64String(fileBytes);
            }
        }

        /// <summary>
        /// Get raw byte array from Microsoft.AspNetCore.Http.IFormFile object
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetRawBytes(this IFormFile formFile)
        {
            if (formFile == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                return memoryStream.ToArray();
            }
        }
    }
}
