namespace API.Helpers
{
    public class UserParms : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public string OrederBy { get; set; } = "lastActive";
   
    }
}