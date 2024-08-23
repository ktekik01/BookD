namespace APIBookD.Models.Entities.User.UserDTOs
{
    public class AdminDTO
    {

                public string Name { get; set; }

        //surname
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        // admin role like super admin, admin
        public string AdminRole { get; set; }
    }
}
