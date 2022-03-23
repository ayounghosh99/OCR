using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace OCR.CLI
{
    public class Program
    {
        private static List<string> URLs = new List<string>
        {
            "https://i.imgur.com/YicZvaE.jpg",
            "https://i.imgur.com/xH94ySW.jpg",
            "https://i.imgur.com/Ta1Zycu.jpg",
            "https://i.imgur.com/ZdJ9iV4.jpg",
            "https://i.imgur.com/bQe0StL.jpg",
            "https://i.imgur.com/oBFBD0P.jpg",
            "https://i.imgur.com/wtvbHG6.jpg",
            "https://i.imgur.com/Nz1bZ2P.jpg",
            "https://i.imgur.com/AEnbtfb.jpg",
            "https://i.imgur.com/bdYki7c.jpg",
            "https://i.imgur.com/WehGT3C.jpg",
        };

        public static void Main(string[] args)
        {
            List<LicenseData> licenseDetails = new List<LicenseData>();

            foreach (var file in URLs)
            {
                Console.WriteLine("----------------------------------------------------------");
                Console.Write("Extracting text from file ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Path.GetFileName(file));
                Console.ResetColor();
                Console.WriteLine("...");
                Console.WriteLine();

                try
                {
                    var data = Ocr.ReadFileUrl(file).GetAwaiter().GetResult();
                    PrintLicenseDetails(data);
                    licenseDetails.Add(data);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    try
                    {
                        Console.WriteLine(((ComputerVisionErrorResponseException)e)?.Body?.Error?.Innererror?.Message ?? "An error has occurred while scanning the image.");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error has occurred while scanning the image.");
                    }
                    Console.ResetColor();
                }

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine();
            }

            Console.WriteLine("Parsing complete. Press any key to exit.");
            Console.ReadKey();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static void PrintLicenseDetails(LicenseData licenseData)
        {
            Console.WriteLine($"Driver's Name: {licenseData.DriverName}");
            Console.WriteLine($"Address:\n{licenseData.AddressLine1}\n{licenseData.AddressLine2}{(string.IsNullOrEmpty(licenseData.AddressLine3) ? string.Empty : $"\n{licenseData.AddressLine3}")}");
            Console.WriteLine($"Issue Date: {licenseData.IssueDate.Value.ToString("dd-MM-yyyy")}");
            Console.WriteLine($"Expiry Date: {licenseData.ExpiryDate.Value.ToString("dd-MM-yyyy")}");
            Console.WriteLine($"TR: {licenseData.Transaction}");
            Console.WriteLine($"Date of Birth: {licenseData.DateOfBirth.Value.ToString("dd-MM-yyyy")}");
            Console.WriteLine($"Sex: {licenseData.Sex}");
            Console.WriteLine($"Vehicle Class: {licenseData.Class}");
            Console.WriteLine($"Permit Number: {licenseData.PermitNumber} {licenseData.PermitNumberSuffix}");
            Console.WriteLine($"Date of Payment: {licenseData.DateOfPayment.Value.ToString("dd-MM-yyyy")}");
        }
    }
}
