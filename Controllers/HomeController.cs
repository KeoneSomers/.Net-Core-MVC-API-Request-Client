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
using apiRequest.Helpers;
using System.Net;

namespace apiRequest.Controllers
{
    public class HomeController : Controller
    {
        StudentAPI _api = new StudentAPI();

        // GET - INDEX
        public async Task<IActionResult> Index()
        {
            List<StudentData> students = new List<StudentData>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/student");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<StudentData>>(results);
            }

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
