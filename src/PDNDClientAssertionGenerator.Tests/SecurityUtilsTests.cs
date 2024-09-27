// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using PDNDClientAssertionGenerator.Utils;

namespace PDNDClientAssertionGenerator.Tests
{
    public class SecurityUtilsTests
    {
        [Fact]
        public void ExtractBase64Key_ShouldRemoveHeaderFooterAndWhitespace_FromValidPemContent()
        {
            // Arrange
            string pemContent = @"
                -----BEGIN RSA PRIVATE KEY-----
                MIIBOgIBAAJBAK5QpQfskLq1djoi0yRz4ksblMdxI0m5lBw9fAWvntA59NgIUlHw
                fNhgUJmOygwoQ6dzQbPUZp0ZEOtR10Q+/gECAwEAAQJAR1XlnMvJ0IbG4P1Rb0P/
                gRJyOgkMxybMfzoVr8f+f4IkH2XfsnBhdCdHkHhbtRfct+dM+7Rp3JFd+n+6IOCC
                RQIhAPPAz9jYPX+3oBlfV92MdhLB3UjsoXTvGaMDHrG7PjCFAiEAuSMOFJNGFmRJ
                kYmujA6SeyDEzJHpxEnbx9FA41gKkBcCIQDHaXsDBL2/WPBOFcOfTLNfBQXoTpEu
                AfWRAd5Nvg0I9QIhAL3n2dfYFXAGpPCTg2MgttVfSe+oAapTknnCK6CRz58nAiEA
                4lYtY4jOOBVZWz1vUpcsWgGVpRfyRbGmJfrJ6UAKfBM=
                -----END RSA PRIVATE KEY-----
                ";

            string expectedBase64Key = "MIIBOgIBAAJBAK5QpQfskLq1djoi0yRz4ksblMdxI0m5lBw9fAWvntA59NgIUlHwfNhgUJmOygwoQ6dzQbPUZp0ZEOtR10Q+/gECAwEAAQJAR1XlnMvJ0IbG4P1Rb0P/gRJyOgkMxybMfzoVr8f+f4IkH2XfsnBhdCdHkHhbtRfct+dM+7Rp3JFd+n+6IOCCRQIhAPPAz9jYPX+3oBlfV92MdhLB3UjsoXTvGaMDHrG7PjCFAiEAuSMOFJNGFmRJkYmujA6SeyDEzJHpxEnbx9FA41gKkBcCIQDHaXsDBL2/WPBOFcOfTLNfBQXoTpEuAfWRAd5Nvg0I9QIhAL3n2dfYFXAGpPCTg2MgttVfSe+oAapTknnCK6CRz58nAiEA4lYtY4jOOBVZWz1vUpcsWgGVpRfyRbGmJfrJ6UAKfBM=";

            // Act
            string actualBase64Key = SecurityUtils.ExtractBase64Key(pemContent);

            // Assert
            Assert.Equal(expectedBase64Key, actualBase64Key);
        }

        [Fact]
        public void ExtractBase64Key_ShouldReturnEmptyString_WhenPemContentIsEmpty()
        {
            // Arrange
            string pemContent = "";
            string expectedBase64Key = "";

            // Act
            string actualBase64Key = SecurityUtils.ExtractBase64Key(pemContent);

            // Assert
            Assert.Equal(expectedBase64Key, actualBase64Key);
        }

        [Fact]
        public void ExtractBase64Key_ShouldReturnEmptyString_WhenPemContentIsOnlyHeaderFooter()
        {
            // Arrange
            string pemContent = @"
                -----BEGIN RSA PRIVATE KEY-----
                -----END RSA PRIVATE KEY-----
                ";
            string expectedBase64Key = "";

            // Act
            string actualBase64Key = SecurityUtils.ExtractBase64Key(pemContent);

            // Assert
            Assert.Equal(expectedBase64Key, actualBase64Key);
        }
    }
}
