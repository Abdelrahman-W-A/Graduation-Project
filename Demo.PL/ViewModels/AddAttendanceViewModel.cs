namespace Demo.PL.ViewModels
{
    public class AddAttendanceViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? CheckIn { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? CheckOut { get; set; }

        public bool IsAbsent { get; set; }
    }
}
