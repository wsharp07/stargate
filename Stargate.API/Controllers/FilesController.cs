using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stargate.API.Data;
using Stargate.API.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Stargate.API.Data.Repository;
using Stargate.API.Models;
using Stargate.API.Services;

namespace Stargate.API.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IStargateRepository _repo;
        private readonly ILogger<FilesController> _logger;
        private readonly IMapper _mapper;
        private readonly IFileUploader _fileUploader;

        public FilesController(IStargateRepository repo, 
            ILogger<FilesController> logger,
            IMapper mapper, 
            StargateContext context, 
            IFileUploader fileUploader)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileUploader = fileUploader ?? throw new ArgumentNullException(nameof(fileUploader));
        }
        // GET api/files
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(
                    _mapper.Map<IEnumerable<Data.Entities.File>, IEnumerable<FileViewModel>>(
                        _repo.GetFiles()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get files: {ex}");
                return StatusCode(500, "Failed to get files");
            }
        }

        // GET api/files/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                var dbFile = _repo.GetFileById(id);

                if (dbFile == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<Data.Entities.File, FileViewModel>(dbFile));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get file '{id}': {ex}");
                return StatusCode(500, $"Failed to get file '{id}'");
            }
        }

        // POST api/files
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file was provided");

                var fileUploadResult = await _fileUploader.UploadAsync(file);

                var fileEntity = CreateFile(file, fileUploadResult);

                var response = new UploadResponseViewModel
                {
                    Success = true,
                    FileName = fileEntity.FileName,
                    Uri = fileEntity.ExternalUri
                };

                response.Id = await UpdatePost(fileEntity);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "An error occurred while trying to upload the file");
            }
        }

        private Data.Entities.File CreateFile(IFormFile file, FileReference reference)
        {
            var fileEntity = new Data.Entities.File
            {
                FileName = file.FileName,
                FileExtension = Path.GetExtension(file.FileName),
                ContentType = file.ContentType,
                ExternalUri = reference.Uri,
                FileSizeBytes = file.Length
            };

            return fileEntity;
        }

        private async Task<int> UpdatePost(Data.Entities.File fileEntity)
        {
            var existingFile = _repo.GetFileByFileName(fileEntity.FileName);

            if (existingFile != null)
                return existingFile.Id;

            await _repo.AddFileAsync(fileEntity);
            _repo.SetShortUri(fileEntity.Id);
            return fileEntity.Id;
        }
    }
}