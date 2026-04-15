using MyProject.Models.Dtos;

namespace MyProject.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationResponse> SubmitApplicationAsync(ApplicationRequest request);
    Task<ApplicationStatusResponse> GetApplicationStatusAsync(Guid applicationId);
    Task<ApplicationResponse> UpdateApplicationAsync(Guid applicationId, ApplicationUpdateRequest request);
}