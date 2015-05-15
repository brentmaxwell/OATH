using System;

namespace OATH
{
    public class TotpGenerator : HotpGenerator
    {
        /// <summary>
        /// The default validity period for an OTP token.
        /// </summary>
        private static readonly TimeSpan DefaultValidityPeriod = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Initializes a new instance of the TimeBasedOtpGenerator class. This is used when the client and server do not share a counter value but the clocks between the two are synchronized within reasonable margins of each other.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="otpLength">The number of digits in the OTP to generate.</param>
        public TotpGenerator(Key secretKey, int otpLength)
            : base(secretKey, otpLength)
        {
        }

        /// <summary>
        /// Generates the OTP for the given <paramref name="time"/> parameter. The client and server compute this independently and come up
        ///     with the same result, provided they use the same shared key.
        /// </summary>
        /// <param name="time">The date and time for which to generate an OTP.</param>
        /// <returns>The OTP for the given secret key and DateTime.</returns>
        public string GenerateOtp(DateTime time)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var span = time.ToUniversalTime() - unixEpoch;
            var steps = (int)(span.TotalSeconds / 30);

            return GenerateOtp(steps);
        }

        /// <summary>
        /// Validates a given OTP. A default validity period of 60 seconds is used.
        /// </summary>
        /// <param name="providedOtp">The OTP provided by the user attempting authentication.</param>
        /// <param name="currentTime">The time at which the given OTP should be valid.</param>
        /// <returns>True if the provided OTP is valid, otherwise false.</returns>
        public bool ValidateOtp(string providedOtp, DateTime currentTime)
        {
            return this.ValidateOtp(providedOtp, currentTime, DefaultValidityPeriod);
        }

        /// <summary>
        /// Validates a given OTP using the provided validity period.
        /// </summary>
        /// <param name="providedOtp">The OTP provided by the user attempting authentication.</param>
        /// <param name="currentTime">The time at which the given OTP should be valid.</param>
        /// <param name="validityPeriod">The interval of time in which the provided OTP should be allowed. For example, a validity period of 60 seconds indicates the code generated at 6:41 PM should still be valid at 6:40 PM and 6:42 PM. This helps accomodate for inaccurately-set clocks.</param>
        /// <returns>True if the provided OTP is valid, otherwise false.</returns>
        public bool ValidateOtp(string providedOtp, DateTime currentTime, TimeSpan validityPeriod)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var span = currentTime.ToUniversalTime() - unixEpoch;
            var steps = (int)(span.TotalSeconds / 30);
            var interval = (int)(Math.Abs(validityPeriod.TotalSeconds) / 30);
            var minSteps = steps - interval;
            var maxSteps = steps + interval;

            for (var step = minSteps; step <= maxSteps; step++)
            {
                if (GenerateOtp(step).Equals(providedOtp, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
