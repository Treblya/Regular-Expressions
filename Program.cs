using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;


namespace Logs
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcefilename = "";
            string dir = "";
            int n = 0;
            Directory.CreateDirectory(@"C:\\Users\\" + Environment.UserName + "\\Desktop\\Result\\"); //директория с результатами
            string writepath = @"C:\\Users\\" + Environment.UserName + "\\Desktop\\Result\\";
            string readpath = @"C:\\Users\\" + Environment.UserName + "\\Desktop\\Logs\\" + sourcefilename + ".txt";
start:      Console.WriteLine(("").PadRight(119, '-'));
            Console.WriteLine("{0,79}","......::::::Welcome::::::......");
            Console.WriteLine("{0,92}", "This program is designed to search logs for a given mask.");
            Console.WriteLine("{0,82}", "Thank you for using our software. <3");
            Console.WriteLine("{0,80}", "Please select one of the options.");
            Console.WriteLine(("").PadRight(119, '-'));
            Console.WriteLine("1. Search by specified mask.");
            Console.WriteLine("2. Recursive log search method.");
            if (sourcefilename != "")
            Console.WriteLine("3. Change source file. (" + sourcefilename + ".txt)");
            else
            Console.WriteLine("3. Change source file. (Source file not specified!)");
            if (dir != "")
            Console.WriteLine("4. Change destination directory name. (..\\Result\\" + dir + ")");
            else
            Console.WriteLine("4. Change destination directory name. (..\\Result)");
            if (n == 0)
            Console.WriteLine("5. Change search depth. (Search depth not specified!)");
            else
            Console.WriteLine("5. Change search depth. (Search depth - " + n + ")");
            Console.WriteLine("Press 'Escape' to exit.");
            ConsoleKeyInfo key;
            key = Console.ReadKey();
            Console.Clear();
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.D1:
                    if (sourcefilename != "" && n != 0)
                    {
                        SpecifiedMask(n, writepath, readpath, sourcefilename);
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                        MessageBox((IntPtr)0, "'Source file name' or 'Search depth' not set", "Variable is not specified", 0);  
                    goto start;
                case ConsoleKey.D2:
                    if (sourcefilename != "" && n != 0)
                    {
                        RecursiveMethod(n, writepath, readpath, sourcefilename);
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                        MessageBox((IntPtr)0, "'Source file name' or 'Search depth' not set", "Variable is not specified", 0);
                    goto start;
                case ConsoleKey.D3:
                    if (Directory.Exists(@"C:\\Users\\" + Environment.UserName + "\\Desktop\\Logs\\"))
                        ChangeSource(ref sourcefilename, ref readpath);
                    else
                    {
                        Directory.CreateDirectory(@"C:\\Users\\" + Environment.UserName + "\\Desktop\\Logs\\");
                        MessageBox((IntPtr)0, "Add source files to folder 'Logs' on Desktop", "Directory does not exist", 0);
                    }
                    goto start;
                case ConsoleKey.D4:
                    ChangeDirectory(ref dir, ref writepath);
                    goto start;
                case ConsoleKey.D5:
                    ChangeSearchDepth(ref n);
                    goto start;
                default:
                    goto start;

            }
           
            


        }
        public static void ChangeSource(ref string sourcefilename, ref string readpath)
        {
            Console.WriteLine("Enter source file name:"); //считываемый файл
            Console.WriteLine(("").PadRight(24, '-'));
            Directory
        .GetFiles(@"C:\\Users\\" + Environment.UserName + "\\Desktop\\Logs\\", "*", SearchOption.AllDirectories)
        .ToList()
        .ForEach(f => Console.WriteLine(Path.GetFileName(f)));
            Console.WriteLine(("").PadRight(24, '-'));
            sourcefilename = Console.ReadLine();
            if (File.Exists(@"C:\\Users\\" + Environment.UserName + "\\Desktop\\Logs\\" + sourcefilename + ".txt"))
                readpath = @"C:\\Users\\" + Environment.UserName + "\\Desktop\\Logs\\" + sourcefilename + ".txt";
            else
            {
                MessageBox((IntPtr)0, "Add source files to folder 'Logs' on Desktop", "File does not exists", 0);
                sourcefilename = "";
            }
            Console.Clear();
        }
        public static void ChangeDirectory(ref string dir, ref string writepath)
        {
            Console.WriteLine("Enter destination directory name. Empty - do not create."); //внутренняя директория
            dir = Console.ReadLine();
            if (dir != "")
            {
                Directory.CreateDirectory(@"C:\\Users\\Treblya\\Desktop\\Result\\" + dir);
                writepath += dir + "\\";
            }
            Console.Clear();
        }
        public static void ChangeSearchDepth(ref int n)
        {
            Console.WriteLine("Enter search depth"); //глубина поиска
            n = int.Parse(Console.ReadLine());
            Console.Clear();
        }

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        public static void SpecifiedMask(int n, string writepath, string readpath, string sourcefilename)
        {
                
                string filename = "logout_" + sourcefilename + "_d" + n;
                writepath = writepath + filename + ".txt";
                StreamReader rd = new StreamReader(readpath, Encoding.Default); //поток для чтения
                StreamWriter wr = new StreamWriter(writepath, false, Encoding.Default); //поток для записи
                string str;
                string[] words;
                string equal, secondequal;
                List<string> text = new List<string>();
                while (rd.EndOfStream != true) //содержимое файла в List
                    text.Add(rd.ReadLine());
                for (int i = 0; i < text.Count; i++)
                {
                    str = text[i];
                    words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    equal = CutEqual(words[2], n);
                    if (equal != "Does not match the search mask")
                        wr.WriteLine(words[0] + words[2]);
                text.RemoveAt(i);
                    for (int j = 0; j < text.Count; j++)
                    {
                        str = text[j];
                        words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        secondequal = CutSecondEqual(words[2], n);
                    if (secondequal == equal)
                    {
                        Console.WriteLine(equal + " == " + secondequal + " ? - True");
                        wr.WriteLine(words[0] + words[2]);
                        text.RemoveAt(j);
                        j--;

                    }
                    else 
                        Console.WriteLine(equal + " == " + secondequal + " ? - False");     
                }
                    i = 0;
                }
                wr.Close();

            
        }
        public static void RecursiveMethod(int n, string writepath, string readpath, string sourcefilename)
        {
            string secondwritepath = writepath;
            for (int rec = 1; rec < n + 1; rec++)
            {
                Console.WriteLine("Depth " + rec + " - Please, stand by.");
                string filename = "logout_" + sourcefilename + "_d" + rec;
                writepath = secondwritepath + filename + ".txt";
                StreamReader rd = new StreamReader(readpath, Encoding.Default); //поток для чтения
                StreamWriter wr = new StreamWriter(writepath, false, Encoding.Default); //поток для записи
                string str;
                string[] words;
                string equal, secondequal;
                List<string> text = new List<string>();
                while (rd.EndOfStream != true) //содержимое файла в List
                    text.Add(rd.ReadLine());
                for (int i = 0; i < text.Count; i++)
                {
                    str = text[i];
                    words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    equal = CutEqual(words[2], rec);
                    if (equal != "empty")
                        wr.WriteLine(words[0] + words[2]);
                    text.RemoveAt(i);
                    for (int j = 0; j < text.Count; j++)
                    {
                        str = text[j];
                        words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        secondequal = CutSecondEqual(words[2], rec);
                        if (secondequal == equal)
                        {
                            Console.WriteLine(equal + " == " + secondequal + " ? - True");
                            wr.WriteLine(words[0] + words[2]);
                            text.RemoveAt(j);
                            j--;
                        }
                        else
                            Console.WriteLine(equal + " == " + secondequal + " ? - False");
                    }
                    i = 0;
                }
                wr.Close();
                Console.WriteLine("Depth " + rec + " - Complete!");
            }
        }

        public static string CutEqual(string equal, int n) //то, что будут сравнивать
        {
            string NewEqual;
            NewEqual = equal.Remove(0, 1);
            if (n == 1) //1 slash
            {
                if (NewEqual.IndexOf('/') >= 0)
                    NewEqual = NewEqual.Remove(NewEqual.IndexOf('/'), NewEqual.Length - NewEqual.IndexOf('/'));
                return NewEqual;
            }
            int k = 1;
            string temp = "";
            string FakeNewEqual = NewEqual;
            while (FakeNewEqual.IndexOf('/') > 0) // n slashes
            {
                k++;
                FakeNewEqual = FakeNewEqual.Remove(FakeNewEqual.IndexOf('/'), 1);
            }
            if (k > n)
            {
                for (int j = k - n; j < k; j++)
                {
                    temp += NewEqual.Substring(0, NewEqual.IndexOf('/') + 1);
                    NewEqual = NewEqual.Remove(0, NewEqual.IndexOf('/') + 1);
                }
                temp = temp.Remove(temp.Length - 1, 1);
                return temp;
            }
            if (k == 1 || k < n)
                return "Does not match the search mask";
            return NewEqual;

        }
        public static string CutSecondEqual(string equal, int n) //то, с чем будут сравнивать
        {
            string NewEqual;
            NewEqual = equal.Remove(0, 1);
            if (n == 1) //1 slash
            {
                if (NewEqual.IndexOf('/') >= 0)
                    NewEqual = NewEqual.Remove(NewEqual.IndexOf('/'), NewEqual.Length - NewEqual.IndexOf('/'));
                return NewEqual;
            }
            int k = 1;
            string temp = "";
            string FakeNewEqual = NewEqual;
            while (FakeNewEqual.IndexOf('/') > 0) // n slashes
            {
                k++;
                FakeNewEqual = FakeNewEqual.Remove(FakeNewEqual.IndexOf('/'), 1);
            }
            if (k > n)
            {
                for (int j = k - n; j < k; j++)
                {
                    temp += NewEqual.Substring(0, NewEqual.IndexOf('/') + 1);
                    NewEqual = NewEqual.Remove(0, NewEqual.IndexOf('/') + 1);
                }
                temp = temp.Remove(temp.Length - 1, 1);
                return temp;
            }
            if (k == 1 || k < n)
                return "Does not match the search mask.";
            return NewEqual;

        }
    }
}
