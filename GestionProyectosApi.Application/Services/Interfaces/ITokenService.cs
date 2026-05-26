using GestionInventariosApi.Domain.Models.Dto;

namespace GestionInventariosApi.Application.Services.Interfaces
{
    public interface ITokenService
    {
        bool Authentication(string User, string Password);
        ResultOperation<Claims> GetRolByUser(string User);
    }
}
