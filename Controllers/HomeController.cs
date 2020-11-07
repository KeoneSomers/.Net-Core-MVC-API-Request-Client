using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using apiRequest.Models;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;
using apiRequest.Helpers;
using System.Net;

namespace apiRequest.Controllers
{
    public class HomeController : Controller
    {
        StudentAPI _api = new StudentAPI();
        
        // PULL DATA FROM API (Using NuGet Package: RestSharp to simplfy things)
        protected List<StudentData> GetStudentData()
        {
            var client = new RestClient("https://localhost:6001");
            // Allow self signed certificates through if in Dev mode
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest("api/student", Method.GET);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = client.Execute<List<StudentData>>(request);
            return response.Data;
        }

        // GET - INDEX
        public IActionResult Index()
        {
            var students = GetStudentData();
            return View(students);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE (Send data back to api in the body of the response)
        [HttpPost]
        public IActionResult Create(StudentData student)
        {
            HttpClient client = _api.Initial(); // maybe should rename this to GetClient() rather than Initial()...

            var postTask = client.PostAsJsonAsync<StudentData>("api/student", student);   

            postTask.Wait();
            // if having trouble with ssl certificate use this...
            // dotnet dev-certs https --trust

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }





        // ERROR
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
