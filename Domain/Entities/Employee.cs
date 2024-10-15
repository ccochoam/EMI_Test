using Domain.Enums;
using Domain.Strategies;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CurrentPosition { get; set; }

        [Column(TypeName = "decimal(18,4)")] 
        public decimal Salary { get; set; }
        public List<PositionHistory> PositionHistories { get; set; }

        [JsonIgnore] 
        public IBonusStrategy BonusStrategy { get; set; } // Se crea para asignar a través del Stragety Pattern, la lógica para el cálculo del bono según la posición actual del empleado. La asignación se hace en la función AssignBonusStrategy

        public Employee()
        {
            AssignBonusStrategy(); // Asigna la estrategia al crear el empleado
        }
        public decimal AnnualBonus //guarda el valor del bono anual del empleado, se asigna el valor al momento de llamar AssignBonusStrategy, es decir, al momento de asignar la estrategia de cálculo
        {
            get
            {
                // Si la estrategia de bono no está asignada, lanza una excepción
                if (BonusStrategy == null)
                    throw new InvalidOperationException("Bonus strategy is not assigned.");

                // Calcula el bono usando la estrategia asignada
                return BonusStrategy.CalculateBonus(this);
            }
        }
        public void AssignBonusStrategy()
        {
            var currentPosition = PositionHistories?.FirstOrDefault(cp => cp.Id == CurrentPosition);
            if (currentPosition?.EmployeeType == (int)EmployeeType.RegularEmployee)
                BonusStrategy = new RegularEmployeeBonusStrategy();
            else if (currentPosition?.EmployeeType == (int)EmployeeType.ManagerEmployee)
                BonusStrategy = new ManagerEmployeeBonusStrategy();
            else
                // Estrategia por defecto si no se asigna un tipo de empleado específico
                BonusStrategy = new DefaultBonusStrategy(); // Estrategia por defecto
        }
    }
}
