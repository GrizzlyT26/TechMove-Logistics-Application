using System.IO;
using Xunit;

namespace TechMove_Logistics_Application.Tests
{
    public class FileUploadValidationTests
    {
        [Fact]
        public void FileUpload_ShouldAcceptPdfOnly()
        {
            string fileName = "contract.pdf";

            bool isPdf =
                Path.GetExtension(fileName)
                .ToLower() == ".pdf";

            Assert.True(isPdf);
        }

        [Fact]
        public void FileUpload_ShouldRejectExeFiles()
        {
            string fileName = "virus.exe";

            bool isPdf =
                Path.GetExtension(fileName)
                .ToLower() == ".pdf";

            Assert.False(isPdf);
        }

        [Fact]
        public void FileUpload_ShouldRejectTxtFiles()
        {
            string fileName = "notes.txt";

            bool isPdf =
                Path.GetExtension(fileName)
                .ToLower() == ".pdf";

            Assert.False(isPdf);
        }
    }
}