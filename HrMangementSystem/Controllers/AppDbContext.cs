using HrManagementSystem.Models;
using HrMangementSystem.Models;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Document> Documents { get; set; }  // Document table
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Leave> leaves { get; set; }

        public DbSet<PerformanceReview> performanceReviews { get; set; }
        public object PerformanceReviews { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=SHEKO;Initial Catalog=project70;Integrated Security=True;Encrypt=False;Connect Timeout=300;", options =>
                {
                    options.CommandTimeout(400); // Adjust command timeout if needed
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PerformanceReview>()
    .Property(pr => pr.Rating)
    .HasColumnType("decimal(5, 2)"); // قم بتحديد الدقة والScale وفقًا لاحتياجاتك



            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<Objective>()
                .HasKey(o => o.ObjectiveID);

            modelBuilder.Entity<PerformanceProgress>()
                .HasKey(pp => pp.ProgressID);

            modelBuilder.Entity<Department>()
                .HasKey(d => d.DepartmentId);


            // إعداد العلاقات مع قيود الحذف
            modelBuilder.Entity<PerformanceProgress>()
                .HasOne(pp => pp.Employee)
                .WithMany(e => e.PerformanceProgresses)
                .HasForeignKey(pp => pp.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // أو DeleteBehavior.NoAction

            modelBuilder.Entity<PerformanceProgress>()
                .HasOne(pp => pp.Objective)
                .WithMany(o => o.PerformanceProgresses)
                .HasForeignKey(pp => pp.ObjectiveID)
                .OnDelete(DeleteBehavior.Restrict); // أو DeleteBehavior.NoAction

            // إعداد العلاقات الأخرى
            modelBuilder.Entity<Objective>()
                .HasOne(o => o.Department)
                .WithMany(d => d.Objectives)
                .HasForeignKey(o => o.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade); // يبقى الحذف هنا

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

            // Define the relationship for PerformanceReview
            modelBuilder.Entity<PerformanceReview>()
                .HasOne(pr => pr.Employee)
                .WithMany(e => e.PerformanceReviews)
                .HasForeignKey(pr => pr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // This allows cascading delete for EmployeeID

            modelBuilder.Entity<PerformanceReview>()
                .HasOne(pr => pr.Reviewer)
                .WithMany(m => m.PerformanceReviews)
                .HasForeignKey(pr => pr.ReviewerID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete for ReviewerID


            modelBuilder.Entity<PerformanceProgress>()
                .HasOne(pp => pp.Employee) // Assuming PerformanceProgress has an Employee navigation property
                .WithMany(e => e.PerformanceProgresses) // Assuming Employee has a PerformanceProgresses navigation property
                .HasForeignKey(pp => pp.EmployeeId) // Replace EmployeeID with the actual FK property in PerformanceProgress
                .OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed
                                                   // Define relationships for Attendance
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances) // تأكد من وجود خاصية Attendances في Employee
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // أو أي سلوك آخر تفضله

            // Seed data for Managers
            modelBuilder.Entity<Manager>().HasData(
                new Manager { ManagerId = 1, FirstName = "John", LastName = "Doe", Position = "HR Manager", Email = "john.doe@example.com", Phone = "123-456-7890" },
                new Manager { ManagerId = 2, FirstName = "Jane", LastName = "Smith", Position = "IT Manager", Email = "jane.smith@example.com", Phone = "987-654-3210" },
                new Manager { ManagerId = 3, FirstName = "Bob", LastName = "Brown", Position = "Marketing Manager", Email = "bob.brown@example.com", Phone = "555-555-5555" },
                new Manager { ManagerId = 4, FirstName = "Alice", LastName = "White", Position = "Sales Manager", Email = "alice.white@example.com", Phone = "444-444-4444" },
                new Manager { ManagerId = 5, FirstName = "Tom", LastName = "Green", Position = "Finance Manager", Email = "tom.green@example.com", Phone = "333-333-3333" }
            );

            // Seed data for Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Human Resources", Location = "Building A", ManagerId = 1 },
                new Department { DepartmentId = 2, DepartmentName = "IT", Location = "Building B", ManagerId = 2 },
                new Department { DepartmentId = 3, DepartmentName = "Marketing", Location = "Building C", ManagerId = 3 },
                new Department { DepartmentId = 4, DepartmentName = "Sales", Location = "Building D", ManagerId = 4 },
                new Department { DepartmentId = 5, DepartmentName = "Finance", Location = "Building E", ManagerId = 5 }
            );

            // Seed data for Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, FirstName = "Michael", LastName = "Jordan", Position = "HR Specialist", Email = "michael.jordan@example.com", Phone = "111-111-1111", DepartmentId = 1, DateOfBirth = new DateTime(1985, 5, 15), Salary = 50000, HireDate = new DateTime(2020, 1, 10), Gender = "Male", Image = "michael_jordan.jpg" },
                new Employee { EmployeeId = 2, FirstName = "Serena", LastName = "Williams", Position = "IT Specialist", Email = "serena.williams@example.com", Phone = "222-222-2222", DepartmentId = 2, DateOfBirth = new DateTime(1990, 8, 25), Salary = 60000, HireDate = new DateTime(2021, 3, 15), Gender = "Female", Image = "serena_williams.jpg" },
                new Employee { EmployeeId = 3, FirstName = "LeBron", LastName = "James", Position = "Marketing Analyst", Email = "lebron.james@example.com", Phone = "333-333-3333", DepartmentId = 3, DateOfBirth = new DateTime(1988, 12, 30), Salary = 55000, HireDate = new DateTime(2019, 6, 22), Gender = "Male", Image = "lebron_james.jpg" },
                new Employee { EmployeeId = 4, FirstName = "Simone", LastName = "Biles", Position = "Sales Representative", Email = "simone.biles@example.com", Phone = "444-444-4444", DepartmentId = 4, DateOfBirth = new DateTime(1996, 3, 14), Salary = 48000, HireDate = new DateTime(2022, 2, 5), Gender = "Female", Image = "simone_biles.jpg" },
                new Employee { EmployeeId = 5, FirstName = "Lionel", LastName = "Messi", Position = "Finance Analyst", Email = "lionel.messi@example.com", Phone = "555-555-5555", DepartmentId = 5, DateOfBirth = new DateTime(1987, 6, 24), Salary = 70000, HireDate = new DateTime(2020, 8, 12), Gender = "Male", Image = "lionel_messi.jpg" }
            );

            // Seed data for Staff
            modelBuilder.Entity<Staff>().HasData(
                new Staff { StaffId = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", Password = "hashed_password1", Role = "HR", Image = "user-1.jpg" },
                new Staff { StaffId = 2, FirstName = "Bob", LastName = "Smith", Email = "bob.smith@example.com", Password = "hashed_password2", Role = "Manager", Image = "user-1.jpg" },
                new Staff { StaffId = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@example.com", Password = "hashed_password3", Role = "Admin", Image = "user-1.jpg" }
            );




        }
    }
    }
