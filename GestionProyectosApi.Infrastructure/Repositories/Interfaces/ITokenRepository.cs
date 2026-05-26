using GestionInventariosApi.Domain.Models.Dto;

namespace GestionInventariosApi.Infrastructure.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        JwtUser GetUserByUserName(string User);
        Claims GetRolByUser(string User);
    }
}
