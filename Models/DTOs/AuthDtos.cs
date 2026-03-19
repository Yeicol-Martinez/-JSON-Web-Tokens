using System.ComponentModel.DataAnnotations;

namespace UsuariosAPI.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MinLength(4, ErrorMessage = "El username debe tener al menos 4 caracteres.")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [MinLength(2)]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [MaxLength(200)]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es requerida.")]
        public DateTime FechaDeNacimiento { get; set; }
    }

    public class RefreshRequestDto
    {
        [Required(ErrorMessage = "El access token es requerido.")]
        public string AccessToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "El refresh token es requerido.")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string AccessToken  { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Username     { get; set; } = string.Empty;
        public int    UsuarioId    { get; set; }
    }
}
