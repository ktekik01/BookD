﻿namespace APIBookD.Models.Entities.Review.ReviewDTOs
{
    public class ReviewDTO
    {


        // review title
        public string Title { get; set; }

        // user id
        public Guid UserId { get; set; }

        // book id
        public string BookTitle { get; set; }

        // review text

        public string ReviewText { get; set; }

    }
}
