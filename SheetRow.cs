namespace ConsoleApplication
{
    public abstract class SheetRow
    {
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