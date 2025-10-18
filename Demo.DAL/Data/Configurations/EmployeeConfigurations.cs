using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models.EmployeeModel;
using Demo.DAL.Models.Shared;

namespace Demo.DAL.Data.Configurations
{
    public class EmployeeConfigurations : BaseEntityConfigurations<Employee> , IEntityTypeConfiguration<Employee>
    {
        public new void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("nvarchar(50)");
            builder.Property(E => E.Address).HasColumnType("nvarchar(100)");
            builder.Property(E => E.Salary).HasColumnType("decimal(10,2)");
            builder.Property(E => E.Gender)
                .HasConversion((EG) => EG.ToString(),
                (_gender => (EmployeeGender)Enum.Parse(typeof(EmployeeGender), _gender)));

            builder.Property(E => E.EmployeeType)
                .HasConversion((EG) => EG.ToString(),
                (_E_Type => (EmployeeType)Enum.Parse(typeof(EmployeeType), _E_Type)));

            base.Configure(builder);
        }
    }
}
