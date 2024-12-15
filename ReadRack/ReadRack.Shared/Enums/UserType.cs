using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadRack.Shared.Enums
{
    public enum UserType
    {
        [Description("Admin")]
        Admin,

        [Description("User")]
        User
    }
}
