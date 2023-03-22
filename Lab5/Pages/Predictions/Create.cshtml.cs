using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5.Data;
using Lab5.Model;
using Azure.Storage.Blobs;
using Azure;

namespace Lab5.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        //private readonly Lab5.Data.PredictionDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        public CreateModel(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            IFormFile newfile = file;

            BlobContainerClient containerClient;
            string containerName;
            if (Prediction.Question.Equals(1))
            {
                containerName = computerContainerName;
            }
            if (Prediction.Question.Equals(0)) 
            {
                containerName = earthContainerName;
            }
            else
            {
                return Page();
            }

            
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }

            try
            {
                string randomFileName = Path.GetRandomFileName();
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(randomFileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }
            }
            catch (RequestFailedException)
            {
              //  Pages("Error");
            }

            return RedirectToPage("./Index");


            
        }
     }

    
    }

