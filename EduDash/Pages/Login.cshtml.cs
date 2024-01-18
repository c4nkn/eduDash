using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduDash.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduDash.Data;
using Microsoft.AspNetCore.Http;

namespace EduDash.Pages
{
	public class LoginModel : PageModel
    {
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string password { get; set; }
        [ViewData]
        public string errorMessage { get; set; }

        public bool invalidEntry { get; set; }

        public IActionResult OnPost()
        {
            var dataHandler = new DataHandler();
            string[] data = dataHandler.readUserData();

            foreach (string line in data)
            {
                string[] splittedLines = line.Split(",");

                if (splittedLines.Length == 6 && splittedLines[4].Trim() == email && splittedLines[5].Trim() == password)
                {
                    HttpContext.Session.SetString("userEmail", email);
                    HttpContext.Session.SetInt32("userType", int.Parse(splittedLines[1]));
                    HttpContext.Session.SetString("userName", splittedLines[3]);
                    HttpContext.Session.SetString("userSurname", splittedLines[4]);
                    HttpContext.Session.SetInt32("studentNumber", int.Parse(splittedLines[2]));
                    HttpContext.Session.SetString("loginState", "true");

                    return RedirectToPage("/Dashboard");
                }
                else
                {
                    invalidEntry = true;
                    ViewData["matchingError"] = "Password and email doesn't match.";
                }
            }

            return Page();
        }

        public IActionResult OnGet()
        {
            var userEmail = HttpContext.Session.GetString("userEmail");

            if (!string.IsNullOrEmpty(userEmail))
            {
                // user already logged in
                return RedirectToPage("/Dashboard");
            } else
            {
                HttpContext.Session.SetString("loginState", "false");
                invalidEntry = false;
            }

            return Page();
        }
    }
}
