using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oath;

namespace OathTests
{
    /// <remarks>test vectors from RFC 6238 (https://www.ietf.org/rfc/rfc6238.txt)</remarks>
    [TestClass]
    public class TotpTests
    {
        public static string Key = "12345678901234567890";

        [TestMethod]
        public void TestGenerateTotp()
        {
            var key = new Key(Encoding.ASCII.GetBytes(Key));
            using (var oath = new TotpGenerator(key, 8, TimeSpan.FromSeconds(30)))
            {
                Assert.AreEqual("94287082", oath.GenerateOtp(new DateTime(1970, 01, 01, 00, 00, 59, DateTimeKind.Utc)));
                Assert.AreEqual("07081804", oath.GenerateOtp(new DateTime(2005, 03, 18, 01, 58, 29, DateTimeKind.Utc)));
                Assert.AreEqual("14050471", oath.GenerateOtp(new DateTime(2005, 03, 18, 01, 58, 31, DateTimeKind.Utc)));
                Assert.AreEqual("89005924", oath.GenerateOtp(new DateTime(2009, 02, 13, 23, 31, 30, DateTimeKind.Utc)));
                Assert.AreEqual("69279037", oath.GenerateOtp(new DateTime(2033, 05, 18, 03, 33, 20, DateTimeKind.Utc)));
                Assert.AreEqual("65353130", oath.GenerateOtp(new DateTime(2603, 10, 11, 11, 33, 20, DateTimeKind.Utc)));

            }
        }

        [TestMethod]
        public void TestValidateTotp()
        {
            var key = new Key(Encoding.ASCII.GetBytes(Key));
            using (var oath = new TotpGenerator(key, 8, TimeSpan.FromSeconds(30)))
            {
                Assert.IsTrue(oath.ValidateOtp("94287082", new DateTime(1970, 01, 01, 00, 00, 59, DateTimeKind.Utc)));
                Assert.IsTrue(oath.ValidateOtp("07081804", new DateTime(2005, 03, 18, 01, 58, 29, DateTimeKind.Utc)));
                Assert.IsTrue(oath.ValidateOtp("14050471", new DateTime(2005, 03, 18, 01, 58, 31, DateTimeKind.Utc)));
                Assert.IsTrue(oath.ValidateOtp("89005924", new DateTime(2009, 02, 13, 23, 31, 30, DateTimeKind.Utc)));
                Assert.IsTrue(oath.ValidateOtp("69279037", new DateTime(2033, 05, 18, 03, 33, 20, DateTimeKind.Utc)));
                Assert.IsTrue(oath.ValidateOtp("65353130", new DateTime(2603, 10, 11, 11, 33, 20, DateTimeKind.Utc)));
            }
        }
    }
}
