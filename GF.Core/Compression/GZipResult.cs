namespace GF.Core.Compression
{
    public class GZipResult
    {
        public GZipFileInfo[] Files = null;
        public int FileCount = 0;
        public long TempFileSize = 0;
        public long ZipFileSize = 0;
        public int CompressionPercent = 0;
        public string TempFile = null;
        public string ZipFile = null;
        public bool TempFileDeleted = false;
        public bool Errors = false;
    }
}