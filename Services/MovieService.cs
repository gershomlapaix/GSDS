using MinAuth.Models;
using MinAuth.Repository;

namespace MinAuth.Services{
    public class MovieService: IMovieService{

        // make a new movie
        public Movie Create(Movie movie){
            movie.Id = MovieRepository.movies.Count + 1;
            MovieRepository.movies.Add(movie);

            return movie;
        }
        

        // get one movie
        public Movie Get(int id){
            var movie = MovieRepository.movies.FirstOrDefault(o => o.Id == id);

            if(movie is null) return null;
            return movie;
        }

        // Get all the movies stored
        public List<Movie> AllMovies(){
            var movies = MovieRepository.movies;
            return movies;
        }
        
        // Update a movie
        public Movie Update(int id, Movie newMovie){
            var oldMovie = MovieRepository.movies.FirstOrDefault(o => o.Id == id);

            if(oldMovie is null) return null;

            // title
            oldMovie.Title = newMovie.Title;
            oldMovie.Description = newMovie.Description;
            oldMovie.Rating = newMovie.Rating;

            return oldMovie;
        }

    }
}