public interface IMovieServiceLogic
{
    List<MovieModel> GetAiringMovies();
    void AddMovie(string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier, int age);
    void UpdateMovie(int id, string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier, int age);
    void DeleteMovie(int id);
}