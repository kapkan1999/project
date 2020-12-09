using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using LAB.Models;
using LAB.Storage;
using Serilog;

namespace LAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly StorageService _storageService;

        public StorageController(StorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet]
        [Route("storageType")]
        public ActionResult<string> GetStorageType()
        {
            return Ok(_storageService.GetStorageType());
        }

        [HttpGet]
        [Route("itemsCount")]
        public ActionResult<string> GetItemsCount()
        {
            return Ok(_storageService.GetNumberOfItems());
        }
    }
}