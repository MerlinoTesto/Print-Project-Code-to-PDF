using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Print_Code_To_PDF
{
    public class Program
    {
        public static string ProductionGUI_Directory = @"C:\Users\patri\Documents\GitHub\Merlin-Test-Studio";
        public static string destinationFolderPath = @"C:\Users\patri\Documents\GitHub\Print-Project-Code-to-PDF\DATA";
        public static string outputFilePath = "directory_tree.txt";

        static void Main(string[] args)
        {
            Console.WriteLine($"Searching the {ProductionGUI_Directory} for .cs files");
            string[] targetExtensions = { ".cs", ".xaml", ".xaml.cs" }; // Add or remove extensions as needed
            List<string> matchingFilePaths = GetFilesWithExtensions(ProductionGUI_Directory, targetExtensions);

            if (matchingFilePaths.Any())
            {
                string pdfFilePath = Path.Combine(destinationFolderPath, "CodeFiles.pdf");
                ExportToPdf(matchingFilePaths, pdfFilePath);
                Console.WriteLine($"PDF file exported to: {pdfFilePath}");
            }
            else
            {
                Console.WriteLine("No .cs files found.");
            }

            int numberOfFiles = matchingFilePaths.Count;
            Console.WriteLine($"\nCode Files Discovered: {numberOfFiles}");

            
            //Create Dir file tree //MODIFY to write to string builder, then file write stream to file.
            ///TraverseDirectory(ProductionGUI_Directory, includeFiles: true, recursive: true, sortDirectories: true);

            Console.ReadLine(); // Stop the program and wait for key press.
        }

        static List<string> GetFilesWithExtensions(string rootFolderPath, string[] extensions)
        {
            List<string> matchingFilePaths = new List<string>();

            try
            {
                foreach (string extension in extensions)
                {
                    string searchPattern = "*" + extension;
                    Console.WriteLine($"Searching for: {searchPattern}");

                    foreach (string filePath in Directory.EnumerateFiles(rootFolderPath, searchPattern, SearchOption.AllDirectories))
                    {
                        if (!filePath.EndsWith(".g.i.cs", StringComparison.OrdinalIgnoreCase) && !filePath.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase))
                        {
                            matchingFilePaths.Add(filePath);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error accessing folder: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Folder not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return matchingFilePaths;
        }

        static void ExportToPdf(List<string> filePaths, string pdfFilePath)
        {
            using (PdfWriter writer = new PdfWriter(pdfFilePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        string fileContent = File.ReadAllText(filePath);

                        document.Add(new Paragraph(fileName));
                        document.Add(new Paragraph(fileContent));
                    }
                }
            }
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
