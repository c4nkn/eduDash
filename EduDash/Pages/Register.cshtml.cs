using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduDash.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduDash.Pages
{
	public class RegisterModel : PageModel
    {
        [BindProperty]
        public string userName { get; set; }
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string password { get; set; }
        [BindProperty]
        public string confirmPassword { get; set; }
        
        public bool invalidForm{ get; set; }

        public IActionResult OnPost()
        {
            Random random = new Random();
            var (emails, numbers, idx) = manipulateData();
            var studentNumber = generateRandomNumber(random);

            if (numbers.Contains(studentNumber))
            {
                studentNumber = generateRandomNumber(random);
            }

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                invalidForm = true;
                ViewData["blankArea"] = "Please fill in all blank areas.";
            }
            else if (password != confirmPassword)
            {
                invalidForm = true;
                ViewData["doesntMatch"] = "Passwords doesn't match!";
            }
            else if (emails.Contains(email.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                invalidForm = true;
                ViewData["registeredEmail"] = "Email is already registered!";
            }
            else
            {
                invalidForm = false;
                System.IO.File.AppendAllText("./Data/Users.txt", $"{idx},0,{studentNumber},{userName},{email},{password}{Environment.NewLine}");
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public int generateRandomNumber(Random random)
        {
            return 80000000 + random.Next(10000000);
        }

        public (List<string>, List<int>, int) manipulateData()
        {
            var emails = new List<string>();
            var numbers = new List<int>();
            var dataHandler = new DataHandler();
            string[] data = dataHandler.readUserData();
            int maxId = 0;

            foreach (string line in data)
            {
                string[] splittedData = line.Split(",");

                if (splittedData.Length >= 1 && int.TryParse(splittedData[0], out int currentId))
                {
                    maxId = Math.Max(maxId, currentId);
                }

                if (splittedData.Length >= 3)
                {
                    numbers.Add(int.Parse(splittedData[2]));
                }

                if (splittedData.Length >= 5)
                {
                    emails.Add(splittedData[4].Trim());
                }
            }

            return (emails, numbers, maxId + 1);
        }

        public IActionResult OnGet()
        {
            var loginState = HttpContext.Session.GetString("loginState");

            if (bool.Parse(loginState))
            {
                // true
                return RedirectToPage("/Dashboard");
            }

            return Page();
        }
    }
}
