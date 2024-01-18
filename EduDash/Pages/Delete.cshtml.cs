using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduDash.Pages
{
	public class DeleteModel : PageModel
    {
        public IActionResult OnGet(int idx)
        {
            var loginState = bool.Parse(HttpContext.Session.GetString("loginState"));

            if (!loginState)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (idx != null)
                {
                    DeleteResult(idx);
                    ViewData["deletedSuccessfully"] = "Result deleted successfully.";
                    return RedirectToPage("/Dashboard");
                }
                else
                {
                    return RedirectToPage("/Dashboard");
                }
            }
        }

        private void DeleteResult(int idx)
        {
            var filePath = "./Data/Results.txt";
            List<string> allLines = System.IO.File.ReadAllLines(filePath).ToList();
            int idxid = 0;
            
            if (idx >= 0 && idx <= allLines.Count)
            {
                allLines.RemoveAt(idx - 1);

                for (int i = 0; i < allLines.Count; i++)
                {
                    var arrayLines = allLines.ToArray();
                    ViewData["debug"] = arrayLines[i][0];
                }
            }
            
            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllLines(filePath, allLines);
        }
    }
}
