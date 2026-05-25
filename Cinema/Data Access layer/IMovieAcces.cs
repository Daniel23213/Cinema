public interface IMovieAcces
{
    List<MovieModel> GetAiringMovies(MoviesGenres genre);
    void AddMovie(string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier);
    void UpdateMovie(int id, string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier);
    void DeleteMovie(int id);
}