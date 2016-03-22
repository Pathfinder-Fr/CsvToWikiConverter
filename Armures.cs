using System;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    internal class Armures
    {
        private readonly ILog log;
        
        private readonly IOut @out;
        
        public Armures(ILog log = null)
        {
            this.log = log ?? new ConsoleLog();
            this.@out = new FileOut(@"Armures.txt");
        }
        
        public void Run()
        {            
            var fileName = @"Armures.tsv";
            if (!File.Exists(fileName))
            {
                log.WriteLine($"Fichier {fileName} introuvable");
                return;
            }          
            
            Row previousRow = null;
            var altRow = false;
            var count = 0;
            
            log.WriteLine("Début de la lecture du fichier wiki...");
            
            foreach (var line in File.ReadAllLines(fileName).Skip(1))
            {
                // lecture ligne
                var row = Row.FromCells(line.Split('\t'));
                
                // conversion format wiki
                this.ConvertRow(row, previousRow, altRow);
                
                // passage ligne suivante
                previousRow = row;
                altRow = !altRow;
                count++;
            }
            
            @out.Close();
            log.WriteLine($"Conversion terminée, {count} lignes écrites dans le fichier Armures.txt");
        }
        
        private void ConvertRow(Row row, Row previousRow, bool altRow)
        {
            // si le type d'armure change, alors on écrit le séparateur de types
            if (previousRow?.Type != row.Type)
            {
                // changement de type
                altRow = false;
                @out.WriteLine("|- CLASS=\"premier\"");
                switch (row.Type)
                {
                    case "Légère":
                        @out.WriteLine("| COLSPAN=\"9\" | {s:Reference|LEGERES}'''ARMURES LÉGÈRES'''");
                        break;
                        
                    case "Intermédiaire":
                        @out.WriteLine("| COLSPAN=\"9\" | {s:Reference|INTERMEDIAIRES}'''ARMURES INTERMÉDIAIRES'''");
                        break;
                    
                    case "Lourde":
                        @out.WriteLine("| COLSPAN=\"9\" | {s:Reference|LOURDES}'''ARMURES LOURDES'''");
                        break;
                    
                    case "Bouclier":
                        @out.WriteLine("| COLSPAN=\"9\" | {s:Reference|BOUCLIER}'''BOUCLIERS'''");
                        break;
                    
                    case "Suppléments":
                        @out.WriteLine("| COLSPAN=\"9\" | '''SUPPLÉMENTS'''");
                        break;
                }
            }
            
            // séparateur de lignes
            if (altRow)
            {
                @out.WriteLine("|- CLASS=\"alt\"");
            }
            else
            {
                @out.WriteLine("|-");
            }
            
            // lien wiki + nom
            @out.Write("| &emsp;[[");
            @out.Write(this.ReadWikiLink(row.WikiLink));
            @out.Write("|");
            @out.Write(row.Name);
            @out.Write("]] || ");
            
            // prix
            @out.Write(row.Price);
            @out.Write(" || ");
            
            // bonus
            @out.Write(row.Bonus);
            @out.Write(" || ");
            
            // bonus dex max
            @out.Write(row.DexMax);
            @out.Write(" || ");
            
            // malus armure
            @out.Write(row.Malus);
            @out.Write(" || ");
            
            // risque échec sort
            @out.Write(row.SpellMiss);
            @out.Write(" || ");
            
            // vitesse 9m
            @out.Write(this.ReadSpeed(row.Speed9));
            @out.Write(" || ");
            
            // vitesse 6m
            @out.Write(this.ReadSpeed(row.Speed6));
            @out.Write(" || ");
            
            // poids
            @out.Write(row.Weight);
            
            // passage ligne suivante
            @out.WriteLine();
        }
        
        private string ReadWikiLink(string wikiLink)
        {
            if (wikiLink.StartsWith("#"))
            {
               // cas d'un lien commençant par un #, il s'agit d'une version "courte" qui doit pointer vers la page "Description individuelle des armures", on l'ajoute
               return "Descriptions individuelles des armures" + wikiLink;
            }
            
            if (wikiLink.StartsWith("orientales#", StringComparison.OrdinalIgnoreCase))
            {
                // cas d'un lien commençant par "orientales#", il s'agit d'un raccourci vers la page "Description des armures orientales", on l'ajoute
                return "Description des armures orientales" + wikiLink.Substring(11);
            }

            // sinon on renvoie le lien tel quel
            return wikiLink;
        }
        
        private string ReadSpeed(string speed)
        {
            switch (speed)
            {
                case "9 m (6 c)": return "9 m (6 {s:c})";
                case "6 m (4 c)": return "6 m (4 {s:c})";                    
                case "4,50 m (3 c)": return "4,50 m (3 {s:c})";
                case "6 m (4 c)3": return "6 m (4 {s:c})<sup>[[Tableau récapitulatif des armures#NOTE3|3]]</sup>";
                case "4,50 m (3 c)3": return "4,50 m (3 {s:c})<sup>[[Tableau récapitulatif des armures#NOTE3|3]]</sup>";
                case "3 m (2 c)3": return "3 m (2 {s:c})<sup>[[Tableau récapitulatif des armures#NOTE3|3]]</sup>";
                case "—": return "—";
                default: throw new NotSupportedException($"Vitesse de déplacement non reconnue : {speed}");                
            }
        }
        
        class Row
        {            
            public string Name { get; set; }
            public string WikiLink { get; set; }
            public string Type { get; set; }
            public string Price { get; set; }
            public string Bonus { get; set; }
            public string DexMax { get; set; }
            public string Malus { get; set; }
            public string SpellMiss { get; set; }            
            public string Speed9 { get; set; }
            public string Speed6 { get; set; }
            public string Weight { get; set; }
            
            public static Row FromCells(string[] cells)
            {
                var i = 0;
                return new Row
                {
                  Name = cells[i++],
                  WikiLink = cells[i++],
                  Type = cells[i++],
                  Price = cells[i++],
                  Bonus = cells[i++],
                  DexMax = cells[i++],
                  Malus = cells[i++],
                  SpellMiss = cells[i++],
                  Speed9 = cells[i++],
                  Speed6 = cells[i++],
                  Weight = cells[i++]                    
                };
            }
        }
    }
}