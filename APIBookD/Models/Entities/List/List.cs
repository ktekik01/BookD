namespace APIBookD.Models.Entities.List
{
    public class List
    {

        // list id
        public Guid Id { get; set; }

        // user id
        public Guid UserId { get; set; }

        // list name
        public string? Name { get; set; }

        // list description
        public string? Description { get; set; }

        // list type
        public string Type { get; set; }  // wishlist or reading list

    }
}
