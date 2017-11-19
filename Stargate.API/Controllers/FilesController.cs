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
                return BadRequest("Failed to get files");
            }
        }

        // GET api/files/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
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

        // POST api/files
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file was provided");

                var result = await _fileUploader.UploadAsync(file);

                var response = new UploadResponseViewModel
                {
                    Success = true,
                    FileName = result.FileName,
                    Uri = result.Uri
                };

                var fileEntity = new Data.Entities.File
                {
                    FileName = file.FileName,
                    FileExtension = Path.GetExtension(file.FileName),
                    ContentType = file.ContentType,
                    ExternalUri = result.Uri,
                    FileSizeBytes = file.Length
                };


                var existingFile = _repo.GetFileByFileName(file.FileName);

                if (existingFile == null)
                {
                    await _repo.AddFileAsync(fileEntity);
                    response.Id = fileEntity.Id;
                    _repo.SetShortUri(response.Id);
                }
                else
                {
                    response.Id = existingFile.Id;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}