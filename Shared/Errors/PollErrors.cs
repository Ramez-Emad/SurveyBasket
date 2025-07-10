using Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Errors;
public class PollErrors
{
    public static readonly Error PollNotFound =
      new("Poll.NotFound", "No poll was found with the given ID");
}
