namespace Application.Dtos
{
    public class PositionHistoryDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public int EmployeeType { get; set; }
        public int? DepartmentId { get; set; }
    }
}
