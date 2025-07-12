using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public sealed class Question : AuditableEntity
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public int PollId { get; set; }
    public Poll Poll { get; set; } = null!;

    public ICollection<Answer> Answers { get; set; } = [];
}
