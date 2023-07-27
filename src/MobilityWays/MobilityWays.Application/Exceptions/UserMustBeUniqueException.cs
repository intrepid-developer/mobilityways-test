using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilityWays.Application.Exceptions;
public class UserMustBeUniqueException : Exception
{
    public UserMustBeUniqueException(string errorMessage): base(errorMessage)
    {
        
    }
}
