using System.Runtime.InteropServices;

namespace Obfuskator
{
    public class Encryptor
    {
        public string inputText { private get; set; }

        private readonly Dictionary<int, char> chars = new() {
            {0, 'A'}, {1, 'B'}, {2, 'C'}, {3, 'D'}, {4, 'E'},
            {5, 'F'}, {6, 'G'}, {7, 'H'}, {8, 'I'}, {9, 'J'},
            {10, 'K'}, {11, 'L'}, {12, 'M'}, {13, 'N'}, {14, 'O'},
            {15, 'P'}, {16, 'Q'}, {17, 'R'}, {18, 'S'}, {19, 'T'},
            {20, 'U'}, {21, 'V'}, {22, 'W'}, {23, 'X'}, {24, 'Y'},
            {25, 'Z'}, {26, 'a'}, {27, 'b'}, {28, 'c'}, {29, 'd'},
            {30, 'e'}, {31, 'f'}, {32, 'g'}, {33, 'h'}, {34, 'i'},
            {35, 'j'}, {36, 'k'}, {37, 'l'}, {38, 'm'}, {39, 'n'},
            {40, 'o'}, {41, 'p'}, {42, 'q'}, {43, 'r'}, {44, 's'},
            {45, 't'}, {46, 'u'}, {47, 'v'}, {48, 'w'}, {49, 'x'},
            {50, 'y'}, {51, 'z'}, {52, '0'}, {53, '1'}, {54, '2'},
            {55, '3'}, {56, '4'}, {57, '5'}, {58, '6'}, {59, '7'},
            {60, '8'}, {61, '9'}, {62, '_'}
        };

        private readonly int moduloChar;

        public Encryptor(string text)
        {
            inputText = text;
            moduloChar = chars.Count;
        }
        private int GenerateSalt()
        {
            int salt = 0;
            foreach (char character in inputText)
            {
                int charValue = chars.FirstOrDefault(c => c.Value == character).Key;
                salt += charValue;
                salt = (salt * 13) % 256;
                salt = (salt + 7) % 256;
            }
            return salt % moduloChar;
        }

        public string Encrypt()
        {
            int key = GenerateSalt();
            string encryptedText = "";

            foreach (char character in inputText)
            {
                int charValue = chars.FirstOrDefault(c => c.Value == character).Key;
                encryptedText += chars.FirstOrDefault(c => c.Key == (charValue + key - (key % 2)) % moduloChar).Value.ToString();
            }
            return encryptedText;
        }

        public string Decrypt()
        {
            int key = GenerateSalt();
            string encryptedText = Encrypt();
            string decryptedText = "";

            foreach (char character in encryptedText)
            {
                int charValue = chars.FirstOrDefault(c => c.Value == character).Key;

                // dodajemy moduloChar przed modulo, aby uniknąć ujemnych wartości
                decryptedText += chars.FirstOrDefault(c => c.Key == (charValue - key + (key % 2) + moduloChar) % moduloChar).Value.ToString();
            }
            return decryptedText;
        }
    }

}