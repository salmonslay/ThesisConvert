using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ThesisConvert
{
    internal enum LineAuthor
    {
        Unknown,
        Interviewer,
        Interviewee,
    };

    internal class Program
    {
        private string _interviewer = "";
        private string _interviewee = "";
        private string _pseudonym = "";

        private static void Main(string[] args)
        {
            bool isRunning = true;
            while (isRunning)
                isRunning = new Program().Run();
        }

        private bool Run()
        {
            // Get interviewer
            Console.Write("Username of interviewer: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            _interviewer = Console.ReadLine() ?? "Salmon";
            Console.ResetColor();

            // Get interviewee
            Console.Write("Username of interviewee: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            _interviewee = Console.ReadLine() ?? "Bruh-mannen";
            Console.ResetColor();

            Console.Write("Pseudonym for interviewee: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            _pseudonym = Console.ReadLine() ?? "P3";
            Console.ResetColor();

            Console.WriteLine($"{_interviewer} is interviewing {_interviewee}.");
            Html html = new Html();
            html.AddHeader($"{_interviewer} is interviewing {_interviewee}.");

            // Get transcription path
            Console.Write("Path to transcript: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string path = Console.ReadLine() ?? "transcript.txt";
            Console.ResetColor();

            try
            {
                string[] lines = File.ReadAllLines(path);
                LineAuthor lastAuthor = LineAuthor.Unknown;
                int i = 0;
                foreach (string line in lines)
                {
                    // Get new line author
                    LineAuthor author = GetLineAuthor(line);
                    if (author == LineAuthor.Unknown)
                    {
                        html.AddText(lastAuthor == LineAuthor.Interviewer ? "I" : _pseudonym, line);
                        i++;
                    }

                    if (author != LineAuthor.Unknown)
                        lastAuthor = author;
                }


                // Write the transcript to a file
                string fileName = $"{Path.GetTempPath()}{_pseudonym}.html";
                Console.WriteLine($"Transcribed {i} lines to {fileName}. Opening...");

                File.WriteAllText(fileName, html.GetHtml());
                Process.Start(@"cmd.exe ", @"/c " + fileName);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e + "\n\nRestarting...\n");
                Console.ResetColor();

                return true;
            }

            return true;
        }

        private LineAuthor GetLineAuthor(string line)
        {
            if (new Regex($@"^{Regex.Escape(_interviewer)} — .+(?:\d\d) ?(?:(?:P|A)M)?$").IsMatch(line))
                return LineAuthor.Interviewer;
            if (new Regex($@"^{Regex.Escape(_interviewee)} — .+(?:\d\d) ?(?:(?:P|A)M)?$").IsMatch(line))
                return LineAuthor.Interviewee;

            return LineAuthor.Unknown;
        }
    }
}