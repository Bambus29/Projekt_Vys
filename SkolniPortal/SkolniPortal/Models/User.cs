namespace SkolniPortal.Models
{
    public class User
    {
        public int Id { get; set; } //automaticky
        public string Name { get; set; } //login(bez čísel)
        public string Password { get; set; }
        public string Kasta { get; set; }//podle čísla (učitel speciální číslo)
        public string Trida { get; set; }//seznam ze kterého se vybírá

    }
}
