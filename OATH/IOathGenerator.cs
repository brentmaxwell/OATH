using System;

namespace Oath
{
    /// <summary>
    /// Interface for the OATH Generator Classes
    /// </summary>
    /// <remarks></remarks>
    public interface IOathGenerator : IDisposable
    {
        string GenerateOtp(int counter);
        bool ValidateOtp(string providedOtp, int counter);
    }
}
