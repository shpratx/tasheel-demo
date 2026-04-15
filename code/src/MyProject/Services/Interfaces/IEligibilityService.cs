using MyProject.Models.Dtos;

namespace MyProject.Services.Interfaces;

public interface IEligibilityService
{
    Task<EligibilityResponse> CheckEligibilityAsync(EligibilityRequest request);
}