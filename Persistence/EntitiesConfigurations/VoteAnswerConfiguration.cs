using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(a => new { a.QuestionId, a.VoteId }).IsUnique();
    }
}
