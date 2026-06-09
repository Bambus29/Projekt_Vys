namespace SkolniPortal.Models
{
    public class Zasedak
    {
        public int Id { get; set; }
        public int pocetMist = 30;
        public string forTrida { get; set; }

        public List<string> Mista { get; set; } = new List<string>();
    }
}
 