using System;

namespace Oath
{
    public class Key
    {
        private readonly byte[] _keyData;

        /// <summary>
        ///     Initializes a new instance of the Key class and generates a random 10-byte key.
        /// </summary>
        public Key()
            : this(10, (new Random()).Next())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the Key class and generates a random key with the specified seed.
        /// </summary>
        /// <param name="keyLength">Length in bytes of the generated key.</param>
        /// <param name="seed">A seed to use for the random key generation.</param>
        public Key(int keyLength, int seed)
        {
            _keyData = new byte[keyLength];
            var gen = new Random(seed);
            gen.NextBytes(_keyData);
        }

        /// <summary>
        ///     Initializes a new instance of the Key class.
        /// </summary>
        /// <param name="data">The key to initialize.</param>
        public Key(byte[] data)
        {
            _keyData = data;
        }

        /// <summary>
        ///     Initializes a new instance of the Key class.
        /// </summary>
        /// <param name="base32Key">The key to initialize.</param>
        /// <exception cref="ArgumentException">base32key is not a valid base32-encoded string.</exception>
        public Key(string base32Key)
        {
            _keyData = base32Key.ToBinary();
        }

        /// <summary>
        ///     Gets the key represented as a byte array.
        /// </summary>
        public virtual byte[] ToBinary()
        {
            return _keyData;
        }

        /// <summary>
        ///     Gets the key represented as base32-encoded string.
        /// </summary>
        public virtual string ToBase32()
        {
            return _keyData.ToBase32();
        }
    }
}
