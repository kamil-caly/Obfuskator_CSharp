// See https://aka.ms/new-console-template for more information
using Obfuskator;

class Program
{
    static void Main()
    {
        string text = "To_jest_przykladowy_teks"; 
        Encryptor encryptor = new Encryptor(text);

        string encryptedText = encryptor.Encrypt();
        Console.WriteLine($"Encrypted text: {encryptedText}");

        string decryptedText = encryptor.Decrypt();
        Console.WriteLine($"Decrypted text: {decryptedText}");

        Console.ReadKey();
    }
}
