namespace Orari.Services
{
    public interface IPdfGenerateService
    {
        byte[] GenerateSchedulePdf(string title, string content);
    }
} 