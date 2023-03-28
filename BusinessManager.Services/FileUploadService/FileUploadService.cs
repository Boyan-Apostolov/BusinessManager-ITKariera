using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetZeroCore.Net;
using BusinessManager.Services.DTOs.TimeOffs;
using Dropbox.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Dropbox.Api.Files;
using static Dropbox.Api.Files.SearchMatchType;
using Microsoft.AspNetCore.Mvc;
using static Dropbox.Api.Files.SearchMatchTypeV2;

namespace BusinessManager.Services.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IConfiguration configuration;
        private readonly DropboxClient _dropboxClient;
        public FileUploadService(IConfiguration configuration)
        {
            this.configuration = configuration;

            var dropBoxAccount = this.configuration.GetSection("Dropbox");
            _dropboxClient = new DropboxClient(
                dropBoxAccount["API_Key"]
                );
        }

        public async Task<string> UploadFileAsync(IFormFile externalFile)
        {
            if (string.IsNullOrWhiteSpace(externalFile?.FileName)) return "";

            using (var mem = externalFile.OpenReadStream())
            {
                var url = "/files" + $"/{Guid.NewGuid().ToString()}/" + externalFile.FileName;
                var updated = await _dropboxClient.Files.UploadAsync(
                    url,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                return url;
            }
        }

        public async Task<FileContentResult> DownloadFileAsync(string fileUrl)
        {
            using (var response = await _dropboxClient.Files.DownloadAsync(fileUrl))
            {
                var bytes = await response.GetContentAsByteArrayAsync();

                return new FileContentResult(bytes, MimeTypeNames.ApplicationOctetStream)
                {
                    FileDownloadName = fileUrl.Split("/").LastOrDefault(),
                };
            }
        }
    }
}
