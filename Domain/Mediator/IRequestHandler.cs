using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mediator
{
    /// <summary>
    /// Interfaz para manejar solicitudes de tipo <typeparamref name="TRequest"/> y devolver una respuesta de tipo <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de solicitud que será manejada.</typeparam>
    /// <typeparam name="TResponse">El tipo de respuesta que será devuelta.</typeparam>
    public interface IRequestHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
