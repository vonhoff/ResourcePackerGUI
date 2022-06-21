namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IAesEncryptionService
    {
        bool DecryptCbc(byte[] input, out byte[] output, uint[] key, IProgress<int>? progress,
            int progressReportInterval, CancellationToken cancellationToken);

        bool EncryptCbc(byte[] input, out byte[] output, uint[] key, IProgress<int>? progress,
            int progressReportInterval, CancellationToken cancellationToken);

        uint[] KeySetup(string password);
    }
}