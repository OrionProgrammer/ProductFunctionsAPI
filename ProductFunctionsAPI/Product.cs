using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using ProductFunctionsAPI.Models;
using System.ComponentModel;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;

namespace ProductFunctionsAPI;

public static class Product
{
    public static readonly List<ProductModel> Products = new List<ProductModel>();


    //Creata a new Product. Simply add to List mock DB
    [FunctionName("Create")]
    public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "post", Route = "product")]
            HttpRequest req, TraceWriter log)
    {
        log.Info("Creating a new product");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var newProduct = JsonConvert.DeserializeObject<ProductCreateUpdateModel>(requestBody);

        var product = new ProductModel()
        {
            Name = newProduct.Name,
            Description = newProduct.Description,
            InStock = newProduct.InStock
        };

        Products.Add(product);

        return new OkObjectResult(product);
    }


    //Fetch all Products. simply return List DB
    [FunctionName("GetAll")]
    public static IActionResult GetAll(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "get", Route = "product")]
            HttpRequest req, TraceWriter log)
    {
        log.Info("Fetching list of Products");

        return new OkObjectResult(Products);
    }

    //Get single Product by ID
    [FunctionName("GetSingleById")]
    public static IActionResult GetSingleById(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "get", Route = "product/{id}")]
            HttpRequest req,
            TraceWriter log, string id)
    {
        var product = Products.FirstOrDefault(t => t.Id == id);

        if (product == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(product);
    }

    //Update Product
    [FunctionName("Update")]
    public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "put", Route = "product/{id}")]
            HttpRequest req,
            TraceWriter log, string id)
    {
        var product = Products.FirstOrDefault(t => t.Id == id);

        if (product == null)
        {
            return new NotFoundResult();
        }

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var updatedProduct = JsonConvert.DeserializeObject<ProductCreateUpdateModel>(requestBody);

        product.InStock = updatedProduct.InStock;

        if (!string.IsNullOrEmpty(updatedProduct.Description))
        {
            product.Description = updatedProduct.Description;
        }

        return new OkObjectResult(product);
    }

    //Delete Product by ID
    [FunctionName("Delete")]
    public static IActionResult Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "delete", Route = "product/{id}")]
            HttpRequest req,
            TraceWriter log, string id)
    {
        var product = Products.FirstOrDefault(t => t.Id == id);

        if (product == null)
        {
            return new NotFoundResult();
        }

        Products.Remove(product);

        return new OkResult();
    }
}
