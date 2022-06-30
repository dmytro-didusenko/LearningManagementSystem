using LearningManagementSystem.Domain.Models.Certificate;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ICertificateService
    {

        Task<IEnumerable<CertificateModel>> GetAllAsync();

        Task<CertificateModel> GetByIdAsync(Guid id);

        Task<IEnumerable<CertificateModel>> GetByStudentIdAsync(Guid studentId);

        Task<Response<(string fileName, byte[] data)>> CreateCertificateInPdf(Guid id);

        /// <summary>
        /// FOR TEST PURPOSES ONLY!
        /// </summary>
        /// <param name="certificateModel"></param>
        /// <returns></returns>
        Task<Response<CertificateModel>> AddAsync(CertificateModel certificateModel);

        Task UpdateAsync(CertificateModel сertificateModel);

        Task DeleteAsync(Guid id);
    }
}