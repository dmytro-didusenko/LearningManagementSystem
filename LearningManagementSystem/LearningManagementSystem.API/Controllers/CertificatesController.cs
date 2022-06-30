using DinkToPdf;
using DinkToPdf.Contracts;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Certificate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateService _certificateService;
        private IConverter _converter;

        public CertificatesController(ICertificateService certificateService, IConverter converter)
        {
            _certificateService = certificateService;
            _converter = converter;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetAllAsync()
        { 
            var certificates = await _certificateService.GetAllAsync();
            return Ok(certificates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var certificate = await _certificateService.GetByIdAsync(id);
            return Ok(certificate);
        }

        [HttpGet("students/{id}")]
        public async Task<ActionResult<IEnumerable<CertificateModel>>> GetByStudentId(Guid studentId)
        {
            var certificates = await _certificateService.GetByStudentIdAsync(studentId);
            return Ok(certificates);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadByIdInPdf(Guid id)
        {
            var response = await _certificateService.CreateCertificateInPdf(id);
            if (response.Error is not null)
            {
                return BadRequest(response.Error.ErrorMessage);
            }
            return File(response.Data.data, "application/pdf", response.Data.fileName);     //to download
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> OpenPdfById(Guid id)
        {
            var response = await _certificateService.CreateCertificateInPdf(id);
            if (response.Error is not null)
            {
                return BadRequest(response.Error.ErrorMessage);
            }
            return File(response.Data.data, "application/pdf");
        }

        //for test purposes only
        [HttpPost]
        public async Task<IActionResult> AddCertificateAsync([FromBody] CertificateModel certificateModel)
        {
            var certificate = await _certificateService.AddAsync(certificateModel);
            return CreatedAtAction(nameof(AddCertificateAsync), certificate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody] CertificateModel certificateModel)
        {
            certificateModel.Id = id;
            await _certificateService.UpdateAsync(certificateModel);
            return Ok(certificateModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            await _certificateService.DeleteAsync(id);
            return Ok();
        }
    }
}