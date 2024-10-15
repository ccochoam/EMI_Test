using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    //Clase para el objeto User
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } //Almacena la contraseña del usuario
        public string HashKey { get; set; } //Almacena la clave que se utiliza como parte de un proceso de hashing seguro. Clave segura para operaciones de cifrado y verificación de la constraseña del usuario
        public int Role { get; set; } //según la posición del empleado 1 = Admin, 2 = User
    }
}
