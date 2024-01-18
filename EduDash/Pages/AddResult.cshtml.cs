using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduDash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduDash.Pages
{
	public class AddResultModel : PageModel
    {
        [BindProperty]
        public int studentNumber { get; set; }
        [BindProperty]
        public string courseName { get; set; }
        [BindProperty]
        public string lecturer { get; set; }
        [BindProperty]
        public int examResult { get; set; }


        public IActionResult OnGet()
        {
            var userType = HttpContext.Session.GetInt32("userType");
            var loginState = HttpContext.Session.GetString("loginState");

            if (bool.Parse(loginState))
            {
                if (userType != 1)
                {
                    return RedirectToPage("/Dashboard");
                }
            } else
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (studentNumber <= 10000000)
            {
                ViewData["invalidStudentNo"] = "  (Invalid student number!)";
            } else if (examResult < 0 || examResult > 100)
            {
                ViewData["resultRange"] = "  (Score must be between 0-100!)";
            } else
            {
                var userName = HttpContext.Session.GetString("userName");
                var dataHandler = new DataHandler();
                string[] data = dataHandler.readResultsData();
                int maxId = 0;

                foreach (string line in data)
                {
                    string[] splittedData = line.Split(",");

                    if (splittedData.Length >= 1 && int.TryParse(splittedData[0], out int currentId))
                    {
                        maxId = Math.Max(maxId, currentId);
                    }
                }

                System.IO.File.AppendAllText("./Data/Results.txt", $"{maxId + 1},{studentNumber},{courseName},{userName},{examResult}{Environment.NewLine}");

                ViewData["addedSuccessfully"] = "Result added successfully!";
            }

            return Page();
        }
    }
}
