using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduDash.Pages
{
	public class CalculatorModel : PageModel
    {
        [BindProperty]
        public int result1 { get; set; }
        
        [BindProperty]
        public int result2 { get; set; }
        
        [BindProperty]
        public int result3 { get; set; }
        
        public double averageResult { get; set; }
        
        public IActionResult OnGet()
        {
            var userType = HttpContext.Session.GetInt32("userType");
            var loginState = HttpContext.Session.GetString("loginState");

            if (!bool.Parse(loginState))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            int count = 3;
            int total = result1 + result2 + result3;
            averageResult = count > 0 ? (double)total / count : 0.0;

            return Page();
        }
    }
}
