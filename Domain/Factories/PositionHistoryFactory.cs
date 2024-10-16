using Domain.Entities;

namespace Domain.Factories
{
    /// <summary>
    /// clase "fábrica" para encapsular la creación de objetos. Se pretende permitir la inversión de contro e inyección de dependencias al tener centralizado la creación del objeto según su posición (cargo).
    /// </summary>
    public class PositionHistoryFactory
    {
        public static PositionHistory CreatePositionHistory(int id, int employeeId, string position, string startDate, string endDate, int employeeType, int? departmentId) 
        {
            return new PositionHistory
            {
                Id = id,
                EmployeeId = employeeId,
                Position = position,
                StartDate = DateTime.ParseExact(startDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                EndDate = string.IsNullOrWhiteSpace(endDate) ? null : DateTime.ParseExact(endDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                EmployeeType = employeeType,
                DepartmentId = departmentId
            };
        }
    }
}
