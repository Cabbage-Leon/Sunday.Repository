namespace Sunday.Repository.Api.Data
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
    }
}