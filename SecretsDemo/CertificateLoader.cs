using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SecretsDemo
{
    static public class CertificateLoader
    {
        private static X509Certificate2 GetCertificateFromStore(StoreName storeName, StoreLocation storeLocation, string thumbPrint)
        {
            if (String.IsNullOrWhiteSpace(thumbPrint)) return null;

            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            return store.Certificates
                        .Find(X509FindType.FindByThumbprint, thumbPrint, false)
                        .OfType<X509Certificate2>()
                        .SingleOrDefault();
        }

        private static X509Certificate2 GetMachineCertificate(string thumbPrint)
        {
            return GetCertificateFromStore(StoreName.My, StoreLocation.LocalMachine, thumbPrint);
        }
        private static X509Certificate2 GetUserCertificate(string thumbPrint)
        {
            return GetCertificateFromStore(StoreName.My, StoreLocation.CurrentUser, thumbPrint);
        }

        public static X509Certificate2 GetCertificate(string thumbPrint)
        {
            return GetMachineCertificate(thumbPrint) ?? GetUserCertificate(thumbPrint);
        }
    }
}
