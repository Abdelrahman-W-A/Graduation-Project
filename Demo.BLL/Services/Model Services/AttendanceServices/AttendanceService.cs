using AutoMapper;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.BLL.Services.AttendanceServices;
using Demo.DAL.Data.DbContex;
using Demo.DAL.Models.AttendanceModel;

public class AttendanceService : IAttendanceService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AttendanceService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public int AddAttendance(DailyAttendanceDTO dto)
    {
        var entity = _mapper.Map<Attendance>(dto);
        _context.Attendances.Add(entity);
        _context.SaveChanges();
        return entity.Id;
    }

    public void UpdateAttendance(DailyAttendanceDTO dto)
    {
        var entity = _context.Attendances.FirstOrDefault(a => a.Id == dto.Id);
        if (entity == null) return;

        _mapper.Map(dto, entity);
        _context.SaveChanges();
    }


    public bool DeleteAttendance(int id)
    {
        var entity = _context.Attendances.Find(id);
        if (entity == null) return false;

        _context.Attendances.Remove(entity);
        _context.SaveChanges();
        return true;
    }

    public DailyAttendanceDTO? GetById(int id)
    {
        return _context.Attendances
                       .Where(a => a.Id == id)
                       .Select(a => new DailyAttendanceDTO
                       {
                           Id = a.Id,
                           EmployeeId = a.EmployeeId,
                           Date = a.Date,
                           CheckIn = a.CheckIn,
                           CheckOut = a.CheckOut,
                           IsAbsent = a.IsAbsent
                       })
                       .FirstOrDefault();
    }


    public IEnumerable<DailyAttendanceDTO> GetByEmployeeId(int employeeId)
    {
        return _context.Attendances
            .Where(a => a.EmployeeId == employeeId)
            .Select(a => new DailyAttendanceDTO
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                Date = a.Date,
                CheckIn = a.CheckIn,
                CheckOut = a.CheckOut,
                IsAbsent = a.IsAbsent
            }).ToList();
    }

}
