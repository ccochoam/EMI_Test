using System;

namespace Domain.Entities
{
    //Clase para el objeto PositionHistory
    public class PositionHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int EmployeeType { get; set; }
        public int? DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
