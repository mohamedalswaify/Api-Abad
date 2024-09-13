using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using todoApiAbadnet.Models;



namespace todoApiAbadnet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CoursesIsonline>().Property(b => b.IsHide).HasDefaultValue(false);
            builder.Entity<CoursesIsonline>().Property(b => b.IsDelete).HasDefaultValue(false);
            //builder.Entity<CoursesScheduless>().Property(b => b.CalculatedEndDate).I
            var dateOnlyConverter = new ValueConverter<DateOnly?, DateTime?>(
             v => v.HasValue ? new DateTime(v.Value.Year, v.Value.Month, v.Value.Day) : (DateTime?)null,
             v => v.HasValue ? new DateOnly(v.Value.Year, v.Value.Month, v.Value.Day) : (DateOnly?)null);
            builder.Entity<SelectData>().HasData(
            new SelectData
            {
                Id = 1,
                VlaueData = 1,
                MessageData = "1",
            });
            var TimeOnlyConverter = new ValueConverter<TimeOnly?, TimeSpan?>(
            v => v.HasValue ? v.Value.ToTimeSpan() : (TimeSpan?)null,
            v => v.HasValue ? TimeOnly.FromTimeSpan(v.Value) : (TimeOnly?)null);
            builder.Entity<CoursesReserved>().HasKey(a => new { a.StudentId, a.CoursesSchedulessId });
            builder.Entity<CoursesScheduless>().Property(e => e.CloseDay).HasConversion(dateOnlyConverter);
            builder.Entity<CoursesScheduless>().Property(e => e.StartDate).HasConversion(dateOnlyConverter);
            builder.Entity<CoursesScheduless>().Property(e => e.EndDate).HasConversion(dateOnlyConverter);
            builder.Entity<CoursesScheduless>().Property(e => e.StartTime).HasConversion(TimeOnlyConverter);
            builder.Entity<CoursesScheduless>().Property(e => e.EndTime).HasConversion(TimeOnlyConverter);
            builder.Entity<Student>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });

            builder.Entity<CoursesScheduless>()
                .HasOne(p => p.CoursesType)
                .WithMany()
                .HasForeignKey(p => p.CoursesTypeCode)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<CoursesScheduless>()
    .HasOne(p => p.CoursesType)
    .WithMany()
    .HasForeignKey(p => p.CoursesTypeCode)
    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CoursesScheduless>()
                .HasOne(p => p.CoursesType)
                .WithMany()
                .HasForeignKey(p => p.CoursesTypeCode)
                .OnDelete(DeleteBehavior.Restrict); // أو .OnDelete(DeleteBehavior.NoAction)
            builder.Entity<RequiredCourse>()
                .HasOne(p => p.CoursesType)
                .WithMany()
                .HasForeignKey(p => p.CoursesTypeCode)
                .OnDelete(DeleteBehavior.Restrict); // أو .OnDelete(DeleteBehavior.NoAction)



            builder.Entity<CoursesType>().HasData(
                new CoursesType { Code = 1, ArabicName = "أمن المعلومات", EnglishName = "Information Security" },
                new CoursesType { Code = 2, ArabicName = "إدارة أعمال", EnglishName = "Business Management" },
                new CoursesType { Code = 3, ArabicName = "برمجـــــــة", EnglishName = "Programming" },
                new CoursesType { Code = 4, ArabicName = "شبكـــــات", EnglishName = "Networking" },
                new CoursesType { Code = 5, ArabicName = "إدارة خوادم وسـيرفرات", EnglishName = "Server Management" }
            );

            builder.Entity<Branch>().HasData(
                  new Branch { Id = 1, ArabicName = "الرياض", EnglishName = "Riyadh" }
            );

            builder.Entity<TypePaid>().HasData(
                new TypePaid { Id = 1, ArabicName = "كاش", EnglishName = "Cash", IsHide = false, IsDelete = false, IsLocked = false, UserCode = "111", CreatedDate = DateTime.Now },
                new TypePaid { Id = 2, ArabicName = "شبكه", EnglishName = "network", IsHide = false, IsDelete = false, IsLocked = false, UserCode = "111", CreatedDate = DateTime.Now },
                new TypePaid { Id = 3, ArabicName = "تمارا", EnglishName = "Tamara", IsHide = false, IsDelete = false, IsLocked = false, UserCode = "111", CreatedDate = DateTime.Now },
                new TypePaid { Id = 4, ArabicName = "تابي", EnglishName = "tabby", IsHide = false, IsDelete = false, IsLocked = false, UserCode = "111", CreatedDate = DateTime.Now }

            );
            builder.Entity<CoursesStatus>().HasData(
                new CoursesStatus { Id = 1, ArabicName = "مفتوحه", EnglishName = "opened", IsHide = false, IsDelete = false, UserCode = "111", CreatedDate = DateTime.Parse("2023-08-27 00:52:52"), LastUpdateUserCode = "111", LastUpdateDate = DateTime.Parse("2023-08-27 00:54:50") },
                new CoursesStatus { Id = 2, ArabicName = "مغلقه", EnglishName = "clossed", IsHide = false, IsDelete = false, UserCode = "111", CreatedDate = DateTime.Parse("2023-08-27 00:53:09"), LastUpdateUserCode = "111", LastUpdateDate = DateTime.Parse("2023-08-27 00:55:18") },
                new CoursesStatus { Id = 3, ArabicName = "منتهيه", EnglishName = "ended", IsHide = false, IsDelete = false, UserCode = "111", CreatedDate = DateTime.Now, LastUpdateUserCode = "111", LastUpdateDate = DateTime.Now },
                new CoursesStatus { Id = 4, ArabicName = "ملغاه", EnglishName = "cancelled", IsHide = false, IsDelete = false, UserCode = "111", CreatedDate = DateTime.Now, LastUpdateUserCode = "111", LastUpdateDate = DateTime.Now }

            );
            builder.Entity<CoursesIsonline>().HasData(
                new CoursesIsonline { Id = 1, ArabicName = "حضوري", EnglishName = "offline", IsHide = false, IsDelete = false, UserCode = "111", CreatedDate = DateTime.Parse("2023-08-27 00:52:18"), LastUpdateUserCode = "111", LastUpdateDate = DateTime.Parse("2023-11-28 00:20:10") },
                new CoursesIsonline { Id = 2, ArabicName = "أونلاين", EnglishName = "online", IsHide = false, IsDelete = false, UserCode = "111", CreatedDate = DateTime.Parse("2023-08-27 00:52:34"), LastUpdateUserCode = null, LastUpdateDate = null }

            );
            builder.Entity<HomeScreen>().HasData(
                new HomeScreen { Id = 1, Title = "مرحبا بكم في آباد للتدريب", IsShowTitel = true, Description = "معهد شبكة آباد للتدريب من المعاهد الرائدة في تقديم الدورات التطويرية المتخصصة في تقنية المعلومات.", IsShowDes = true, Image = "banner.webp", IsButton = true, State = true }

            );

            builder.Entity<ContractStatus>().HasData(
               new ContractStatus { Code = 1, ArabicName = "مفعل", EnglishName = "Active", IsDelete = false, UserCode = 111, CreatedDate = DateTime.Parse("2023-08-27 00:52:18"), LastUpdateUserCode = null, LastUpdateDate = null },
               new ContractStatus { Code = 2, ArabicName = "غير مفعل", EnglishName = "Not Active", IsDelete = false, UserCode = 111, CreatedDate = DateTime.Parse("2023-08-27 00:52:34"), LastUpdateUserCode = null, LastUpdateDate = null }

           );


        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // other configuration settings...
        }



        public virtual DbSet<CoursesScheduless> coursesSchedulesses { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CompaniesData> CompaniesData { get; set; }
        public virtual DbSet<ContractDatum> ContractData { get; set; }
        public virtual DbSet<ContractStatus> ContractStatuses { get; set; }
        public virtual DbSet<CoursesData> CoursesData { get; set; }
        public virtual DbSet<CoursesIsonline> CoursesIsonlines { get; set; }
        public virtual DbSet<CoursesReserved> CoursesReserveds { get; set; }
        public virtual DbSet<CoursesScheduless> CoursesSchedulesses { get; set; }
        public virtual DbSet<CoursesStatus> CoursesStatuses { get; set; }
        public virtual DbSet<ViewsNow> ViewsNows { get; set; }
        public virtual DbSet<CoursesType> CoursesTypes { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Trainer> TrainerData { get; set; }
        public virtual DbSet<TypePaid> TypePaidData { get; set; }
        public virtual DbSet<MoveStudent> MoveStudent { get; set; }
        public virtual DbSet<SelectData> SelectDatas { get; set; }
        public virtual DbSet<ViewStudnt> ViewStudnts { get; set; }
        public virtual DbSet<RequiredCourse> RequiredCourse { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<PayTabsCallbackModel> PayTabsCallbackModel { get; set; }
        public virtual DbSet<paytabsPaymentCourse> PaytabsPaymentCourses { get; set; }

        public virtual DbSet<CompanyRequest> CompanyRequests { get; set; }
        public virtual DbSet<SessionsRecord> SessionsRecords { get; set; }
        public virtual DbSet<SessionsWeek> SessionsWeeks { get; set; }
        public virtual DbSet<SessionsDay> SessionsDays { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<HomeScreen> HomeScreens { get; set; }
        public virtual DbSet<EmployeePremotion> EmployeePremotion { get; set; }
        public virtual DbSet<CuntaryCode> CuntaryCodes { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<courseBasket> CourseBaskets { get; set; }
        public virtual DbSet<TamaraPayment> TamaraPayments { get; set; }
        public virtual DbSet<TamaraPaymentCourse> TamaraPaymentCourses { get; set; }
        public virtual DbSet<AuthorizeOrderModel> AuthorizeOrderModels { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<SerivesModel> SerivesModels { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<SettingsHome> SettingsHomes { get; set; }
        public virtual DbSet<PrivacyModel> PrivacyModels { get; set; }
        public virtual DbSet<CampsModel> CampsModels { get; set; }
        public virtual DbSet<VisitorCount> VisitorCounts { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<TabbyPayment> TabbyPayments { get; set; }
        public virtual DbSet<TabbyPaymentCourse> TabbyPaymentCourses { get; set; }
        public virtual DbSet<DiscountCodes> DiscountCodes { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<ArticleViewCount>  ArticleViewCounts { get; set; }




    }
}
