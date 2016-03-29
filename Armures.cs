using System;

namespace ConsoleApplication
{
    internal class Armures : SheetReader<ArmuresRow>
    {
        public Armures(ILog log = null)
            : base("armures.txt", log)
        {
        }
        
        protected override ArmuresRow FromLine(string line)
        {
            var cells = ReadCsvCells(line, 14);
            var i = 0;
            return new ArmuresRow
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
                Weight = cells[i++],
                Special = cells[i++],
                Source = cells[i++],
                Note = cells[i++],
            };            
        }
        
        protected override void ReadRow(ArmuresRow row, ArmuresRow previousRow, bool altRow, IOut @out)
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
            @out.Write("]]");
            @out.Write(this.ReadNameNote(row.Note));
            @out.Write(" || ");
            
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
            @out.Write(this.ReadSpellMissNote(row.Note));
            @out.Write(" || ");
            
            // vitesse 9m
            @out.Write(this.ReadSpeed(row.Speed9));
            @out.Write(this.ReadSpeedNote(row.Note));
            @out.Write(" || ");
            
            // vitesse 6m
            @out.Write(this.ReadSpeed(row.Speed6));
            @out.Write(this.ReadSpeedNote(row.Note));
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
                case "3 m (2 c)": return "3 m (2 {s:c})";
                case "—": return "—";
                default: throw new NotSupportedException($"Vitesse de deplacement non reconnue : {speed}");                
            }
        }
        
        private string ReadNameNote(string note)
        {
            switch (note)
            {
                case "4": return "<sup>[[#NOTE4|4]]</sup>";
                default: return null;
            }
        }
        
        private string ReadSpellMissNote(string note)
        {
            switch (note)
            {
                case "5": return "([[#NOTE5|5]])";
                default: return null;
            }
        }
        
        private string ReadSpeedNote(string note)
        {
            switch (note)
            {
                case "2": return "<sup>[[#NOTE2|2]]</sup>";
                case "3": return "<sup>[[#NOTE3|3]]</sup>";
                default: return null;
            }
        }
    }
        
    public class ArmuresRow : SheetRow
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
        public string Special { get; set; }
        public string Source { get; set; }
        public string Note { get; set; }
    }
}