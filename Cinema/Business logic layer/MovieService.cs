public class MovieService
{
    private readonly MovieRepository _repo = new MovieRepository();

    public List<Movies> GetAiringMovies()
    {
        return _repo.GetAiringMovies();
    }

    public void AddMovie(string title, string author, string genre, TimeSpan duration, DateTime premier)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new Exception("Title cannot be empty");

        _repo.AddMovie(title, author, genre, duration, premier);
    }
}