namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            var producerId = int.Parse(Console.ReadLine());

            Console.WriteLine(ExportAlbumsInfo(context, producerId));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Albums
                .Where(x => x.ProducerId == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    AlbumPrice = x.Price,
                    Songs = x.Songs.Select(x => new
                    {
                        SongName = x.Name,
                        SongPrice = x.Price,
                        WriterName = x.Writer.Name,
                    })
                    .OrderByDescending(x => x.SongName)
                    .ThenBy(x => x.WriterName)
                    .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var albumInfo in albumsInfo.OrderByDescending(x=>x.AlbumPrice))
            {
                int counter = 1;

                sb.AppendLine($"-AlbumName: {albumInfo.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {albumInfo.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {albumInfo.ProducerName}");
                sb.AppendLine("-Songs:");

                foreach (var song in albumInfo.Songs)
                {
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:f2}");
                    sb.AppendLine($"---Writer: {song.WriterName}");
                    counter++;
                }

                sb.AppendLine($"-AlbumPrice: {albumInfo.AlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
