// See https://aka.ms/new-console-template for more information
using Obfuskator;

class Program
{
    static void Main()
    {
        string text = "To_jest_przykladowy_teks"; 
        Encryptor encryptor = new Encryptor();

        string encryptedText = encryptor.Encrypt(text);
        Console.WriteLine($"Encrypted text: {encryptedText}");

        string decryptedText = encryptor.Decrypt(encryptedText);
        Console.WriteLine($"Decrypted text: {decryptedText}");

        string encryptedText2 = encryptor.Encrypt("jakis_inny_tekst");
        Console.WriteLine($"Encrypted text2: {encryptedText2}");

        string decryptedText2 = encryptor.Decrypt(encryptedText2);
        Console.WriteLine($"Decrypted text: {decryptedText2}");

        Console.ReadKey();
    }
}
