using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            var fileName = args.FirstOrDefault() ?? string.Empty;            
            switch (fileName)
            {
                case "Armures":
                    new Armures().Run();
                    break;
                    
                case "":
                    Console.WriteLine("Vous devez indiquer un fichier à traduire");
                    break;
                    
                default:
                    Console.WriteLine($"Fichier non supporté : {fileName}");
                    Console.WriteLine("Liste des fichiers supportés :");
                    Console.WriteLine("- Armures : Fichier des armures");
                    break;
            }            
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
            this.fileStream = File.OpenWrite(fileName);
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