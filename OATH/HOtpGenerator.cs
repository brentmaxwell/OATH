using System;
using System.Security.Cryptography;

namespace OATH
{
    /// <summary>
    /// OATH-HOTP Generator (RFC 4226)
    /// </summary>
    public class HotpGenerator : IOathGenerator
    {
        /// <summary>
        /// Used to calculate the checksum.
        /// </summary>
        private static readonly int[] _digits =
        {
            1, // 0
            10, // 1
            100, // 2
            1000, // 3
            10000, // 4
            100000, // 5
            1000000, // 6
            10000000, // 7
            100000000 // 8
        };

        /// <summary>
        /// Number of digits to return in the OTP.
        /// </summary>
        private readonly int _otpLength;

        /// <summary>
        /// System.Security.Cryptography.HMAC algorithm used.
        /// </summary>
        private readonly HMAC _hmac;

        /// <summary>
        /// Initializes a new instance of the CounterBasedOtpGenerator class. This is used when the client and server share a counter value.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="otpLength">The number of digits in the OTP to generate.</param>
        public HotpGenerator(Key secretKey, int otpLength)
        {
            this._otpLength = otpLength;
            this._hmac = new HMACSHA1(secretKey.Binary);
        }

        /// <summary>
        /// Generates the OTP for the given <paramref name="counter"/> value. The client and server compute this independently and come up
        ///     with the same result, provided they use the same shared key.
        /// </summary>
        /// <param name="counter">The counter value to use.</param>
        /// <returns>The OTP for the given counter value.</returns>
        public string GenerateOtp(int counter)
        {
            var text = BitConverter.GetBytes(counter);

            if (BitConverter.IsLittleEndian)
            {
                Array.Resize(ref text, 8); // text = { 04, 03, 02, 01, 00, 00, 00, 00 }
                Array.Reverse(text); // text = { 00, 00, 00, 00, 01, 02, 03, 04 }
            }
            else
            {
                Array.Reverse(text); // text = { 04, 03, 02, 01 }
                Array.Resize(ref text, 8); // text = { 04, 03, 02, 01, 00, 00, 00, 00 }
                Array.Reverse(text); // text = { 00, 00, 00, 00, 01, 02, 03, 04 }
            }

            var hash = this._hmac.ComputeHash(text);

            int offset = hash[hash.Length - 1] & 0xF;

            int binary = ((hash[offset] & 0x7F) << 24) |
                         ((hash[offset + 1] & 0xFF) << 16) |
                         ((hash[offset + 2] & 0xFF) << 8) |
                         (hash[offset + 3] & 0xFF);

            var otp = binary%_digits[this._otpLength];

            var result = otp.ToString("D" + this._otpLength);

            return result;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the OATH.HotpGenerator class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the OATH.HotpGenrator class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hmac.Dispose();
            }
        }
    }
}
