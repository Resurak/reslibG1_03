using reslibG1_03.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace reslibG1_03.Cryptography
{
    public class BaseCrypt : StreamHandler
    {
        internal short _AES_KeySize = 256;
        internal short _SaltSize = 32;
        internal short _IVSize = 32;

        internal int _BufferSize = 1024 * 64;
        internal int _RfcIterations = 50000;

        internal string _Extension = ".crypt";

        public short AES_KeySize { get => _AES_KeySize; set => _AES_KeySize = value; } 
        public short SaltSize { get => _SaltSize; set => _SaltSize = value; }
        public short IVSize { get => _IVSize; set => _IVSize = value; }

        public int BufferSize { get => _BufferSize; set => _BufferSize = value; }
        public int RfcIterations { get => _RfcIterations; set => _RfcIterations = value; }

        public string Extension { get => _Extension; set => _Extension = value; }



        public BaseCrypt() { }
        public BaseCrypt(int bufferSize) : this(bufferSize, 0, 0, 0) { }
        public BaseCrypt(short saltSize, short ivSize) : this(0, 0, saltSize, ivSize) { }
        public BaseCrypt(int bufferSize, int rfcIterations) : this(bufferSize, rfcIterations, 0, 0) { }
        public BaseCrypt(int bufferSize, int rfcIterations, short saltSize, short IVSize) => Initialize(bufferSize, rfcIterations, saltSize, IVSize);



        private protected void Initialize(int bufSize, int rfc, short saltSize, short ivSize) 
        {
            if (bufSize > 0)
                BufferSize = bufSize;

            if (rfc > 0)
                RfcIterations = rfc;

            if (saltSize > 0 && saltSize <= 1024)
                SaltSize = saltSize;

            if (ivSize > 0 && ivSize <= 1024)
                IVSize = ivSize;
        }



        internal void Encrypt(Stream input, Stream output, byte[] pass, byte[] iv)
        {
            using (var AES_Key = new RijndaelManaged { BlockSize = AES_KeySize, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            using (var encryptor = AES_Key.CreateEncryptor(pass, iv))
            using (var cryptoS = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
            {
                WriteInToOut(input, cryptoS as Stream);
            }
        }


        internal void Decrypt(Stream input, Stream output, byte[] pass, byte[] iv)
        {
            using (var AES_Key = new RijndaelManaged { BlockSize = AES_KeySize, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            using (var decryptor = AES_Key.CreateDecryptor(pass, iv))
            using (var cryptoS = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
            {
                WriteInToOut(cryptoS as Stream, output);
            }
        }
    }
}
