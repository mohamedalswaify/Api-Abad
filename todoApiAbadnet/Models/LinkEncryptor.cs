using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace todoApiAbadnet.Models
{
    public class LinkEncryptor
    {
        private readonly byte[] _key;

        public LinkEncryptor(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");
            }

            _key = Encoding.UTF8.GetBytes(secretKey);
        }

        public string EncryptLink(string link)
        {
            using (var hmac = new HMACSHA256(_key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(link));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyLink(string link, string hash)
        {
            using (var hmac = new HMACSHA256(_key))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(link));
                var expectedHash = Convert.FromBase64String(hash);

                return computedHash.SequenceEqual(expectedHash);
            }
        }
    }
}
