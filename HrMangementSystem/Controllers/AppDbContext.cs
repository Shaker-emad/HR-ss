using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace HrMangementSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<PerformanceProgress> PerformanceProgresses { get; set; }
        public DbSet<Objective> Objectives { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // تأكد من أنك قد قمت بإعداد الاتصال بشكل صحيح
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SHEKO;Initial Catalog=project28;Integrated Security=True;Encrypt=False", options =>
                {
                    options.CommandTimeout(400); // ضبط المهلة لـ 3 دقائق
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // إعداد العلاقات بين المديرين والأقسام
            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Department)
                .WithOne(d => d.Manager)
                .HasForeignKey<Department>(d => d.ManagerId);

            // إعداد العلاقات بين الموظفين والأقسام
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // إضافة بيانات افتراضية للمديرين
            modelBuilder.Entity<Manager>().HasData(
                new Manager { ManagerId = 1, FirstName = "John", LastName = "Doe", Position = "HR Manager", Email = "john.doe@example.com", Phone = "123-456-7890" },
                new Manager { ManagerId = 2, FirstName = "Jane", LastName = "Smith", Position = "IT Manager", Email = "jane.smith@example.com", Phone = "987-654-3210" },
                new Manager { ManagerId = 3, FirstName = "Bob", LastName = "Brown", Position = "Marketing Manager", Email = "bob.brown@example.com", Phone = "555-555-5555" },
                new Manager { ManagerId = 4, FirstName = "Alice", LastName = "White", Position = "Sales Manager", Email = "alice.white@example.com", Phone = "444-444-4444" },
                new Manager { ManagerId = 5, FirstName = "Tom", LastName = "Green", Position = "Finance Manager", Email = "tom.green@example.com", Phone = "333-333-3333" }
            );

            // إضافة بيانات افتراضية للأقسام
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Human Resources", Location = "Building A", ManagerId = 1 },
                new Department { DepartmentId = 2, DepartmentName = "IT", Location = "Building B", ManagerId = 2 },
                new Department { DepartmentId = 3, DepartmentName = "Marketing", Location = "Building C", ManagerId = 3 },
                new Department { DepartmentId = 4, DepartmentName = "Sales", Location = "Building D", ManagerId = 4 },
                new Department { DepartmentId = 5, DepartmentName = "Finance", Location = "Building E", ManagerId = 5 }
            );

            // إضافة بيانات افتراضية للموظفين
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, FirstName = "Michael", LastName = "Jordan", Position = "HR Specialist", Email = "michael.jordan@example.com", Phone = "111-111-1111", DepartmentId = 1, DateOfBirth = new DateTime(1985, 5, 15), Salary = 50000, HireDate = new DateTime(2020, 1, 10), Gender = "Male", Image = "michael_jordan.jpg", Certificate = null, Resume = null },
                new Employee { EmployeeId = 2, FirstName = "Serena", LastName = "Williams", Position = "IT Specialist", Email = "serena.williams@example.com", Phone = "222-222-2222", DepartmentId = 2, DateOfBirth = new DateTime(1990, 8, 25), Salary = 60000, HireDate = new DateTime(2021, 3, 15), Gender = "Female", Image = "serena_williams.jpg", Certificate = null, Resume = null },
                new Employee { EmployeeId = 3, FirstName = "LeBron", LastName = "James", Position = "Marketing Analyst", Email = "lebron.james@example.com", Phone = "333-333-3333", DepartmentId = 3, DateOfBirth = new DateTime(1988, 12, 30), Salary = 55000, HireDate = new DateTime(2019, 6, 22), Gender = "Male", Image = "lebron_james.jpg", Certificate = null, Resume = null },
                new Employee { EmployeeId = 4, FirstName = "Simone", LastName = "Biles", Position = "Sales Representative", Email = "simone.biles@example.com", Phone = "444-444-4444", DepartmentId = 4, DateOfBirth = new DateTime(1996, 3, 14), Salary = 48000, HireDate = new DateTime(2022, 2, 5), Gender = "Female", Image = "simone_biles.jpg", Certificate = null, Resume = null },
                new Employee { EmployeeId = 5, FirstName = "Lionel", LastName = "Messi", Position = "Finance Analyst", Email = "lionel.messi@example.com", Phone = "555-555-5555", DepartmentId = 5, DateOfBirth = new DateTime(1987, 6, 24), Salary = 70000, HireDate = new DateTime(2020, 8, 12), Gender = "Male", Image = "lionel_messi.jpg", Certificate = null, Resume = null }
            );

            // إضافة بيانات افتراضية للموظفين
            modelBuilder.Entity<Staff>().HasData(
                new Staff { StaffId = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", Password = "hashed_password1", Role = "HR", Image = "user-1.jpg" },
                new Staff { StaffId = 2, FirstName = "Bob", LastName = "Smith", Email = "bob.smith@example.com", Password = "hashed_password2", Role = "Manager", Image = "user-1.jpg" },
                new Staff { StaffId = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@example.com", Password = "hashed_password3", Role = "Employee", Image = "user-1.jpg" }
            );

            // إعداد نوع بيانات العمود للراتب
            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)");

            // إعداد العلاقات بين تقدم الأداء والأهداف
            modelBuilder.Entity<PerformanceProgress>()
                .HasOne(pp => pp.Objective)
                .WithMany(o => o.PerformanceProgresses)
                .HasForeignKey(pp => pp.ObjectiveID)
                .OnDelete(DeleteBehavior.Restrict); // منع الحذف المتسلسل
        }
    }
}
