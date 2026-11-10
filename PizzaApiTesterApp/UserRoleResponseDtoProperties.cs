using System;
using System.Collections.Generic;
using System.Text;

namespace ApiService;

public partial class UserRoleResponseDto
{
    public string DisplayName => $"[{Id}] {Name}";
}
