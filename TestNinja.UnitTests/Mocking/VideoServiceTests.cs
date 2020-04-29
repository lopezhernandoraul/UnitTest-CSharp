using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _repository;
        private VideoService _videoService;


        [SetUp]
        public void SetUp() 
        {
            _fileReader = new Mock<IFileReader>();
            _repository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object,_repository.Object);
        }
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            //var fileReader= new Mock<IFileReader>();
            //fileReader.Setup(fr => fr.Read("video.txt")).Returns("");


            ////var service = new VideoService(new FakeFileReader());

            //var result = service.ReadVideoTitle();

            //Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnproccesedVideosAsCSV_AllVideosAreProccesed_ReturnAndEmptyString()
        {
            _repository.Setup(s => s.GetUnprocessedVideos()).Returns(List<Video>());

            var result = _videoService.GetUnprocessedVideoAsCsv();

            Assert.That(result, IsEqualTo(""));
        }

        [Test]
        public void GetUnproccesedVideosAsCSV_AFewUnprocessedVideos_ReturnAStringWithIdOfUnprocessedVideos()
        {
            _repository.Setup(s => s.GetUnprocessedVideos()).Returns(new List<Video>
            {
                new Video { Id = 1 },
                new Video { Id = 2 },
                new Video { Id = 3 },
            });
            

            var result = _videoService.GetUnprocessedVideoAsCsv();

            Assert.That(result, IsEqualTo(""));
        }
    }
}
