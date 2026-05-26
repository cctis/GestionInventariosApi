using GestionProyectosApi.Domain.Models.Dto;

namespace GestionProyectosApi.Infrastructure.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        JwtUser GetUserByUserName(string User);
        Claims GetRolByUser(string User);
    }
}
