namespace API.Entities
{
    public class AppUser
    {
        public int ID { get; set; }

        public string UserName { get; set; }
        public byte[] Passwordhash { get; set; }
        public byte[] Passwordsalt { get; set; }
    }
}