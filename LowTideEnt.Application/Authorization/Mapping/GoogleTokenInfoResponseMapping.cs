using LowTideEnt.Application.Authorization.Dto;
namespace LowTideEnt.Application.Authorization.Mapping
{
    public static class GoogleTokenInfoMapping
    {
        public static GoogleAuthResponse ToResponse(this GoogleTokenInfo tokenInfo) =>
            new GoogleAuthResponse(tokenInfo.Sub, tokenInfo.Email, tokenInfo.Name, tokenInfo.Picture);
    }
}
