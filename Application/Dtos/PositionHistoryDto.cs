namespace Application.Dtos
{
    public class PositionHistoryDto
    {
        public int EmployeeId { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
