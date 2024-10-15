using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// Tipo de rol del usuario para validar autorizaciones de acceso a los endpoint.
    /// </summary>
    public enum UserRole
    {
        Admin = 1,
        User = 2
    }
}
