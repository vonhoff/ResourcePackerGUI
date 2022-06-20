namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IAesEncryptionService
    {
        bool DecryptCbc(byte[] input, out byte[] output, uint[] key,
            IProgress<int>? progress = null, int progressReportInterval = 100,
            CancellationToken cancellationToken = default);

        bool EncryptCbc(byte[] input, out byte[] output, uint[] key,
            IProgress<int>? progress = null, int progressReportInterval = 100,
            CancellationToken cancellationToken = default);

        uint[] KeySetup(string password);
    }
}