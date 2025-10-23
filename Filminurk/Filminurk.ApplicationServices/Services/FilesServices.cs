using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Hosting;

namespace Filminurk.ApplicationServices.Services
{
    public class FilesServices : IFilesServices
    {
        public readonly IHostEnvironment _weblost;
        private readonly FilminurkTARpe24Context _context;

        public FilesServices(IHostEnvironment weblost, FilminurkTARpe24Context context)
        {
            _context = context;
            _weblost = weblost;
        }

        public void FilesToApi(MoviesDTO dto, Movie domain)
        {
            if (dto.Files != null  && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_weblost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_weblost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    string uploadsFolder = Path.Combine(_weblost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        FileToApi path = new FileToApi
                        {
                            ImageID = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            MovieID = domain.ID

                        };
                        _context.FilesToApi.AddAsync(path);

                    }
                }

            }

        }

        public async Task<FileToApi> RemoveImageFromApi(FileToApiDTO dto)
        {
            var ImageID = await _context.FilesToApi.FirstOrDefaultAsync(x => x.ImageID == dto.ImageID);
            var filePath = _weblost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\" + ImageID.ExistingFilePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FilesToApi.Remove(ImageID);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDTO[] dtos)
        {
            foreach (var dto in dtos)
            {
                RemoveImageFromApi(dto);
            }
            return null;
        }
    }
}
