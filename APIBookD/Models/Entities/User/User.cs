namespace APIBookD.Models.Entities.User
{
    public class User
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        //surname
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        // user type
        public string UserType { get; set; }

    





    }
}
