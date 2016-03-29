using System;
using System.Linq;
using System.Net.Http;

namespace ConsoleApplication
{
    public abstract class SheetReader<T> where T : class
    {
        protected readonly ILog log;
        
        private readonly string fileName;
        private readonly IOut @out;
        
        protected SheetReader(string fileName, ILog log = null)
        {
            this.fileName = fileName;
            this.log = log ?? new ConsoleLog();
            this.@out = new FileOut(fileName);
        }
        
        /// <summary>
        /// Obtient l'identifiant de l'onglet du fichier google spreadsheet à utiliser.
        /// </summary>
        protected abstract string SheetId { get; } 
        
        /// <summary>
        /// A partir d'une ligne du fichier CSV, renvoie la réprésentation objet.
        /// </summary>
        protected abstract T FromLine(string line);
        
        protected abstract void ReadRow(T row, T previousRow, ref bool altRow, IOut @out);
        
        public void Run()
        {
            log.WriteLine($"Conversion du tableau {fileName} au format wiki");
            
            log.WriteLine("Téléchargement du contenu");
            var content = this.Download(this.SheetId);
            
            var lines = content.Split(new [] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);            
            
            T previousRow = null;
            var altRow = false;
            var count = 0;
            
            log.WriteLine("Demarrage de la conversion des lignes...");            
            foreach (var line in lines.Skip(1))
            {
                // lecture ligne
                var row = this.FromLine(line);
                
                // conversion format wiki
                this.ReadRow(row, previousRow, ref altRow, @out);
                
                // passage ligne suivante
                previousRow = row;
                altRow = !altRow;
                count++;
            }
            
            @out.Close();
            log.WriteLine($"Conversion terminee, {count} lignes ecrites dans le fichier {fileName}");
        }
        
        protected string Download(string sheetId)
        {
            var client = new HttpClient();            
            return client.GetStringAsync($"https://docs.google.com/spreadsheets/d/1MZ5Nz424T1CRSNi00Ky7jG-TrcKZeCYgqoClRjTfaXQ/export?exportFormat=csv&gid={sheetId}").Result;
        }        
        
        protected static string[] ReadCsvCells(string line, int cellCount)
        {
            var cells = new string[cellCount];
            var quoted = false;
            var start = true;
            var cellIndex = 0;
            var cellStart = 0;
            var cellEnd = 0;
            
            for (int i = 0; i <= line.Length; i++)
            {
                var c = (i < line.Length) ? line[i] : '\n';
                var c1 = (i < line.Length - 1) ? line[i+1] : '\n';
                
                if (start && c == '"')
                {
                    // début guillemets
                    // => on marque le démarrage
                    quoted = true;
                    continue;
                }
                
                if (quoted && c == '"' && c1 == '"')
                {
                    // guillemets à l'intérieur de guillemets
                    // => on les évite
                    i++;
                    continue;
                }
                
                if (quoted && c == '"')
                {
                    // fin des guillemets
                    // => on marque la sortie
                    quoted = false;
                    continue;
                }
                
                if (c == '\n')
                {
                    // fin de ligne
                    var cellContent = line.Substring(cellStart, cellEnd - cellStart + 1).Replace("\"\"", "\"");
                    cells[cellIndex] = cellContent;
                    cellIndex++;
                    continue;
                }
                
                if (start && c == ',')
                {
                    // séparateur sans contenu
                    cells[cellIndex] = string.Empty;
                    cellIndex++;
                    continue;                        
                }
                
                if (!quoted && c == ',')
                {
                    // séparateur
                    var cellContent = line.Substring(cellStart, cellEnd - cellStart + 1).Replace("\"\"", "\"");
                    cells[cellIndex] = cellContent;
                    cellIndex++;
                    start = true;
                    continue;
                }
                    
                if (start)
                {
                    // début du mot
                    cellStart = i;
                    cellEnd = i;
                    start = false;
                    continue;
                }
                
                cellEnd = i;
            }
            
            return cells;
        }
    }
}