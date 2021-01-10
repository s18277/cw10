namespace Cw10.Services.EncryptionServices
{
    public interface IEncryptionService
    {
        string Encrypt(string value);
        bool Verify(string candidatePlainString, string existingHashString);
    }
}