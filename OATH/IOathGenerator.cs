using System;

namespace OATH
{
    /// <summary>
    /// Interface for the OATH Generator Classes
    /// </summary>
    /// <remarks></remarks>
    public interface IOathGenerator : IDisposable
    {
        string GenerateOtp(int counter);
    }
}
