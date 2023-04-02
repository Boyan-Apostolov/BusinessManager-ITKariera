using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManager.Services.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> EditFileAsync(string fileUrl, IFormFile externalFile);

        Task<string> UploadFileAsync(IFormFile externalFile);

        Task<FileContentResult> DownloadFileAsync(string fileUrl);
    }
}
