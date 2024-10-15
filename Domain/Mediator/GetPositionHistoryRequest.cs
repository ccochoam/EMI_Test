using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mediator
{
    /// <summary>
    /// Solicitud para obtener el historial de posiciones por ID de empleado.Solicitud para obtener el historial de posiciones por ID de empleado.
    /// </summary>
    public class GetPositionHistoryRequest
    {
        public int PositionHistoryId { get; }

        public GetPositionHistoryRequest(int positionHistoryId) 
        {
            PositionHistoryId = positionHistoryId;
        }
    }
}
