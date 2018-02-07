using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RC4
    {
        /// <summary>
        /// Vrsi enkripciju podataka.
        /// </summary>
        /// <param name="pwd"> Kljuc kojim se enkriptuju podaci. </param>
        /// <param name="data"> Podaci koje je potrebno enkriptovati. </param>
        /// <returns> Sifra u vidu niza byte-ova duzine parametra data.</returns>
        public static byte[] Encrypt(byte[] pwd, byte[] data)
        {
            int a, i, j, k;
            int[] key, box;
            byte[] cipher;          // Sifra (cipher) je povratna vrednost ove funkcije.

            key = new int[256];
            box = new int[256];
            cipher = new byte[data.Length];

            for (i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd.Length];
                box[i] = i;
            }

            for (j = i = 0; i < 256; i++)
            {
                j = (j + box[i] + key[i]) % 256;
                swap(ref box[i], ref box[j]);
            }

            for (a = j = i = 0; i < data.Length; i++)
            {
                a++;
                a %= 256;
                j += box[a];
                j %= 256;
                swap(ref box[a], ref box[j]);
                k = box[((box[a] + box[j]) % 256)];
                cipher[i] = (byte)(data[i] ^ k);
            }

            return cipher;
        }

        /// <summary>
        /// Vrsi dekripciju podataka.
        /// </summary>
        /// <param name="pwd"> Kljuc za dekripciju podataka. </param>
        /// <param name="data"> Podaci koje je potrebno dekriptovati. </param>
        /// <returns> Sifra u vidu niza byte-ova duzine parametra data. </returns>
        public static byte[] Decrypt(byte[] pwd, byte[] data)
        {
            return Encrypt(pwd, data);
        }

        /// <summary>
        /// Zamenjivanjuje prosledjene vrednosti.
        /// </summary>
        /// <param name="a"> Prva promenljiva koja dobija vrednost druge. </param>
        /// <param name="b"> Druga promenljiva koja dobija vrednost prve. </param>
        private static void swap(ref int a, ref int b)
        {
            int temp;

            temp = a;
            a = b;
            b = temp;
        }
    }
}
