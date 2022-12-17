using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Certificate;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class CertificateService : ICertificateService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private readonly ILogger<CertificateService> _logger;

        public CertificateService(AppDbContext context, IMapper mapper, 
                                  ILogger<CertificateService> logger, IConverter converter)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _converter = converter;
        }

        public async Task<Response<CertificateModel>> AddAsync(CertificateModel certificateModel)
        {
            ArgumentNullException.ThrowIfNull(certificateModel);

            var certificate = _mapper.Map<Certificate>(certificateModel);
            await _context.AddAsync(certificate);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New certificate[id]:{certificateModel.Id} was added to the database");
            return Response<CertificateModel>.GetSuccess(certificateModel);
        }

        public async Task<Response<(string fileName, byte[] data)>> CreateCertificateInPdf(Guid id)
        {
            var certificate = await GetByIdAsync(id);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Certificate"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = CertificateGenerator.GetHTMLCertificate(certificate).ToString(),
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileName = $"{globalSettings.DocumentTitle}.pdf";
            var file = _converter.Convert(pdf);

            return Response<(string fileName, byte[] data)>.GetSuccess((fileName, file));
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Certificates.FindAsync(id);
            ArgumentNullException.ThrowIfNull(entity);

            if (_context.Entry(entity).State == EntityState.Detached)
                _context.Certificates.Attach(entity);

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Certificate[id]:{id} was removed from the database");
        }

        public async Task<IEnumerable<CertificateModel>> GetAllAsync()
        {
            var certificates = await _context.Certificates
                .Include(c => c.Student)
                    .ThenInclude(s => s.User)
                .Include(c => c.Course)
                .ToListAsync();

            if (certificates is null)
                throw new NotFoundException("No certificates found in database.");
            
            return _mapper.Map<IEnumerable<CertificateModel>>(certificates);
        }

        public async Task<CertificateModel> GetByIdAsync(Guid id)
        {
            var entity = await _context.Certificates
                .Include(c => c.Student)
                    .ThenInclude(s => s.User)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);

            ArgumentNullException.ThrowIfNull(entity);

            return _mapper.Map<CertificateModel>(entity);
        }

        public async Task<IEnumerable<CertificateModel>> GetByStudentIdAsync(Guid studentId)
        {
            var certificates = await _context.Certificates.Where(c => c.StudentId == studentId).ToListAsync();

            if (certificates is null || certificates.Count == 0)
                throw new NotFoundException("This student has no certificates yet.");

            return _mapper.Map<IEnumerable<CertificateModel>>(certificates);
        }

        public async Task UpdateAsync(CertificateModel certificateModel)
        {
            ArgumentNullException.ThrowIfNull(certificateModel);

            var entity = _mapper.Map<Certificate>(certificateModel);
            _context.Certificates.Update(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Certificate[id]:{entity.Id} was updated in the database");
        }
    }
}