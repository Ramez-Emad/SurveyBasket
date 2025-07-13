using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class VoteConfiguration : IEntityTypeConfiguration<Vote>
{

    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(v => new { v.UserId, v.PollId }).IsUnique();
    }
}
