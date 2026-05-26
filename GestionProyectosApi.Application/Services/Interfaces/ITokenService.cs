using GestionProyectosApi.Domain.Models.Dto;

namespace GestionProyectosApi.Application.Services.Interfaces
{
    public interface ITokenService
    {
        bool Authentication(string User, string Password);
        ResultOperation<Claims> GetRolByUser(string User);
    }
}
