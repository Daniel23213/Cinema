public interface IMovieAcces
{
    List<MovieModel> GetAiringMovies();
    void AddMovie(string title, string author, string genre, TimeSpan duration, DateTime premier);
    void UpdateMovie(int id, string title, string author, string genre, TimeSpan duration, DateTime premier);
    void DeleteMovie(int id);
}