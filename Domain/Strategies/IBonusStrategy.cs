using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Strategies
{
    /// <summary>
    /// Interface para definir el método de cálculo del bono del empleado
    /// </summary>
    public interface IBonusStrategy
    {
        decimal CalculateBonus(Employee employee);
    }
}
