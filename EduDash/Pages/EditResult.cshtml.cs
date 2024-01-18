using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduDash.Pages
{
	public class EditResultModel : PageModel
    {
        [BindProperty]
        public int studentNumber { get; set; }
        [BindProperty]
        public string courseName { get; set; }
        [BindProperty]
        public string lecturer { get; set; }
        [BindProperty]
        public int examResult { get; set; }
        
        public int idxNumber { get; set; }
        public string[] infoData { get; set; }
        public string[] lastData { get; set; }
        
        public void OnGet(int idx)
        {
            HttpContext.Session.SetInt32("idx", idx);
            var loginState = bool.Parse(HttpContext.Session.GetString("loginState"));

            if (!loginState)
            {
                RedirectToPage("/Index");
            }
            else
            {
                var dataHandler = new Data.DataHandler();
                string[] allLines = dataHandler.readResultsData();
                infoData = allLines[idx - 1].Split(",");
                
                // check null or not
                if (studentNumber == null || studentNumber == 0)
                {
                    studentNumber = int.Parse(infoData[1]);
                    HttpContext.Session.SetInt32("recordStuNo", studentNumber);
                } 
                
                if (lecturer == null || lecturer == "")
                {
                    lecturer = infoData[3];
                    HttpContext.Session.SetString("recordLecturer", lecturer);
                }
            }
        }

        public IActionResult OnPost()
        {
            var idxNo = HttpContext.Session.GetInt32("idx");
            var recordStuNo = HttpContext.Session.GetInt32("recordStuNo");
            var recordLecturer = HttpContext.Session.GetString("recordLecturer");
            
            var filePath = "./Data/Results.txt";
            List<string> allLines = System.IO.File.ReadAllLines(filePath).ToList();
            lastData = allLines[(int)idxNo - 1].Split(",");
            
            if (idxNo >= 0 && idxNo <= allLines.Count)
            {
                lastData[0] = idxNo.ToString();
                lastData[1] = recordStuNo.ToString();
                lastData[2] = courseName;
                lastData[3] = recordLecturer;
                lastData[4] = examResult.ToString();

                allLines[(int)idxNo - 1] = string.Join(",", lastData);
            }
            
            System.IO.File.WriteAllText(filePath, string.Empty);
            System.IO.File.WriteAllLines(filePath, allLines);
            
            return RedirectToPage("Dashboard");
        }
    }
}
