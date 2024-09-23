using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Configs;
using Teram.HR.Module.Recruitment.Entities.BaseInfo;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.Web.Core.Configurations;

namespace Teram.HR.Module.Recruitment.Entities.DbContext
{
    public class HRContext : TeramBaseContext
    {

        private readonly string connectionString;
        public DbSet<JobApplicant> JobApplicants { get; set; }
        public DbSet<JobApplicantFile> JobApplicantFiles { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public DbSet<PersonQuestionnaireQuestion> PersonQuestionnaireQuestions { get; set; }
        public DbSet<PersonnelQuestionnaire> personnelQuestionnaires { get; set; }
        public DbSet<BaseInformation> BaseInformation { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<TrainingRecord> TrainingRecords { get; set; }
        public HRContext()
        {
            connectionString = GlobalConfiguration.Configurations.ModuleDevelopeConnectionString;
        }

        public HRContext(IConfiguration configuration)
        {
            configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            connectionString = configuration.GetConnectionString("TeramConnectionString");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("_HRMigrationHistory"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JobApplicantConfiguration());            
            base.OnModelCreating(modelBuilder);
        }

    }
}
