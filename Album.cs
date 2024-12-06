internal class Album
{
	private int id;
	private string performer;
	private string title;

	public Album(int id, string performer, string title)
	{
		this.id = id;
		this.performer = performer;
		this.title = title;
	}
	public int Id { get => id; set => id = value; }
	public string Performer { get => performer; set => performer = value; }
	public string Title { get => title; set => title = value; }
	public override int GetHashCode()
	{
		return HashCode.Combine(Id);
	}
	public override bool Equals(object? obj)
	{
		Album? album = obj as Album;
		if (album == null) return false;
		if (album.Id == this.Id) return true;
		return false;
	}
	
}