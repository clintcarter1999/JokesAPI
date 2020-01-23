using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Models;

namespace JokesAPI.Middleware
{
    public static class AppDbContextExtensions
    {
        public static void EnsureDatabaseSeeded(this JokesAPI.Models.JokesContext context)
        {
            if (context != null)
            {
                if (!context.JokeItems.Any())
                {
                    context.AddRange(new JokeItem[]
                        {
                            new JokeItem() { Id = 1,  Joke = "What do you call a nose without a body? NOBODY Knows!" },
                            new JokeItem() { Id = 2,  Joke = "How do you catch a unique rabbit? UNIQUE up on him! How do you catch a tame rabbit? TAME way, unique up on him!" },
                            new JokeItem() { Id = 3,  Joke = "Did you hear about the mathematician who’s afraid of negative numbers? He’ll stop at nothing to avoid them." },
                            new JokeItem() { Id = 4,  Joke = "Why do we tell actors to “break a leg? Because every play has a cast." },
                            new JokeItem() { Id = 5,  Joke = "Hear about the new restaurant called Karma? There’s no menu: You get what you deserve." },
                            new JokeItem() { Id = 6,  Joke = "A woman in labour suddenly shouted, Shouldn’t! Wouldn’t! Couldn’t! Didn’t! Can’t! Don’t worry, said the doctor. Those are just contractions." },
                            new JokeItem() { Id = 7,  Joke = "Why don't scientists trust atoms? Because they makeup everything!" },
                            new JokeItem() { Id = 8,  Joke = "Why did the chicken go to the seance? To get to the other side." },
                            new JokeItem() { Id = 9,  Joke = "Where are average things manufactured? The satisfactory!" },
                            new JokeItem() { Id = 10, Joke = "What is a lazy person's favorite exercise? Didly-Squats!" },
                            new JokeItem() { Id = 11, Joke = "I ate a clock yesterday. It was very time-consuming." },
                            new JokeItem() { Id = 12, Joke = "I invented a new word...Plagarism" },
                            new JokeItem() { Id = 13, Joke = "Helvetica and Times New Roman walk into a bar.  Get out of here shouts the bartender.  We don't serve your type!!" },
                            new JokeItem() { Id = 14, Joke = "My dog ate all the Scrabble tiles.  For days he kept leaving little messages around the house." },
                            new JokeItem() { Id = 15, Joke = "The dinosaurs looked at Chuck Norris the wrong way once ...ONCE." },
                            new JokeItem() { Id = 16, Joke = "Chuck Norris puts the 'laughter' in 'manslaughter'." },
                            new JokeItem() { Id = 17, Joke = "Once a cobra bit Chuck Norris' leg. After five days of excruciating pain, the cobra died." },
                            new JokeItem() { Id = 18, Joke = "Chuck Norris can divide by zero." },
                            new JokeItem() { Id = 19, Joke = "Chuck Norris doesn't read books. He stares them down until he gets the information he wants." },
                            new JokeItem() { Id = 20, Joke = "Some kids piss their name in the snow. Chuck Norris can piss his name into concrete." },

                        });

                    context.SaveChanges();
                }
            }

        }

    }
}
