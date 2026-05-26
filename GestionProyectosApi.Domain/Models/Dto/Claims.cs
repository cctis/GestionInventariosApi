namespace GestionInventariosApi.Domain.Models.Dto
{
    public class Claims
    {
        public int Id { get; set; }

        public string Nombres { get; set; }

        public string Email { get; set; }

        public string Rol { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
