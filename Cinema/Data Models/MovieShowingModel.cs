public class MovieShowingModel
{
	public int Id { get; set; }

	public int MovieId { get; set; }

	public int TheaterId { get; set; }

	public DateTime ShowTime { get; set; }

	public bool IsCulinary { get; set; }

	public double ExtraPrice { get; set; }

	public MovieShowingModel(int id, int movieId, int theaterId, DateTime showTime, bool isCulinary, double extraPrice)
	{
		Id = id;
		MovieId = movieId;
		TheaterId = theaterId;
		ShowTime = showTime;
		IsCulinary = isCulinary;
		ExtraPrice = extraPrice;
	}
}