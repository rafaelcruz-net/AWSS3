using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCS3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVCS3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile s3Form)
        {
            
            RegionEndpoint regionEndpoint = RegionEndpoint.USWest2;

            IAmazonS3 s3Client = new AmazonS3Client(regionEndpoint);

            var transfer = new TransferUtility(s3Client);

            MemoryStream ms = new MemoryStream();

            var uploadFile = s3Form.OpenReadStream();

            await uploadFile.CopyToAsync(ms);

            var fileName = s3Form.FileName;

            await transfer.UploadAsync(ms, "codersinriobucket", fileName);

            return View();
        }

      
    }
}
