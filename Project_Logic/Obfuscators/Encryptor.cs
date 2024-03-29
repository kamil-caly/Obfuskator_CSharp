﻿using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace Project_Logic.Obfuscators
{
    public class Encryptor
    {
        private readonly int Salt;

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
            {50, 'y'}, {51, 'z'}, {52, '_'}
        };

        private readonly int moduloChar;

        public Encryptor()
        {
            moduloChar = chars.Count;
            Salt = new Random().Next(moduloChar);
        }

        public string Encrypt(string InputText)
        {
            string encryptedText = "";

            foreach (char character in InputText)
            {
                int charValue = chars.FirstOrDefault(c => c.Value == character).Key;
                encryptedText += chars.FirstOrDefault(c => c.Key == (charValue + Salt - Salt % 2) % moduloChar).Value.ToString();
                encryptedText += chars.FirstOrDefault(c => c.Key == (charValue + Salt + (Salt % 2 - 4)) % moduloChar).Value.ToString();
                encryptedText += chars.FirstOrDefault(c => c.Key == (charValue + Salt + Salt % 2 + 10) % moduloChar).Value.ToString();
            }

            return encryptedText;
        }
    }
}