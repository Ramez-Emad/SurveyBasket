using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public sealed class Vote
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;

    public int PollId { get; set; }
    public Poll Poll { get; set; } = default!;

    public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;

    public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];

}
