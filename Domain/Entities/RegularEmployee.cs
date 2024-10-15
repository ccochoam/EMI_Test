using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    //Entidad para manejar los empleados de tipo RegularEmployee, pensado en principio para el cálculo del bono anual de los empleados regulares. Hereda las propiedades de Employee
    public class RegularEmployee: Employee
    {
        public RegularEmployee() { }
    }
}
