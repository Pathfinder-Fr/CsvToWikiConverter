using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Outil de conversion des tableaux google au format WIKI");
            Console.WriteLine("======================================================");
            Console.WriteLine(" ");

            var fileName = args.FirstOrDefault() ?? string.Empty;
            switch (fileName.ToLowerInvariant())
            {
                case "armures":
                    try
                    {
                        new Armures().Run();
                    }
                    catch (NotSupportedException nse)
                    {
                        Console.WriteLine($"ERREUR : {nse.Message}");
                    }
                    break;

                case "armes":
                    try
                    {
                        new Armes().Run();
                    }
                    catch (NotSupportedException nse)
                    {
                        Console.WriteLine($"ERREUR : {nse.Message}");
                    }
                    break;

                case "":
                    Console.WriteLine("ERREUR : Vous devez indiquer un fichier a convertir.");
                    ShowHelp();
                    break;

                default:
                    Console.WriteLine($"ERREUR : Fichier non supporte : {fileName}.");
                    ShowHelp();
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Liste des tableaux supportés :");
            Console.WriteLine("- Armures : fichier des armures");
        }
    }

    public interface ILog
    {
        void WriteLine(string message);
    }

    public class ConsoleLog : ILog
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }

    public static class Log
    {
        public static void WriteLine(this ILog @this, string format, params object[] args)
        {
            @this.WriteLine(string.Format(format, args));
        }
    }

    public interface IOut
    {
        void Write(string message);

        void Close();
    }

    public class FileOut : IOut
    {
        private readonly StreamWriter writer;

        private readonly Stream fileStream;

        public FileOut(string fileName)
        {
            this.fileStream = new FileStream(fileName, FileMode.Create);
            this.writer = new StreamWriter(this.fileStream);
        }

        public void Write(string message)
        {
            this.writer.Write(message);
        }

        public void Close()
        {
            this.writer.Dispose();
            this.fileStream.Dispose();
        }
    }

    public static class Out
    {
        public static void WriteLine(this IOut @this, string message)
        {
            @this.Write(message);
            @this.WriteLine();
        }

        public static void WriteLine(this IOut @this)
        {
            @this.Write(Environment.NewLine);
        }
    }
}