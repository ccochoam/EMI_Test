using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Strategies
{
    /// <summary>
    /// Estrategia que implementa el método de cálculo del bono para los empleados regulares
    /// </summary>
    public class RegularEmployeeBonusStrategy: IBonusStrategy
    {
        public decimal CalculateBonus(Employee employee)
        {
            return employee.Salary * 0.10M;
        }
    }
}
