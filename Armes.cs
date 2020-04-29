using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    internal class Armes : SheetReader<ArmesRow>
    {
        public Armes(ILog log = null)
            : base("armes.txt", log)
        {
        }

        protected override string SheetId { get { return "1772862108"; } }

        protected override ArmesRow FromLine(string line)
        {
            var cells = ReadCsvCells(line, 18);
            var i = 0;
            return new ArmesRow
            {
                Name = cells[i++],
                Price = cells[i++],
                DamageSmall = cells[i++],
                DamageMedium = cells[i++],
                Critical = cells[i++],
                Range = cells[i++],
                Weight = cells[i++],
                Type = cells[i++],
                Special = cells[i++],
                Kind1 = cells[i++],
                Kind2 = cells[i++],
                Kind3 = cells[i++],
                SortKind1 = cells[i++],
                SortKind2 = cells[i++],
                Link = cells[i++],
                Source = cells[i++],
                Modifier = cells[i++],
                Note = cells[i++],
            };
        }

        protected override void ReadRow(ArmesRow row, ArmesRow previousRow, ref bool altRow, IOut @out)
        {
            // si le type d'arme change, alors on change de tableau
            if (previousRow?.SortKind1 != row.SortKind1)
            {
                string title;
                string titleRef;
                string tableHeadTitle;
                string tableRowTitle;
                string tableRowLink;

                switch (row.SortKind1)
                {
                    case "1-courante":
                        title = "Les armes courantes";
                        titleRef = "TABCOURANTES";
                        tableHeadTitle = "Tableau récapitulatif des armes courantes";
                        tableRowTitle = "Armes courantes";
                        tableRowLink = "Caractéristiques des armes#ARMECOURANTE";
                        break;

                    case "2-de guerre":
                        title = "Les armes de guerre";
                        titleRef = "TABGUERRE";
                        tableHeadTitle = "Tableau récapitulatif des armes de guerre";
                        tableRowTitle = "Armes de guerre";
                        tableRowLink = "Caractéristiques des armes#ARMEDEGUERRE";

                        break;

                    case "3-exotique":
                        title = "Les armes exotiques";
                        titleRef = "TABEXOTIQUES";
                        tableHeadTitle = "Tableau récapitulatif des armes exotiques";
                        tableRowTitle = "Armes exotiques";
                        tableRowLink = "Caractéristiques des armes#ARMEEXOTIQUE";
                        break;

                    default:
                        throw new NotSupportedException(row.SortKind1);
                }

                if (previousRow != null)
                {
                    @out.WriteLine("|}");
                    @out.WriteLine("</center>");
                    @out.WriteLine();
                }

                @out.WriteLine($"{{s:Reference|{titleRef}}}");
                @out.WriteLine($"=== {title} ===");
                @out.WriteLine("<center>");
                @out.WriteLine("{| CLASS=\"tablo\" style=\"width:100%\"");
                @out.WriteLine($"|+ {tableHeadTitle}");
                @out.WriteLine("|- CLASS=\"titre\"");
                @out.Write($"| [[{tableRowLink}|{tableRowTitle}]]");
                @out.WriteLine(" || Prix || Dégâts (P) || Dégâts (M) || [[Caractéristiques des armes#Critique_6|Critique]] || [[Caractéristiques des armes#Facteur_de_portée_7|Facteur de portée]] || Poids <sup>([[Tableau récapitulatif des armes#NOTE1|1]])</sup> || [[Caractéristiques des armes#Type_9|Type]] <sup>([[Tableau récapitulatif des armes#NOTE2|2]])</sup> || Spécial");
            }

            if (previousRow?.SortKind1 != row.SortKind1 || previousRow?.SortKind2 != row.SortKind2)
            {
                // changement de type
                altRow = false;
                string title;
                switch (row.SortKind2)
                {
                    case "1-corps à corps mains nues":
                        title = "'''COMBAT À MAINS NUES''' ";
                        break;

                    case "2-corps à corps légère":
                        title = "'''ARMES DE [[Caractéristiques des armes#ARMEDECORPSACORPS|CORPS À CORPS]] [[Caractéristiques des armes#ARMELEGERE|LÉGÈRE]]'''{s:Reference|ARMESCOURANTESLEGERES}";
                        break;

                    case "3-corps à corps à une main":
                        title = "'''ARMES DE [[Caractéristiques des armes#ARMEDECORPSACORPS|CORPS À CORPS]] [[Caractéristiques des armes#ARMEAUNEMAIN|À UNE MAIN]]'''{s:Reference|ARMESCOURANTESUNEMAIN}";
                        break;

                    case "4-corps à corps à deux mains":
                        title = "'''ARMES DE [[Caractéristiques des armes#ARMEDECORPSACORPS|CORPS À CORPS]] [[Caractéristiques des armes#ARMEADEUXMAINS|À DEUX MAINS]]'''{s:Reference|ARMESCOURANTESDEUXMAINS}";
                        break;

                    case "5-à distance":
                        title = "'''ARMES [[Caractéristiques des armes#ARMEADISTANCE|À DISTANCE]]'''{s:Reference|ARMESCOURANTESDISTANCE}";
                        break;

                    default:
                        throw new NotSupportedException(row.SortKind1);
                }

                @out.WriteLine("|- CLASS=\"premier\"");
                @out.WriteLine($"| COLSPAN=\"9\" | {title}");
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

            // Nom
            var nameLink = this.ReadWikiLink(row.Link, row.Name);
            @out.Write("| &emsp;");
            if (row.Special == "orientale")
            {
                @out.Write("''");
            }
            @out.Write("[[");
            @out.Write(nameLink);
            @out.Write("|");
            @out.Write(row.Name);
            @out.Write("]]");
            if (row.Special == "orientale")
            {
                @out.Write("''");
            }

            @out.Write(this.ReadNameNote(row.Note));

            if (row.Modifier?.StartsWith("raciale:") ?? false)
            {
                var race = row.Modifier.Substring("raciale:".Length);
                var raceLink = race;
                race = race.Replace(" (race)", "");
                @out.Write(" ([[");
                @out.Write(raceLink);
                @out.Write("|");
                @out.Write(race);
                @out.Write("]])");
            }

            @out.Write(" || ");

            // prix
            @out.Write(row.Price.Replace(" ", "&nbsp;"));
            @out.Write(" || ");

            // dégâts (P)
            @out.Write(row.DamageSmall);
            @out.Write(" || ");

            // dégâts (M)
            @out.Write(row.DamageMedium);
            @out.Write(" || ");

            // critique
            @out.Write(row.Critical);
            @out.Write(" || ");

            // facteur de portée
            @out.Write(row.Range);
            //@out.Write(this.ReadSpellMissNote(row.Note));
            @out.Write(" || ");

            // poids
            @out.Write(row.Weight);
            //@out.Write(this.ReadSpeedNote(row.Note));
            @out.Write(" || ");

            // type
            @out.Write(row.Type);
            // @out.Write(this.ReadSpeed(row.Speed6));
            // @out.Write(this.ReadSpeedNote(row.Note));
            @out.Write(" || ");

            // spécial
            if (row.Special != "—")
            {
                foreach (var token in Regex.Split(row.Special, @"(,|(\s+ou\s+))"))
                {
                    if (token.Trim() == "," || token.Trim() == "ou")
                    {
                        @out.Write(token);
                    }
                    else if (string.Equals(token.Trim(), "voir texte", StringComparison.OrdinalIgnoreCase))
                    {
                        @out.Write(" [[");
                        @out.Write(nameLink);
                        @out.Write("|");
                        @out.Write("Voir texte");
                        @out.Write("]]");
                    }
                    else
                    {
                        // pas de lien indiqué : on se base sur le nom en appliquant la règle standard :
                        // - suppression des caractères spéciaux et des espaces
                        // - suppression des mots entre parenthèses
                        // - suppression des accents

                        var cleanedName = Regex.Replace(token.Trim(), @"[ '-]", @"");
                        if (cleanedName.IndexOf('(') != -1)
                        {
                            cleanedName = cleanedName.Substring(0, cleanedName.IndexOf('('));
                        }
                        cleanedName = RemoveDiacritics(cleanedName);
                        @out.Write(" [[Caractéristiques des armes#");
                        @out.Write(cleanedName.ToUpperInvariant());
                        @out.Write("|");
                        @out.Write(token.Trim());
                        @out.Write("]]");
                    }
                }
            }
            else
            {
                @out.Write(row.Special);
            }

            // passage ligne suivante
            @out.WriteLine();
        }

        private string ReadWikiLink(string wikiLink, string name)
        {
            if (string.IsNullOrEmpty(wikiLink))
            {
                // pas de lien indiqué : on se base sur le nom en appliquant la règle standard :
                // - suppression des caractères spéciaux et des espaces
                // - suppression des mots entre parenthèses
                var cleanedName = Regex.Replace(name, @"[ '-]", @"");
                if (cleanedName.IndexOf('(') != -1)
                {
                    cleanedName = cleanedName.Substring(0, cleanedName.IndexOf('('));
                }

                return "Descriptions individuelles des armes#" + cleanedName;
            }

            if (wikiLink.StartsWith("#"))
            {
                // cas d'un lien commençant par un #, il s'agit d'une version "courte" qui doit pointer vers la page "Description individuelle des armures", on l'ajoute
                return "Descriptions individuelles des armes" + wikiLink;
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
                //case "4": return "<sup>[[#NOTE4|4]]</sup>";
                default: return null;
            }
        }

        private string ReadSpellMissNote(string note)
        {
            switch (note)
            {
                case "3": return "<sup>([[Tableau récapitulatif des armes#NOTE3|3]])</sup>";
                // case "5": return "([[#NOTE5|5]])";
                default: return null;
            }
        }

        private string ReadSpeedNote(string note)
        {
            switch (note)
            {
                //case "2": return "<sup>[[#NOTE2|2]]</sup>";
                //case "3": return "<sup>[[#NOTE3|3]]</sup>";
                default: return null;
            }
        }
        public static IEnumerable<char> RemoveDiacriticsEnum(string src, bool compatNorm = false, Func<char, char> customFolding = null)
        {
            foreach (char c in src.Normalize(compatNorm ? NormalizationForm.FormKD : NormalizationForm.FormD))
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.EnclosingMark:
                        //do nothing
                        break;
                    default:
                        if (customFolding == null)
                            yield return c;
                        else
                            yield return customFolding(c);
                        break;
                }
        }

        public static string RemoveDiacritics(string src, bool compatNorm = false, Func<char, char> customFolding = null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in RemoveDiacriticsEnum(src, compatNorm, customFolding))
                sb.Append(c);
            return sb.ToString();
        }
    }

    public class ArmesRow
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string DamageSmall { get; set; }
        public string DamageMedium { get; set; }
        public string Critical { get; set; }
        public string Range { get; set; }
        public string Weight { get; set; }
        public string Type { get; set; }
        public string Special { get; set; }
        public string Kind1 { get; set; }
        public string Kind2 { get; set; }
        public string Kind3 { get; set; }
        public string SortKind1 { get; set; }
        public string SortKind2 { get; set; }
        public string Link { get; set; }
        public string Source { get; set; }
        public string Modifier { get; set; }
        public string Note { get; set; }
    }
}