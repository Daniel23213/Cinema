public interface IMovieServiceLogic
{
    List<MovieModel> GetAiringMovies();

    void AddMovie(
        string title,
        string author,
        MoviesGenres genre,
        TimeSpan duration,
        DateTime premier,
        int age);

    void UpdateMovie(
        int id,
        string title,
        string author,
        MoviesGenres genre,
        TimeSpan duration,
        DateTime premier,
        int age);

    void DeleteMovie(int id);

    bool AddMovieShowing(
        int movieId,
        int theaterId,
        DateTime showTime,
        bool isCulinary);

    List<string> GetShowings(UserModel user);

    List<string> GetShowingsByGenre(MoviesGenres genre);
}