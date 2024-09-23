using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;

namespace Teram.HR.Module.Recruitment.Configs
{
    public class JobApplicantConfiguration : IEntityTypeConfiguration<JobApplicant>
    {
        public void Configure(EntityTypeBuilder<JobApplicant> builder)
        {
            builder.HasIndex(x => x.NationalCode).IsUnique();            
        }
    }
}
