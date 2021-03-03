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

            var duration = int.Parse(Console.ReadLine());

            Console.WriteLine(ExportSongsAboveDuration(context, duration));
        }


        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var allSongs = context.Songs.ToList();

            var songsInfo = allSongs
                .Where(x =>x.Duration.TotalSeconds > duration)
                .Select(x => new
                {
                    SongName = x.Name,
                    PerformerFullName = x.SongPerformers.Select(x => x.Performer.FirstName + " " + x.Performer.LastName).FirstOrDefault(),
                    WriterName = x.Writer.Name,
                    AlbumProducer = x.Album.Producer.Name,
                    Duration = x.Duration.ToString()
                })
                .OrderBy(x=>x.SongName)
                .ThenBy(x=>x.WriterName)
                .ThenBy(x=>x.PerformerFullName)
                .ToList();

            StringBuilder sb = new StringBuilder();
            int counter = 1;

            foreach (var songInfo in songsInfo)
            {
                sb.AppendLine($"-Song #{counter}");
                sb.AppendLine($"---SongName: {songInfo.SongName}");
                sb.AppendLine($"---Writer: {songInfo.WriterName}");
                sb.AppendLine($"---Performer: {songInfo.PerformerFullName}");
                sb.AppendLine($"---AlbumProducer: {songInfo.AlbumProducer}");
                sb.AppendLine($"---Duration: {songInfo.Duration}");

                counter++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
