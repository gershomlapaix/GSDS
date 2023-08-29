using MinAuth.Models;

namespace MinAuth.Repository{
    public class MovieRepository{
        public static List<Movie> movies = new()
        {
            new(){Id=1, Title="Eternals", Description="This is a description", Rating=6.4f},
            new(){Id=2, Title="Dune", Description="This is a description", Rating=6.8f},
            new(){Id=3, Title="No time to die", Description="This is a description", Rating=6.0f},
        };
    }
}