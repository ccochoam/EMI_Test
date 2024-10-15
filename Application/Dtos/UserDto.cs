using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string Username { get; set; } //Propiedad para almacenar el usuario, se define que el usuario sea el mismo email del usuario entonces se valida el formato de correo válido

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()_+])[A-Za-z\\d!@#$%^&*()_+]{8,}$",
            ErrorMessage = "The Password must contain at least one uppercase letter, one lowercase letter, one number, one special character, and must not be less than 8 characters.")]
        public string Password { get; set; } //Propiedad para almacenar la contraseña, se valida el formato con al menos una mayúscula, un número y un caracter especial
        public int Role { get; set; } //Propiedad para almacenar el rol del usuario, 1 para Admin y 2 para User

        public string GetPassword()
        {
            return Password;
        }
    }
}
