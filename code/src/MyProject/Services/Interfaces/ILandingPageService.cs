using MyProject.Models.Dtos;

namespace MyProject.Services.Interfaces;

public interface ILandingPageService
{
    Task<LandingPageResponse> GetLandingPageContentAsync();
}