using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Enums
{
    public enum ResponseType
    {
        Success,
        ValidationError,
        Unauthorized,
        Conflict,
        NotFound,
        ServerError
    }
}
