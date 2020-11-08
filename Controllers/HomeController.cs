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






        // INDEX (Get) -------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<StudentModel> students = new List<StudentModel>();
            HttpClient client = APIclientConnection();
            HttpResponseMessage apiResponse = await client.GetAsync("api/StudentController/getAll");

            if (apiResponse.IsSuccessStatusCode)
            {
                var results = apiResponse.Content.ReadAsStringAsync().Result;
                students = JsonConvert.DeserializeObject<List<StudentModel>>(results);
            }

            return View(students);
        }



        // DETAILS (Get) --------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            HttpResponseMessage response = await APIclientConnection().GetAsync($"api/StudentController/GetSingle/{Id}");

            var student = new StudentModel();
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<StudentModel>(result);
            }

            return View(student);
        }



        // DELETE (Get) --------------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            // ask the api to delete the student
            HttpResponseMessage response = await APIclientConnection().DeleteAsync($"api/StudentController/Delete/{Id}");

            return RedirectToAction("Index");
        }



        // CREATE (Get) --------------------------------------------------------------
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        // CREATE (Post) ----------------------------------------------------------------------
        [HttpPost]
        public IActionResult Create(StudentModel newStudent)
        {
            // Ask the Api's create method to save our json data to it's database
            var apiPostRequest = APIclientConnection().PostAsJsonAsync<StudentModel>("api/StudentController/Create", newStudent);
            apiPostRequest.Wait();

            // if success
            if (apiPostRequest.Result.IsSuccessStatusCode) {return RedirectToAction("Index");}

            // if fail
            return View();
        }



    }
}
