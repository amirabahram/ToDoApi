using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Application.Tests.Implementations.Extensions
{
    public class IdentityResultMock : IdentityResult
    {
        public IdentityResultMock(bool succeeded = false)
        {
            this.Succeeded = succeeded;
        }
    }
}
