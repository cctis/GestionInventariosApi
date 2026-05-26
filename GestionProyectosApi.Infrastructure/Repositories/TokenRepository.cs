using Dapper;
using GestionProyectosApi.Domain.Models.Dto;
using GestionProyectosApi.Infrastructure.Repositories._UnitOfWork;
using GestionProyectosApi.Infrastructure.Repositories.Interfaces;

namespace GestionProyectosApi.Infrastructure.Repositories
{
    public class TokenRepository : Repository, ITokenRepository
    {
        public TokenRepository()
        {

        }

        public TokenRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public JwtUser GetUserByUserName(string User)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("User", User);
            string sql = @"select Login, Password, Salt from Usuarios where Login = @User";
            return Get<JwtUser>(sql, prms);
        }

        public Claims GetRolByUser(string User)
        {
            DynamicParameters prms = new DynamicParameters();
            prms.Add("User", User);
            string sql = @"SELECT U.Nombres,
                            U.Email,
                            UR.Rol
                            FROM Usuarios U
                            INNER JOIN UsuariosRol UR ON UR.IdUsuariosRol = U.Rol
                            WHERE Login = @User";
            return Get<Claims>(sql, prms);
        }
    }
}
