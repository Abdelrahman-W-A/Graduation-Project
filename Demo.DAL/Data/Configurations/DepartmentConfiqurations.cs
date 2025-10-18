using Demo.DAL.Models.DepartmentModel;

namespace Demo.DAL.Data.Configurations
{
    public class DepartmentConfigurations : BaseEntityConfigurations<Department> , IEntityTypeConfiguration<Department>
    {
        public new void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(10,10);
            builder.Property(D => D.Name).HasColumnType("varchar(20)");
            builder.Property(D => D.Code).HasColumnType("varchar(20)");
            builder.HasMany(D=>D.Employees)
                   .WithOne(D => D.Department)
                   .HasForeignKey(D => D.DepartmentID)
                   .OnDelete(DeleteBehavior.NoAction);

            base.Configure(builder);
        }
    }
}
