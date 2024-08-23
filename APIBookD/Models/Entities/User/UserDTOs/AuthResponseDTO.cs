namespace APIBookD.Models.Entities.User.UserDTOs
{
    public class AuthResponseDTO
    {

        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
