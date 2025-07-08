namespace MFL_VisitorManagement.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAuthentication>()
                .HasKey(x => x.Id);
        
        modelBuilder.Entity<VisitorDetailsDto>()
                .HasNoKey();

        modelBuilder.Entity<UserAuthentication>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<UserAuthentication>()
                   .Property(e => e.IsActive)
                   .HasConversion(v => v == true ? '1' : '0', v => v == '1'); 
        modelBuilder.Entity<UserAuthentication>()
                   .Property(e => e.IsDelete)
                   .HasConversion(v => v == true ? '1' : '0', v => v == '1');

        modelBuilder.Entity<UserAuthentication>().ToTable("USERAUTHENTICATION");

        modelBuilder.Entity<DepartmentMaster>(entity =>
        {
            entity.ToTable("DEPARTMENTMASTER");

            entity.HasKey(d => d.DepartmentId);

            entity.Property(d => d.DepartmentId).HasColumnName("DEPARTMENTID");
            entity.Property(d => d.DepartmentName).HasColumnName("DEPARTMENTNAME");
            entity.Property(d => d.IsDelete).HasColumnName("ISDELETE");
            entity.Property(d => d.IsActive).HasColumnName("ISACTIVE");
        });

        modelBuilder.Entity<IdProofMaster>(entity =>
        {
            entity.ToTable("IDPROOFMASTER");
            entity.HasKey(Id => Id.IdProofId);

            entity.Property(Id => Id.IdProofId).HasColumnName("IDPROOFID");
            entity.Property(Id => Id.IdProofType).HasColumnName("IDPROOFTYPE");
            entity.Property(Id => Id.IsDelete).HasColumnName("ISDELETE");
            entity.Property(Id => Id.IsActive).HasColumnName("ISACTIVE");
        });
    }

    public DbSet<UserAuthentication> UserAuthentication { get; set; }  
    public DbSet<UserLoginResponse> UserLoginResponse { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<IdProofMaster> IdProofMasters { get; set; }
    public DbSet<DepartmentMaster>  DepartmentMasters { get; set; }
    public DbSet<VisitorDetailsDto>  VisitorDetailsDtos { get; set; }



}
