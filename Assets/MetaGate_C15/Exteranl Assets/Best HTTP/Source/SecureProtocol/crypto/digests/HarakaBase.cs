#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
    public abstract class HarakaBase
        : IDigest
    {
        internal static readonly int DIGEST_SIZE = 32;

        // Haraka round constants
        internal static readonly byte[][] RC = new byte[][]
        {
            new byte[]{ 0x9D, 0x7B, 0x81, 0x75, 0xF0, 0xFE, 0xC5, 0xB2, 0x0A, 0xC0, 0x20, 0xE6, 0x4C, 0x70, 0x84, 0x06 },
            new byte[]{ 0x17, 0xF7, 0x08, 0x2F, 0xA4, 0x6B, 0x0F, 0x64, 0x6B, 0xA0, 0xF3, 0x88, 0xE1, 0xB4, 0x66, 0x8B },
            new byte[]{ 0x14, 0x91, 0x02, 0x9F, 0x60, 0x9D, 0x02, 0xCF, 0x98, 0x84, 0xF2, 0x53, 0x2D, 0xDE, 0x02, 0x34 },
            new byte[]{ 0x79, 0x4F, 0x5B, 0xFD, 0xAF, 0xBC, 0xF3, 0xBB, 0x08, 0x4F, 0x7B, 0x2E, 0xE6, 0xEA, 0xD6, 0x0E },
            new byte[]{ 0x44, 0x70, 0x39, 0xBE, 0x1C, 0xCD, 0xEE, 0x79, 0x8B, 0x44, 0x72, 0x48, 0xCB, 0xB0, 0xCF, 0xCB },
            new byte[]{ 0x7B, 0x05, 0x8A, 0x2B, 0xED, 0x35, 0x53, 0x8D, 0xB7, 0x32, 0x90, 0x6E, 0xEE, 0xCD, 0xEA, 0x7E },
            new byte[]{ 0x1B, 0xEF, 0x4F, 0xDA, 0x61, 0x27, 0x41, 0xE2, 0xD0, 0x7C, 0x2E, 0x5E, 0x43, 0x8F, 0xC2, 0x67 },
            new byte[]{ 0x3B, 0x0B, 0xC7, 0x1F, 0xE2, 0xFD, 0x5F, 0x67, 0x07, 0xCC, 0xCA, 0xAF, 0xB0, 0xD9, 0x24, 0x29 },
            new byte[]{ 0xEE, 0x65, 0xD4, 0xB9, 0xCA, 0x8F, 0xDB, 0xEC, 0xE9, 0x7F, 0x86, 0xE6, 0xF1, 0x63, 0x4D, 0xAB },
            new byte[]{ 0x33, 0x7E, 0x03, 0xAD, 0x4F, 0x40, 0x2A, 0x5B, 0x64, 0xCD, 0xB7, 0xD4, 0x84, 0xBF, 0x30, 0x1C },
            new byte[]{ 0x00, 0x98, 0xF6, 0x8D, 0x2E, 0x8B, 0x02, 0x69, 0xBF, 0x23, 0x17, 0x94, 0xB9, 0x0B, 0xCC, 0xB2 },
            new byte[]{ 0x8A, 0x2D, 0x9D, 0x5C, 0xC8, 0x9E, 0xAA, 0x4A, 0x72, 0x55, 0x6F, 0xDE, 0xA6, 0x78, 0x04, 0xFA },
            new byte[]{ 0xD4, 0x9F, 0x12, 0x29, 0x2E, 0x4F, 0xFA, 0x0E, 0x12, 0x2A, 0x77, 0x6B, 0x2B, 0x9F, 0xB4, 0xDF },
            new byte[]{ 0xEE, 0x12, 0x6A, 0xBB, 0xAE, 0x11, 0xD6, 0x32, 0x36, 0xA2, 0x49, 0xF4, 0x44, 0x03, 0xA1, 0x1E },
            new byte[]{ 0xA6, 0xEC, 0xA8, 0x9C, 0xC9, 0x00, 0x96, 0x5F, 0x84, 0x00, 0x05, 0x4B, 0x88, 0x49, 0x04, 0xAF },
            new byte[]{ 0xEC, 0x93, 0xE5, 0x27, 0xE3, 0xC7, 0xA2, 0x78, 0x4F, 0x9C, 0x19, 0x9D, 0xD8, 0x5E, 0x02, 0x21 },
            new byte[]{ 0x73, 0x01, 0xD4, 0x82, 0xCD, 0x2E, 0x28, 0xB9, 0xB7, 0xC9, 0x59, 0xA7, 0xF8, 0xAA, 0x3A, 0xBF },
            new byte[]{ 0x6B, 0x7D, 0x30, 0x10, 0xD9, 0xEF, 0xF2, 0x37, 0x17, 0xB0, 0x86, 0x61, 0x0D, 0x70, 0x60, 0x62 },
            new byte[]{ 0xC6, 0x9A, 0xFC, 0xF6, 0x53, 0x91, 0xC2, 0x81, 0x43, 0x04, 0x30, 0x21, 0xC2, 0x45, 0xCA, 0x5A },
            new byte[]{ 0x3A, 0x94, 0xD1, 0x36, 0xE8, 0x92, 0xAF, 0x2C, 0xBB, 0x68, 0x6B, 0x22, 0x3C, 0x97, 0x23, 0x92 },
            new byte[]{ 0xB4, 0x71, 0x10, 0xE5, 0x58, 0xB9, 0xBA, 0x6C, 0xEB, 0x86, 0x58, 0x22, 0x38, 0x92, 0xBF, 0xD3 },
            new byte[]{ 0x8D, 0x12, 0xE1, 0x24, 0xDD, 0xFD, 0x3D, 0x93, 0x77, 0xC6, 0xF0, 0xAE, 0xE5, 0x3C, 0x86, 0xDB },
            new byte[]{ 0xB1, 0x12, 0x22, 0xCB, 0xE3, 0x8D, 0xE4, 0x83, 0x9C, 0xA0, 0xEB, 0xFF, 0x68, 0x62, 0x60, 0xBB },
            new byte[]{ 0x7D, 0xF7, 0x2B, 0xC7, 0x4E, 0x1A, 0xB9, 0x2D, 0x9C, 0xD1, 0xE4, 0xE2, 0xDC, 0xD3, 0x4B, 0x73 },
            new byte[]{ 0x4E, 0x92, 0xB3, 0x2C, 0xC4, 0x15, 0x14, 0x4B, 0x43, 0x1B, 0x30, 0x61, 0xC3, 0x47, 0xBB, 0x43 },
            new byte[]{ 0x99, 0x68, 0xEB, 0x16, 0xDD, 0x31, 0xB2, 0x03, 0xF6, 0xEF, 0x07, 0xE7, 0xA8, 0x75, 0xA7, 0xDB },
            new byte[]{ 0x2C, 0x47, 0xCA, 0x7E, 0x02, 0x23, 0x5E, 0x8E, 0x77, 0x59, 0x75, 0x3C, 0x4B, 0x61, 0xF3, 0x6D },
            new byte[]{ 0xF9, 0x17, 0x86, 0xB8, 0xB9, 0xE5, 0x1B, 0x6D, 0x77, 0x7D, 0xDE, 0xD6, 0x17, 0x5A, 0xA7, 0xCD },
            new byte[]{ 0x5D, 0xEE, 0x46, 0xA9, 0x9D, 0x06, 0x6C, 0x9D, 0xAA, 0xE9, 0xA8, 0x6B, 0xF0, 0x43, 0x6B, 0xEC },
            new byte[]{ 0xC1, 0x27, 0xF3, 0x3B, 0x59, 0x11, 0x53, 0xA2, 0x2B, 0x33, 0x57, 0xF9, 0x50, 0x69, 0x1E, 0xCB },
            new byte[]{ 0xD9, 0xD0, 0x0E, 0x60, 0x53, 0x03, 0xED, 0xE4, 0x9C, 0x61, 0xDA, 0x00, 0x75, 0x0C, 0xEE, 0x2C },
            new byte[]{ 0x50, 0xA3, 0xA4, 0x63, 0xBC, 0xBA, 0xBB, 0x80, 0xAB, 0x0C, 0xE9, 0x96, 0xA1, 0xA5, 0xB1, 0xF0 },
            new byte[]{ 0x39, 0xCA, 0x8D, 0x93, 0x30, 0xDE, 0x0D, 0xAB, 0x88, 0x29, 0x96, 0x5E, 0x02, 0xB1, 0x3D, 0xAE },
            new byte[]{ 0x42, 0xB4, 0x75, 0x2E, 0xA8, 0xF3, 0x14, 0x88, 0x0B, 0xA4, 0x54, 0xD5, 0x38, 0x8F, 0xBB, 0x17 },
            new byte[]{ 0xF6, 0x16, 0x0A, 0x36, 0x79, 0xB7, 0xB6, 0xAE, 0xD7, 0x7F, 0x42, 0x5F, 0x5B, 0x8A, 0xBB, 0x34 },
            new byte[]{ 0xDE, 0xAF, 0xBA, 0xFF, 0x18, 0x59, 0xCE, 0x43, 0x38, 0x54, 0xE5, 0xCB, 0x41, 0x52, 0xF6, 0x26 },
            new byte[]{ 0x78, 0xC9, 0x9E, 0x83, 0xF7, 0x9C, 0xCA, 0xA2, 0x6A, 0x02, 0xF3, 0xB9, 0x54, 0x9A, 0xE9, 0x4C },
            new byte[]{ 0x35, 0x12, 0x90, 0x22, 0x28, 0x6E, 0xC0, 0x40, 0xBE, 0xF7, 0xDF, 0x1B, 0x1A, 0xA5, 0x51, 0xAE },
            new byte[]{ 0xCF, 0x59, 0xA6, 0x48, 0x0F, 0xBC, 0x73, 0xC1, 0x2B, 0xD2, 0x7E, 0xBA, 0x3C, 0x61, 0xC1, 0xA0 },
            new byte[]{ 0xA1, 0x9D, 0xC5, 0xE9, 0xFD, 0xBD, 0xD6, 0x4A, 0x88, 0x82, 0x28, 0x02, 0x03, 0xCC, 0x6A, 0x75 },
        };

        private static readonly byte[,] S =
        {
            { 0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76 },
            { 0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0 },
            { 0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15 },
            { 0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75 },
            { 0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84 },
            { 0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF },
            { 0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8 },
            { 0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2 },
            { 0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73 },
            { 0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB },
            { 0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79 },
            { 0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08 },
            { 0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A },
            { 0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E },
            { 0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF },
            { 0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16 },
        };

        private static byte SBox(byte x)
        {
            return S[(uint)x >> 4, x & 0xFU];
        }

        private static byte[] SubBytes(byte[] s)
        {
            byte[] output = new byte[s.Length];
            for(int i = 0; i < 16; ++i)
            {
                output[i] = SBox(s[i]);
            }
            return output;
        }

        private static byte[] ShiftRows(byte[] s)
        {
            return new byte[]{
                s[0], s[5], s[10], s[15],
                s[4], s[9], s[14], s[3],
                s[8], s[13], s[2], s[7],
                s[12], s[1], s[6], s[11]
            };
        }

        internal static byte[] AesEnc(byte[] s, byte[] rk)
        {
            s = SubBytes(s);
            s = ShiftRows(s);
            s = MixColumns(s);
            XorTo(rk, s);
            return s;
        }

        private static byte MulX(byte p)
        {
            return (byte)(((p & 0x7F) << 1) ^ (((uint)p >> 7) * 0x1BU));
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        internal static void Xor(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y, Span<byte> z)
        {
            for (int i = 0; i < z.Length; i++)
            {
                z[i] = (byte)(x[i] ^ y[i]);
            }
        }
#else
        internal static byte[] Xor(byte[] x, byte[] y, int yStart)
        {
            byte[] output = new byte[16];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = (byte)(x[i] ^ y[yStart++]);
            }
            return output;
        }
#endif

        private static void XorTo(byte[] x, byte[] z)
        {
            for (int i = 0; i < 16; i += 4)
            {
                z[i + 0] ^= x[i + 0];
                z[i + 1] ^= x[i + 1];
                z[i + 2] ^= x[i + 2];
                z[i + 3] ^= x[i + 3];
            }
        }

        private static byte[] MixColumns(byte[] s)
        {
            byte[] output = new byte[s.Length];
            int j = 0, i4;
            for (int i = 0; i < 4; i++)
            {
                i4 = i << 2;
                output[j++] = (byte)(MulX(s[i4]) ^ MulX(s[i4 + 1]) ^ s[i4 + 1] ^ s[i4 + 2] ^ s[i4 + 3]);
                output[j++] = (byte)(s[i4] ^ MulX(s[i4 + 1]) ^ MulX(s[i4 + 2]) ^ s[i4 + 2] ^ s[i4 + 3]);
                output[j++] = (byte)(s[i4] ^ s[i4 + 1] ^ MulX(s[i4 + 2]) ^ MulX(s[i4 + 3]) ^ s[i4 + 3]);
                output[j++] = (byte)(MulX(s[i4]) ^ s[i4] ^ s[i4 + 1] ^ s[i4 + 2] ^ MulX(s[i4 + 3]));
            }

            return output;
        }

        public abstract string AlgorithmName { get; }

        public int GetDigestSize()
        {
            return DIGEST_SIZE;
        }

        public abstract int GetByteLength();

        public abstract void Update(byte input);

        public abstract void BlockUpdate(byte[] input, int inOff, int length);

        public abstract int DoFinal(byte[] output, int outOff);

        public abstract void Reset();

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER || UNITY_2021_2_OR_NEWER
        public abstract void BlockUpdate(ReadOnlySpan<byte> input);

        public abstract int DoFinal(Span<byte> output);
#endif
    }
}
#pragma warning restore
#endif
