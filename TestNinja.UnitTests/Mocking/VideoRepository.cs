using System.Collections.Generic;
using TestNinja.Mocking;
using System.Linq;
namespace TestNinja.UnitTests.Mocking
{
    public interface IVideoRepository
    {
        IEnumerable<Video> GetUnprocessedVideos();
    }

    public class VideoRepository : IVideoRepository
    {
        public IEnumerable<Video> GetUnprocessedVideos()
        {
            using (var context = new VideoContext())
            {
                var videos =
               (from video in context.Videos
                where !video.IsProcessed
                select video).ToList();

                return videos;
            }
        }
    }
}
