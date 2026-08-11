namespace LowTideEnt.Application.Authorization.Dto
{
    public class GoogleTokenInfo
    {
        public string Sub {  get; set; }
        public string Email {  get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Aud { get; set; }
    }
}
