using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Demo.BLL.Data_Transfer_Objects__DTO_;
using Demo.BLL.Data_Transfer_Objects__DTOs_;
using Demo.BLL.Data_Transfer_Objects__DTOs_.DepartmentDTOs;
using Demo.BLL.Data_Transfer_Objects__DTOs_.EmployeeDTOs;
using Demo.BLL.Factories.EmployeesFactory;
using Demo.BLL.Services.Attachment_Services;
using Demo.BLL.Services.AttendanceServices;
using Demo.DAL.Data.Repostitories.EntityTypes;
using Demo.DAL.Models.EmployeeModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Demo.BLL.Services.EmployeeServices
{
    public class EmployeeServices(IEntityTypeRepo<Employee> _entity , IMapper _mapper , IAttachmentServices _attachmentServices,IAttendanceService _attendanceService) : IEmployeeServices
    {


        public int AddEmployee(AddNewEmployeeDTO createdEmployee)
        {
            if (createdEmployee.Image != null)
            {
                createdEmployee.ImageName = _attachmentServices.Upload(createdEmployee.Image, "Images");
            }

            if (createdEmployee.CvFile != null)
            {
                createdEmployee.CvFileName = _attachmentServices.Upload(createdEmployee.CvFile, "CVs");
            }

            var employee = _mapper.Map<Employee>(createdEmployee);

            employee.ImageName = createdEmployee.ImageName;
            employee.CvFileName = createdEmployee.CvFileName;

            return _entity.Add(employee);
        }




        public bool DeleteEmployee(int id)
        {
            // Get the employee
            var employee = _entity.GetById(id);
            if (employee == null) return false;

            // Optional: delete related attendances first
            var attendances = _attendanceService.GetByEmployeeId(id);
            foreach (var att in attendances)
            {
                _attendanceService.DeleteAttendance(att.Id);
            }

            // Delete employee
            int result = _entity.Remove(employee);
            return result > 0;
        }



        public IEnumerable<GetEmployeeDTO> GetAllEmployees(string? name)
        {
            //var Emp = _entity.GetAll(E => E.Name.ToLower().Contains(name.ToLower()));



            ////var EmpToReturn = Emp.Select(E => new GetEmployeeDTO()
            ////{
            ////    Id = E.Id,
            ////    Name = E.Name,
            ////    Age = E.age,
            ////    IsActive = E.IsActive,
            ////    Salary = E.Salary,
            ////    Email = E.Email,
            ////    gender = E.Gender,
            ////    EmployeeType = E.EmployeeType
            ////});

            //var EmpToReturn = _mapper.Map<IEnumerable<Employee> , IEnumerable<GetEmployeeDTO>>(Emp);

            //return EmpToReturn;

            IEnumerable<Employee> Employees;
            if (string.IsNullOrWhiteSpace(name))
            {
                Employees = _entity.GetAll();
            }
            else
            {
                Employees = _entity.GetAll(E => E.Name.ToLower().Contains(name.ToLower()));
            }
            var EmpToReturn = _mapper.Map<IEnumerable<Employee>, IEnumerable<GetEmployeeDTO>>(Employees);
            return EmpToReturn;
        }

        public GetEmployeeByIdDTO? GetEmployeeById(int id)
        {
            var employee = _entity.GetById(id);

            if (employee is null) return null;
            else
            {
                //var ReturnEmployee = new GetEmployeeByIdDTO()
                //{
                //    Id = employee.Id,
                //    Name = employee.Name,
                //    Age = employee.age,
                //    Address = employee.Address,
                //    IsActive = employee.IsActive,
                //    Salary = employee.Salary,
                //    Email = employee.Email,
                //    PhoneNumber = employee.PhoneNumber,
                //    HiringDate = DateOnly.FromDateTime(employee.HiringDate),
                //    gender = employee.Gender,
                //    EmployeeType = employee.EmployeeType,
                //    CreatedBy = 1,
                //    CreatedOn = DateTime.Now,
                //    LastModifiedBy = 1,
                //    LastModifiedOn = DateTime.Now

                //};

                var ReturnEmployee = _mapper.Map<Employee, GetEmployeeByIdDTO>(employee);

                return ReturnEmployee;
            }
        }

        public int UpdateEmployee(UpdatedEmployeeDTO updatedEmployee)
        {
            var existingEmployee = _entity.GetById(updatedEmployee.Id);
            if (existingEmployee == null) return 0;

            string imageFileName = existingEmployee.ImageName;
            string cvFileName = existingEmployee.CvFileName;

            // Handle CV update
            if (updatedEmployee.CvFile != null)
            {
                cvFileName = _attachmentServices.Upload(updatedEmployee.CvFile, "CVs");
            }

            // Handle Image update
            if (updatedEmployee.Image != null)
            {
                imageFileName = _attachmentServices.Upload(updatedEmployee.Image, "Images");
            }
            else if (!string.IsNullOrEmpty(updatedEmployee.CapturedImage))
            {
                var base64Data = updatedEmployee.CapturedImage.Replace("data:image/png;base64,", string.Empty);
                var bytes = Convert.FromBase64String(base64Data);
                imageFileName = $"{Guid.NewGuid()}.png";
                var filePath = Path.Combine("wwwroot/Files/Images", imageFileName);
                File.WriteAllBytes(filePath, bytes);
            }

            // Assign filenames back to DTO
            updatedEmployee.ImageName = imageFileName;
            updatedEmployee.CvFileName = cvFileName;

            // Map to entity
            var employee = _mapper.Map(updatedEmployee, existingEmployee);
            employee.ImageName = imageFileName;
            employee.CvFileName = cvFileName;

            return _entity.Update(employee);
        }



    }


}
