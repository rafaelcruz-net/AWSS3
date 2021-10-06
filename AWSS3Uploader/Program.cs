using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;

namespace AWSS3Uploader
{
    class Program
    {
        static void Main(string[] args)
        {
            IAmazonS3 s3Client;

            RegionEndpoint regionEndpoint = RegionEndpoint.USWest2;

            s3Client = new AmazonS3Client(regionEndpoint);

            var transfer = new TransferUtility(s3Client);

            var fileName = $"{Guid.NewGuid().ToString()}.rec";
            using (var file = File.Open($"{fileName}", FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine("nome;cpf;valor;data");

                    for (int i = 0; i < 100; i++)
                    {
                        writer.WriteLine($"CodersInRio{i};{i.ToString().PadLeft(11, '0')};{new Random().Next(1, 10000).ToString("N2")};{DateTime.Now.AddHours(i)}");
                    }

                    writer.Flush();
                }
            }


            var fileUpload = File.Open($"{fileName}", FileMode.OpenOrCreate);

            transfer.UploadAsync(fileUpload, bucketName: "codersinriobucket", key: fileName).Wait();

            Console.WriteLine("Upload realizado com sucesso");

            Console.ReadKey();
        }
    }
}
