using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions;
public sealed class PollNotFoundException(int id) : NotFoundException($"Poll with id = {id} is not found")
{
}
