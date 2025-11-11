using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMS.Domain;

namespace PMS.Domains;

public partial class PmsDbContext : DbContext
{
    public PmsDbContext()
    {
    }

    public PmsDbContext(DbContextOptions<PmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DepartmentMaster> DepartmentMasters { get; set; }

    public virtual DbSet<DesignationMaster> DesignationMasters { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeTaskDetail> EmployeeTaskDetails { get; set; }

    public virtual DbSet<EmployeeTimeSheet> EmployeeTimeSheets { get; set; }

    public virtual DbSet<EmployeeTimeSheetTaskDetail> EmployeeTimeSheetTaskDetails { get; set; }

    public virtual DbSet<EmployeeTodo> EmployeeTodos { get; set; }

    public virtual DbSet<MenuMaster> MenuMasters { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectDocument> ProjectDocuments { get; set; }

    public virtual DbSet<ProjectEmployee> ProjectEmployees { get; set; }

    public virtual DbSet<ProjectTask> ProjectTasks { get; set; }

    public virtual DbSet<ProjectTaskDocument> ProjectTaskDocuments { get; set; }

    public virtual DbSet<ProjectTaskEmployeeHistory> ProjectTaskEmployeeHistories { get; set; }

    public virtual DbSet<ProjectTaskPriorityHistory> ProjectTaskPriorityHistories { get; set; }

    public virtual DbSet<ProjectTaskStatusHistory> ProjectTaskStatusHistories { get; set; }

    public virtual DbSet<ProjectTaskTypeHistory> ProjectTaskTypeHistories { get; set; }

    public virtual DbSet<RoleMaster> RoleMasters { get; set; }

    public virtual DbSet<RoleMenuMapping> RoleMenuMappings { get; set; }

    public virtual DbSet<TaskDiscussionBoard> TaskDiscussionBoards { get; set; }

    public virtual DbSet<TaskStatusMaster> TaskStatusMasters { get; set; }

    public virtual DbSet<UserManagement> UserManagements { get; set; }

    public virtual DbSet<Sprint> Sprints { get; set; }

    public virtual DbSet<StatusHelper> StatusHelpers { get; set; }

    public virtual DbSet<ProjectDocumentsRequest> ProjectDocumentsRequests { get; set; }

    public virtual DbSet<VacationDetail> VacationDetails { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<NotificationDetail> NotificationDetails { get; set; }

    public virtual DbSet<EmployeeLeave> EmployeeLeaves { get; set; }

    public virtual DbSet<ProjectMessage> ProjectMessages { get; set; }

    public virtual DbSet<ReportMaster> ReportMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=122.176.55.107,1433;Database=ProjectManagementSystem;User Id=sa;Password=vi@pra91;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DepartmentMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC076005FAD7");

            entity.ToTable("DepartmentMaster");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DesignationMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Designat__3214EC07080F8F9E");

            entity.ToTable("DesignationMaster");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07588FBEC4");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DateOfJoining).HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeeTaskDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC074A47F49C");

            entity.ToTable("EmployeeTaskDetail");

            entity.Property(e => e.Comment)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<EmployeeTimeSheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC073A0D29AC");

            entity.ToTable("EmployeeTimeSheet");

            entity.Property(e => e.ApprovedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.TimeSheetName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.TimeSheetStatus)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeTimeSheetTaskDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0744CFBF20");

            entity.ToTable("EmployeeTimeSheetTaskDetail");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.WorkingHour)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeTodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0786C2E7D3");

            entity.ToTable("EmployeeTodo");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.TaskDetail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TaskStatus)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MenuMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MenuMast__3214EC07D02FA869");

            entity.ToTable("MenuMaster");

            entity.Property(e => e.ActionName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ControllerName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.MenuName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.SubMenuName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC079DAC3248");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ProjectEndDate).HasColumnType("datetime");
            entity.Property(e => e.ProjectStartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectD__3214EC07E30FB105");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectE__3214EC07E7B7EDBE");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectT__3214EC07B9EC7EE4");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModuleName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.TaskCode)
                .HasMaxLength(3000)
                .IsUnicode(false);
            entity.Property(e => e.TaskName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TaskPriority)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.TaskStatus)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.TaskType)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectTaskDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectT__3214EC072B76F17F");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentDetail)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.DocumentName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectTaskEmployeeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectT__3214EC074FBA0A63");

            entity.ToTable("ProjectTaskEmployeeHistory");

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectTaskPriorityHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectT__3214EC07CAF23E4A");

            entity.ToTable("ProjectTaskPriorityHistory");

            entity.Property(e => e.TaskPriority)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectTaskStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ProjectTaskStatusHistory");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TaskStatus)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProjectTaskTypeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectT__3214EC073E30A048");

            entity.ToTable("ProjectTaskTypeHistory");

            entity.Property(e => e.TaskType)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<RoleMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleMast__3214EC077D1E9B7D");

            entity.ToTable("RoleMaster");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<RoleMenuMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleMenu__3214EC07D7B03414");

            entity.ToTable("RoleMenuMapping");
        });

        modelBuilder.Entity<TaskDiscussionBoard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("TaskDiscussionBoard");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TaskStatusMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaskStat__3214EC0705737706");

            entity.ToTable("TaskStatusMaster");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserManagement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMana__3214EC071F0CAB15");

            entity.ToTable("UserManagement");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsLocked).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
