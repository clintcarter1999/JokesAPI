﻿using System.ComponentModel.DataAnnotations;

namespace JokesAPI.Models
{
    public class JokeItem
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Joke { get; set; }

        //TODO: Add other attributes such as Category, Author, DateAdded, DateModified, Language, Rating, etc
        //Thinking about adding an Author Table and associating jokes to either 'anonymous' or defined authors/users

    }
}
