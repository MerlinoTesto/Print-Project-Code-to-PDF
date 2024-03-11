using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Print_Code_To_PDF
{
    public class Program
    {
        public static string ProductionGUI_Directory = @"C:\Users\Patrick\Documents\GitHub\ProductionGUI";
        public static string Data_Directory = @"C:\Users\Patrick\Documents\GitHub\Print-Project-Code-to-PDF\DATA";
        public static string outputFilePath = "directory_tree.txt";

        static void Main(string[] args)
        {
            Console.WriteLine($"Searching the {ProductionGUI_Directory} for .cs files");
            string[] targetExtensions = { ".cs", ".xaml" }; // Add or remove extensions as needed
            List<string> matchingFilePaths = GetFilesWithExtensions(ProductionGUI_Directory, targetExtensions);

            // Output the results
            foreach (string filePath in matchingFilePaths)
            {
                Console.WriteLine(filePath);
            }

            int numberOfFiles = 0;
            Console.WriteLine($"\nCode Files Discovered: {numberOfFiles}");

            
            //Create Dir file tree //MODIFY to write to string builder, then file write stream to file.
            ///TraverseDirectory(ProductionGUI_Directory, includeFiles: true, recursive: true, sortDirectories: true);

            Console.ReadLine(); // Stop the program and wait for key press.
        }

        static List<string> GetFilesWithExtensions(string rootFolderPath, string[] extensions)
        {
            List<string> matchingFilePaths = new List<string>();

            foreach (string extension in extensions)
            {
                string searchPattern = Path.Combine(rootFolderPath, "*" + extension);
                foreach (string filePath in Directory.GetFiles(rootFolderPath, extension, SearchOption.AllDirectories))
                {
                    matchingFilePaths.Add(filePath);
                }
            }

            return matchingFilePaths;
        }


        #region Directory Tree Creation Methods
        private static string GetIndent(int level)
        {
            return new string(' ', level * 4); // Adjust indentation width here (currently 4 spaces)
        }

        public static void TraverseDirectory(string directoryPath, bool includeFiles = true, bool recursive = true, bool sortDirectories = false)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Error: Directory '{directoryPath}' does not exist.");
                return;
            }

            var directoryInfo = new DirectoryInfo(directoryPath);

            // Traverse directories first, then files (if included) with optional sorting
            if (recursive)
            {
                // Increased indentation for subdirectories
                var indent = GetIndent(1);  // Adjust level 1 indentation
                foreach (var childDirectory in directoryInfo.GetDirectories())
                {
                    Console.WriteLine($"{indent}{childDirectory.Name}");
                    TraverseDirectory(childDirectory.FullName, includeFiles, recursive, sortDirectories);
                }
            }

            if (includeFiles)
            {
                if (sortDirectories)
                {
                    // Sort files based on name for a consistent order
                    var files = directoryInfo.GetFiles().OrderBy(f => f.Name).ToArray();
                    foreach (var file in files)
                    {
                        var indent = GetIndent(2);  // Adjust level 2 indentation for files
                        Console.WriteLine($"{indent}{file.Name}");
                    }
                }
                else
                {
                    foreach (var file in directoryInfo.GetFiles())
                    {
                        var indent = GetIndent(1);  // Adjust level 1 indentation for files (if no sorting)
                        Console.WriteLine($"{indent}{file.Name}");
                    }
                }
            }
        }
        #endregion
    }


    public class Object
    {

        public int ID;
        public string name;
        public int ParentID;

        public Object(int ID, string name, int ParentID)
        {
            this.ID = ID;
            this.name = name;
            this.ParentID = ParentID;
        }
    }
}
