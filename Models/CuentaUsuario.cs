using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI.Models
{
    public class CuentaUsuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MinLength(4,  ErrorMessage = "El username debe tener al menos 4 caracteres.")]
        [MaxLength(50, ErrorMessage = "El username no puede superar los 50 caracteres.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
