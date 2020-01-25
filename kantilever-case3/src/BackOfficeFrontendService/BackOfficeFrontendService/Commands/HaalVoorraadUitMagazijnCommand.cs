namespace BackOfficeFrontendService.Commands
{
    /// <summary>
    /// Special command for existing microservices
    /// </summary>
    public class HaalVoorraadUitMagazijnCommand
    {
        public long Artikelnummer { get; set; }
        public int Aantal { get; set; }
    }
}
