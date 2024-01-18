using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduDash.Pages
{
	public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            var loginState = HttpContext.Session.GetString("loginState");

            if (bool.Parse(loginState))
            {
                HttpContext.Session.SetString("loginState", "false");
                HttpContext.Session.Clear();

                return RedirectToPage("/Index");
            } else
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
