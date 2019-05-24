using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Services
{
    public interface IPasswordSupplier
    {
        SecureString GetPassword { get; }
        SecureString GetCurrentPassword { get; }
        bool ConfirmPassword();
        bool IsEmpty();
    }
}
