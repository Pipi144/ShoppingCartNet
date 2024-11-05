using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Services;

public class StorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public StorageService()
    {
        _s3Client = new AmazonS3Client(RegionEndpoint.APSoutheast2);
        _bucketName = "shopping-cart-1417";
    }
    [HttpPost]
    public async Task<string?> AddFileS3(IFormFile formFile, string? prefixFolder = null)
    {
        try
        {
            var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
            if (!bucketExist)
            {
                var bucketRequest = new PutBucketRequest()
                {
                    BucketName = _bucketName,
                    UseClientRegion = true

                };
                await _s3Client.PutBucketAsync(bucketRequest);
            }

            string fileKey =
                $"{(prefixFolder != null ? $"{prefixFolder}/" : "")}{(DateTime.Now.ToString("yyyy-MM-dd-hhmmss"))}-{formFile.FileName}";
            // Set up the file upload request
            var fileRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                InputStream = formFile.OpenReadStream(), // Set the file's content stream
                ContentType = formFile.ContentType // Optional: sets the MIME type for the object
            };

            // Upload the file to S3
            await _s3Client.PutObjectAsync(fileRequest);
            return fileKey;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
    }
    [HttpGet]
    public string GeneratePreSignedUrl(string objectKey, TimeSpan expiryDuration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            Expires = DateTime.UtcNow.Add(expiryDuration)
        };

        string url = _s3Client.GetPreSignedURL(request);
        return url;
    }
    public async Task<Boolean> DeleteSingleFile(string s3Key)
    {
        try
        {
            // Delete from S3
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = s3Key
            });
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
        
    }

    public async Task OnPostDeleteFolderAsync(string folderName)
    {
        // Ensure folder name ends with a slash ("/") to target all objects within the folder
        if (!folderName.EndsWith("/"))
        {
            folderName += "/";
        }

        // List all objects under the folder prefix
        var listRequest = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = folderName
        };

        var objectsToDelete = new List<KeyVersion>();

        // Collect all objects in the folder
        ListObjectsV2Response listResponse;
        do
        {
            listResponse = await _s3Client.ListObjectsV2Async(listRequest);

            foreach (var s3Object in listResponse.S3Objects)
            {
                objectsToDelete.Add(new KeyVersion { Key = s3Object.Key });
            }

            listRequest.ContinuationToken = listResponse.NextContinuationToken;

        } while (listResponse.IsTruncated);

        // If there are objects to delete, perform the delete
        if (objectsToDelete.Count > 0)
        {
            var deleteRequest = new DeleteObjectsRequest
            {
                BucketName = _bucketName,
                Objects = objectsToDelete
            };

            await _s3Client.DeleteObjectsAsync(deleteRequest);
        }
    }
}