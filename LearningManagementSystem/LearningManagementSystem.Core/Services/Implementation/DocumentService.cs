using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class DocumentService : IDocumentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DocumentService> _logger;

        public DocumentService(AppDbContext context, IMapper mapper, ILogger<DocumentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<DocumentModel>> AddDocumentAsync(DocumentModel document)
        {
            ArgumentNullException.ThrowIfNull(document);
            var userExist = await _context.Users.FirstOrDefaultAsync(f => f.Id.Equals(document.UserId));
            if (userExist is null)
            {
                return new Response<DocumentModel>()
                {
                    IsSuccessful = false,
                    Error = "User not found"
                };
            }

            document.DateAdded = DateTime.Parse(DateTime.Today.ToShortDateString());
            document.DateOfIssue = DateTime.Parse(document.DateOfIssue.ToShortDateString());
            if (document.DateOfExpiration.HasValue)
            {
                document.DateOfExpiration = DateTime.Parse(document.DateOfExpiration.Value.ToShortDateString());
            }
            var entity = _mapper.Map<Document>(document);
            await _context.Documents.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New document has been successfully added");
            return new Response<DocumentModel>()
            {
                IsSuccessful = true,
                Data = document
            };
        }

        public async Task<IEnumerable<DocumentModel>> GetDocumentsByFilterAsync(DocumentQueryModel? query = null)
        {
            var queryable = _context.Documents.AsQueryable();

            if (query is null)
            {
                return _mapper.Map<IEnumerable<DocumentModel>>(await queryable.ToListAsync());
            }
            if (query.UserId.HasValue)
            {
                queryable = queryable.Where(i => i.UserId.Equals(query.UserId));
            }

            if (query.Name is not null)
            {
                queryable = queryable.Where(i => i.Name.Contains(query.Name));
            }

            if (query.DateAdded.HasValue)
            {
                queryable = queryable.Where(i =>
                    i.DateAdded.Equals(query.DateAdded.Value));
            }

            if (query.DateOfIssue.HasValue)
            {
                queryable = queryable.Where(i =>
                    i.DateOfIssue.Equals(query.DateOfIssue!.Value));
            }
            if (query.DateOfExpiration.HasValue)
            {
                queryable = queryable.Where(i=>i.DateOfExpiration != null).Where(i =>
                    i.DateOfExpiration.Value.Equals(query.DateOfExpiration!.Value));
            }

            if (query.DocumentType.HasValue)
            {
                queryable = queryable.Where(i => i.DocumentType.Equals(query.DocumentType));
            }
            _logger.LogInformation("Getting documents from filter");
            var res = await queryable.ToListAsync();
            return _mapper.Map<IEnumerable<DocumentModel>>(res);
        }

        public async Task RemoveDocumentByIdAsync(Guid id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (document == null)
            {
                throw new Exception("Document does not exist");
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New document has been successfully removed");
        }

        public async Task<DocumentModel> GetDocumentByIdAsync(Guid id)
        {
            var entity = await _context.Documents.FirstOrDefaultAsync(f => f.Id.Equals(id));
            _logger.LogInformation("Getting document by id");
            return _mapper.Map<DocumentModel>(entity);
        }
    }
}
