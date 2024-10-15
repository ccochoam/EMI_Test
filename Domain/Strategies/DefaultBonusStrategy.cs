using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Strategies
{
    /// <summary>
    /// Estrategia de cálculo del bono por defecto, para usuarios que apenas se van a crear
    /// </summary>
    public class DefaultBonusStrategy: IBonusStrategy
    {
        public decimal CalculateBonus(Employee employee)
        {
            // Retorna un bono por defecto (por ejemplo, 0) si no se ha asignado un tipo claro
            return 0;
        }
    }
}
