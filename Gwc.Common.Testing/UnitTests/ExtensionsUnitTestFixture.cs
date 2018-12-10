using NUnit.Framework;
using Gwc.Common.Utilities.Extensions;
using System;
using System.Linq;

namespace Gwc.Testing.UnitTesting
{
    public class ExtensionsUnitTestFixture
    {
        [Test]
        public void DecryptAimsDev()
        {
            //var connection = _context.GetDbSet<Passwords>().FirstOrDefault(x => x.PWKey == "AIMSUCDEV");
            //var pw = connection.Password.Decrypt();
            //Assert.That("p9bjnp8n7o", Is.EqualTo(pw));
        }
        [Test]
        public void EncryptDecrypt()
        {
            var pw = "[yo)?ut=ips~";
            var encPw = pw.Encrypt();
            var dcPw = encPw.Decrypt();
            Assert.That(pw == dcPw);
        }
    }
}