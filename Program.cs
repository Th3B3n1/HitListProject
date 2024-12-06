using MySql.Data.MySqlClient;
using System.Data.SqlTypes;

//Establishing a connection to the database
List<Album> albums = new List<Album>();
List<TopList> topLists = new List<TopList>();
try
{
	MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1;uid=root;pwd=;database=slagerlista");
	mySqlConnection.Open();
	MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
	mySqlCommand.CommandText = "SELECT * FROM album";
	MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
	while (mySqlDataReader.Read())
	{
		albums.Add(new Album(mySqlDataReader.GetInt32("id"), mySqlDataReader.GetString("eloado"), mySqlDataReader.GetString("cim")));
	}
	mySqlDataReader.Close();
    mySqlCommand.CommandText = "SELECT * FROM toplista";
    mySqlDataReader = mySqlCommand.ExecuteReader();
    while (mySqlDataReader.Read())
    {
        try
        {
            topLists.Add(new TopList(mySqlDataReader.GetInt32("albumid"), mySqlDataReader.GetInt32("helyezes"), mySqlDataReader.GetInt32("platinadb"), mySqlDataReader.GetInt32("ev"), mySqlDataReader.GetString("kiado")));
        }
        catch (SqlNullValueException e)
        {
            Console.WriteLine("[WARNING] Data is null but adding it regardless (" + e.Message + ")");
            topLists.Add(new TopList(mySqlDataReader.GetInt32("albumid"), mySqlDataReader.GetInt32("helyezes"), 0, mySqlDataReader.GetInt32("ev"), mySqlDataReader.GetString("kiado")));
        }
    }
    mySqlDataReader.Close();
    mySqlConnection.Close();
}
catch (MySqlException e)
{
	Console.WriteLine(e.Message);
}

/*
for (int i = 0; i < albums.Count; i++)
{
	Console.WriteLine(albums[i].Id + " | " + albums[i].Performer + " | " + albums[i].Title);
}

for (int i = 0; i < topLists.Count; i++)
{
	Console.WriteLine(topLists[i].AlbumId + " | " + topLists[i].Rank + " | " + topLists[i].Platinum + " | " + topLists[i].Year + " | " + topLists[i].Publisher);
}
*/


//2.feladat
Console.WriteLine("-- 2.feladat --");
IEnumerable<Album> list = albums.Where(album => album.Title.Contains("fekete") || album.Performer.Contains("fekete"));
foreach (Album album in list)
{
	Console.WriteLine(album.Id + " | " + album.Performer + " | " + album.Title);
}

//3.feladat
Console.WriteLine("--- 3.feladat ---");
Dictionary<string, int> publisherAmount = new Dictionary<string, int>();
for (int i = 0; i < topLists.Count; i++)
{
	if (!publisherAmount.ContainsKey(topLists[i].Publisher))
	{
        publisherAmount.Add(topLists[i].Publisher, 1);
	}
	else
	{
        publisherAmount[topLists[i].Publisher] += 1;
	}
}
foreach (KeyValuePair<string, int> pair in publisherAmount.OrderByDescending(e => e.Value))
{
	Console.WriteLine(pair);
}

//4.feladat
Console.WriteLine("---- 4.feladat ----");
Dictionary<string, int> platinumPerformers = new Dictionary<string, int>();
for (int i = 0; i < albums.Count; i++)
{
	for (int y = 0; y < topLists.Count; y++)
	{
		if (albums[i].Id == topLists[y].AlbumId)
		{
			if (!platinumPerformers.ContainsKey(albums[i].Performer) && topLists[y].Platinum > 0)
			{
                platinumPerformers.Add(albums[i].Performer, topLists[y].Platinum);
			}
			else if (platinumPerformers.ContainsKey(albums[i].Performer))
			{
                platinumPerformers[albums[i].Performer] += topLists[y].Platinum;
			}
		}
	}
}

KeyValuePair<string, int> biggest = platinumPerformers.First();
foreach (KeyValuePair<string, int> pair in platinumPerformers)
{
	if (pair.Value > biggest.Value)
	{
		biggest = pair;
	}
}
Console.WriteLine(biggest.Key + " | " + biggest.Value);

//5.feladat
Console.WriteLine("----- 5.feladat -----");
Dictionary<Album, List<int>> albumYears = new Dictionary<Album, List<int>>();
for (int i = 0; i < albums.Count; i++)
{
	for (int y = 0; y < topLists.Count; y++)
	{
		if (albums[i].Id == topLists[y].AlbumId)
		{
			if (!albumYears.ContainsKey(albums[i]))
			{
                albumYears.Add(albums[i], new List<int> { topLists[y].Year });
			}
			else if (albumYears.ContainsKey(albums[i]))
			{
                albumYears[albums[i]].Add(topLists[y].Year);
			}
		}
	}
}

List<KeyValuePair<Album, List<int>>> pairs = new List<KeyValuePair<Album, List<int>>>();
foreach (KeyValuePair<Album, List<int>> pair in albumYears)
{
	if (pair.Value.Count >= 3)
	{
		pairs.Add(pair);
	}
}

for (int i = 0; i < pairs.Count; i++)
{
	Console.WriteLine(pairs[i].Key.Title + " | " + pairs[i].Key.Performer + " | " + pairs[i].Value.Count);
}

//6.feladat
Console.WriteLine("-- 6.feladat --");
IEnumerable<Album> albumsContainingPerformers = albums.Where(album => album.Title.Contains(album.Performer));
foreach (Album album in albumsContainingPerformers)
{
    Console.WriteLine(album.Performer + " | " + album.Title);
}

//7.feladat
Console.WriteLine("----- 7.feladat -----");
Dictionary<string, HashSet<string>> topListForPerformer = new Dictionary<string, HashSet<string>>();
for (int i = 0; i < albums.Count; i++)
{
	if (albums[i].Performer == "Palya Bea")
	{
		for (int y = 0; y < topLists.Count; y++)
		{
			if (albums[i].Id == topLists[y].AlbumId)
			{
				if (!topListForPerformer.ContainsKey(topLists[y].Publisher))
				{
                    topListForPerformer.Add(topLists[y].Publisher, new HashSet<string> { });
				}
			}
		}
		break;
	}
}
for (int i = 0; i < albums.Count; i++)
{
    for (int y = 0; y < topLists.Count; y++)
	{
		if (albums[i].Id == topLists[y].AlbumId)
		{
			if (!(albums[i].Performer == "Palya Bea"))
			{
                if (topListForPerformer.ContainsKey(topLists[y].Publisher))
                {
                    topListForPerformer[topLists[y].Publisher].Add(albums[i].Performer);
                }
            }
		}
	}
}

foreach (KeyValuePair<string, HashSet<string>> pair in topListForPerformer)
{
	Console.WriteLine(pair.Key + ":");
	foreach(string item in pair.Value)
	{
		Console.WriteLine("--- " + item);
	}
}
