internal class TopList
{
	private int albumId;
	private int rank;
	private int platinum;
	private int year;
	private string publisher;

	public TopList(int albumId, int rank, int platinum, int year, string publisher)
	{
		this.albumId = albumId;
		this.rank = rank;
		this.platinum = platinum;
		this.year = year;
		this.publisher = publisher;
	}

	public int AlbumId { get => albumId; set => albumId = value; }
	public int Rank { get => rank; set => rank = value; }
	public int Platinum { get => platinum; set => platinum = value; }
	public int Year { get => year; set => year = value; }
	public string Publisher { get => publisher; set => publisher = value; }
}