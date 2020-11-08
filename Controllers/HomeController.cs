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
using System.Net;

namespace apiRequest.Controllers
{
    public class HomeController : Controller
    {
        // API CONNECTION ----------------------------------------------------------
        public HttpClient APIclientConnection()
        {
            var APIclient = new HttpClient();
            APIclient.BaseAddress = new Uri("https://localhost:6001");

            return APIclient;
        }
        // -------------------------------------------------------------------------



        // GET - INDEX -------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            List<StudentData> students = new List<StudentData>();
            HttpClient client = APIclientConnection();
            HttpResponseMessage apiResponse = await client.GetAsync("api/StudentController/getAll");

            if (apiResponse.IsSuccessStatusCode)
            {
                var results = apiResponse.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<StudentData>>(results);
            }

            return View(students);
        }



        // GET - DETAILS --------------------------------------------------------------
        public async Task<IActionResult> Details(int Id)
        {
            var student = new StudentData();
            HttpClient client = APIclientConnection();
            HttpResponseMessage response = await client.GetAsync($"api/StudentController/GetSingle/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<StudentData>(results);
            }
            return View(student);
        }




        // GET - CREATE --------------------------------------------------------------
        public IActionResult Create()
        {
            return View();
        }




        // POST - CREATE ----------------------------------------------------------------------
        [HttpPost]
        public IActionResult Create(StudentData student)
        {
            HttpClient client = APIclientConnection();

            var postTask = client.PostAsJsonAsync<StudentData>("api/StudentController/Create", student);

            postTask.Wait(); // need to rename this!

            if (postTask.Result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }



        // GET - DELETE --------------------------------------------------------------------------------
        public async Task<IActionResult> Delete(int Id)
        {
            var student = new StudentData();
            HttpClient client = APIclientConnection();
            HttpResponseMessage response = await client.DeleteAsync($"api/StudentController/Delete/{Id}");
            return RedirectToAction("Index");
        }



    }
}
