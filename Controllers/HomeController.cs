using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using apiRequest.Models;
using apiRequest.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;

namespace apiRequest.Controllers
{
    public class HomeController : Controller
    {
        StudentAPI _api = new StudentAPI();

        public async Task<IActionResult> Index()
        {
            // List<StudentData> students = new List<StudentData>();
            // HttpClient client = _api.Initial();
            // HttpResponseMessage res = await client.GetAsync("api/student");
            // if (res.IsSuccessStatusCode)
            // {
            //     var results = res.Content.ReadAsStringAsync().Result;
            //     students = JsonConvert.DeserializeObject<List<StudentData>>(results);
            // }
            var students = GetStudentData();
            return View(students);
        }

        protected List<StudentData> GetStudentData()
        {
            var client = new RestClient("https://localhost:6001");

            // Allow self signed certificates through if in Dev mode
            if (true)
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var request = new RestRequest("api/student", Method.GET);

            request.AddHeader("Content-Type", "application/json; charset=utf-8");

            // request.AddParameter("pageIndex", search.PageNumber);
            // request.AddParameter("pageSize", search.PageSize);
            // request.AddParameter("sortOrder", search.SortOrder);
            // request.AddParameter("ascend", search.Ascend);
            // request.AddParameter("searchTerm", search.SearchTerm);
            // request.AddParameter("companies", search.Filters.GetSearchFilter("company"));
            // request.AddParameter("tails", search.Filters.GetSearchFilter("tail"));

            var response = client.Execute<List<StudentData>>(request);

            if (!response.IsSuccessful)
            {
                var respUri = client.BuildUri(request);
                var failedMessage = $"Failed to get GX SIU Log summaries {respUri}";
                // _logger.Error(failedMessage,
                //     new
                //     {
                //         HttpStatusCode = response.StatusCode,
                //         HttpErrorMessage = response.ErrorMessage,
                //         response.Content,
                //         response.ErrorMessage,
                //         Request=request
                //     });
                
                throw new Exception(failedMessage);
            }

            return response.Data;
        }












        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
