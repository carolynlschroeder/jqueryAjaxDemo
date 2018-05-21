using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using jQueryAjaxDemo.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace jQueryAjaxDemo.BusinessLogic
{
    public class AzureMethods
    {
        public static void AddToAzureStorage(byte[] fileData, string fileName)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobContainerName = ConfigurationManager.AppSettings["BlobContainerName"];
            var blobContainer = blobClient.GetContainerReference(blobContainerName);
            AddToStorage(fileData, fileName, blobContainer);
        }

        private static void AddToStorage(byte[] fileData, string fileName, CloudBlobContainer blobContainer)
        {
            var containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            blobContainer.SetPermissions(containerPermissions);
            var blockBlob = blobContainer.GetBlockBlobReference(fileName);
            using (var stream = new MemoryStream(fileData, writable: false))
            {
                blockBlob.UploadFromStream(stream);
            }
        }


        public static List<BlobImageModel> GetBlobs()
        {
            var list = new List<BlobImageModel>();

            var cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobContainerName = ConfigurationManager.AppSettings["BlobContainerName"];
            var blobContainer = blobClient.GetContainerReference(blobContainerName);
            foreach (IListBlobItem item in blobContainer.ListBlobs(null, true))
            {
                var blobImage = new BlobImageModel();
                var b = (CloudBlockBlob)item;
                blobImage.BlobImageUri = b.Uri.ToString();
                blobImage.BlobImageName = b.Uri.Segments.Last();
                blobImage.LastModifiedDate = b.Properties.LastModified.Value.UtcDateTime;
                list.Add(blobImage);
            }

            return list;
        }
    }
}