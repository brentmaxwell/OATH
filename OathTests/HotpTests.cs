using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oath;

namespace OathTests
{
    /// <remarks>test vectors from RFC 4226 (https://www.ietf.org/rfc/rfc4226.txt)</remarks>
    [TestClass]
    public class HotpTests
    {
        public static string Key = "12345678901234567890";

        [TestMethod]
        public void TestGenerateHotp()
        {
            var key = new Key(Encoding.ASCII.GetBytes(Key));
            using (var oath = new HotpGenerator(key, 6))
            {
                Assert.AreEqual(oath.GenerateOtp(0), "755224");
                Assert.AreEqual(oath.GenerateOtp(1), "287082");
                Assert.AreEqual(oath.GenerateOtp(2), "359152");
                Assert.AreEqual(oath.GenerateOtp(3), "969429");
                Assert.AreEqual(oath.GenerateOtp(4), "338314");
                Assert.AreEqual(oath.GenerateOtp(5), "254676");
                Assert.AreEqual(oath.GenerateOtp(6), "287922");
                Assert.AreEqual(oath.GenerateOtp(7), "162583");
                Assert.AreEqual(oath.GenerateOtp(8), "399871");
                Assert.AreEqual(oath.GenerateOtp(9), "520489");
            }
        }

        [TestMethod]
        public void TestValidateHotp()
        {
            var key = new Key(Encoding.ASCII.GetBytes(Key));
            using (var oath = new HotpGenerator(key, 6))
            {
                Assert.IsTrue(oath.ValidateOtp("755224", 0));
                Assert.IsTrue(oath.ValidateOtp("287082", 1));
                Assert.IsTrue(oath.ValidateOtp("359152", 2));
                Assert.IsTrue(oath.ValidateOtp("969429", 3));
                Assert.IsTrue(oath.ValidateOtp("338314", 4));
                Assert.IsTrue(oath.ValidateOtp("254676", 5));
                Assert.IsTrue(oath.ValidateOtp("287922", 6));
                Assert.IsTrue(oath.ValidateOtp("162583", 7));
                Assert.IsTrue(oath.ValidateOtp("399871", 8));
                Assert.IsTrue(oath.ValidateOtp("520489", 9));
            }
        }
    }
}
