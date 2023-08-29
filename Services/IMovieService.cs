using MinAuth.Models;

namespace MinAuth.Services{
    public interface IMovieService{
        // methods to be performed
        public Movie Create(Movie movie);
        public Movie Get(int id);
        public List<Movie> AllMovies();
        public Movie Update(int id, Movie movie);
        // public bool Delete(int id);
    }
}