using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduDash.Data;
using System.Reflection;

namespace EduDash.Pages
{
	public class DashboardModel : PageModel
    {
        public List<Data.Results> userResults { get; set; }
        public List<Data.Results> allResults { get; set; }

        public bool isStudent { get; set; }
        public bool isLecturer { get; set; }
        public bool isSearched { get; set; }

        public double averageResult { get; set; }

        public IActionResult OnGet(string searchString)
        {
            var loginState = HttpContext.Session.GetString("loginState");

            if (bool.Parse(loginState))
            {
                ViewData["CurrentFilter"] = searchString;

                var userType = HttpContext.Session.GetInt32("userType");

                if (userType == 0)
                {
                    isStudent = true;

                    var studentNumber = HttpContext.Session.GetInt32("studentNumber");
                    userResults = getResultsByStudentNumber((int)studentNumber);
                }
                else if (userType == 1)
                {
                    isLecturer = true;

                    allResults = listAllResults();

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        var filteredList = allResults
                        .Where(obj => !string.IsNullOrEmpty(searchString) &&
                            obj.studentNumber.ToString().Contains(searchString))
                        .ToList();

                        allResults = filteredList;
                    }
                    else
                    {
                        allResults = listAllResults();
                    }
                }

                return Page();
            } else
            {
                return RedirectToPage("/Login");
            }
        }

        public List<Data.Results> getResultsByStudentNumber(int studentNo)
        {
            var results = new List<Data.Results>();
            var dataHandler = new DataHandler();
            string[] data = dataHandler.readResultsData();

            foreach (string line in data)
            {
                string[] splittedLines = line.Split(",");

                if (splittedLines.Length == 5)
                {
                    if (int.Parse(splittedLines[1]) == studentNo)
                    {
                        results.Add(new Data.Results
                        {
                            idx = 0,
                            studentNumber = int.Parse(splittedLines[1]),
                            courseName = splittedLines[2],
                            lecturer = splittedLines[3],
                            examResult = int.Parse(splittedLines[4])
                        });
                    }
                }
            }

            return results;
        }

        public List<Data.Results> listAllResults()
        {
            var allResults = new List<Data.Results>();
            var dataHandler = new DataHandler();
            string[] data = dataHandler.readResultsData();

            foreach (string line in data)
            {
                string[] splittedLines = line.Split(",");

                if (splittedLines.Length == 5)
                {
                    allResults.Add(new Data.Results
                    {
                        idx = int.Parse(splittedLines[0]),
                        studentNumber = int.Parse(splittedLines[1]),
                        courseName = splittedLines[2],
                        lecturer = splittedLines[3],
                        examResult = int.Parse(splittedLines[4])
                    });
                }
            }

            return allResults;
        }
    }
}
