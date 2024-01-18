using System;
using System.IO;
using System.Linq;

namespace EduDash.Data
{
    public class DataHandler
	{
		public DataHandler()
		{
		}

        public string usersTxtDir = "Data/Users.txt";
		public string resultsTxtDir = "Data/Results.txt";

		public string[] readUserData()
		{
            string[] allLines = File.ReadAllLines(usersTxtDir);

            return allLines;
        }

		public string[] readResultsData()
		{
            string[] allLines = File.ReadAllLines(resultsTxtDir);

            return allLines;
        }
	}
}

