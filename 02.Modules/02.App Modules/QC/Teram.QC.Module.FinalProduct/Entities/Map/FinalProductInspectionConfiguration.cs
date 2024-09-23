using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Teram.QC.Module.FinalProduct.Entities.Map
{
    public class FinalProductInspectionConfiguration : IEntityTypeConfiguration<FinalProductInspection>
    {
        public void Configure(EntityTypeBuilder<FinalProductInspection> builder)
        {
            builder.HasIndex(x => x.Number).IsUnique();
        }
    }
}
