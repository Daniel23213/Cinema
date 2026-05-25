public class MovieServiceLogic : IMovieServiceLogic
{
    private readonly IMovieAcces _repo;

    public MovieServiceLogic()
    {
        _repo = new MovieAcces();
    }

    public List<MovieModel> GetAiringMovies()
    {
        return _repo.GetAiringMovies();
    }

    public void AddMovie(string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be greater than 0");

        _repo.AddMovie(title, author, genre, duration, premier);
    }

    public void UpdateMovie(int id, string title, string author, MoviesGenres genre, TimeSpan duration, DateTime premier)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        _repo.UpdateMovie(id, title, author, genre, duration, premier);
    }

    public void DeleteMovie(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        _repo.DeleteMovie(id);
    }
}