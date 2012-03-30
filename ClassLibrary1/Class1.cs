using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Newtonsoft.Json;
using WpfApplication1;
using Xunit;

namespace ClassLibrary1
{
    public class Class1
    {
        [Fact]
        public async Task get_artists()
        {
            // arrange
            var asyncWebClient = Substitute.For<IAsyncWebClient>();
            var artists = new List<Artist>();
            var cancellationTokenSource = new CancellationTokenSource();
            var artist = new Artist
                             {
                                 name = "Jake",
                                 image = new[]{new Image{ size = "small", text = "http://localhost/image.png"}}
                             };
            asyncWebClient
                .GetStringAsync(Arg.Any<Uri>(), cancellationTokenSource.Token)
                .Returns(Task.FromResult(JsonConvert.SerializeObject(new Wrapper { Artists = new Artists { artist = new[]{artist }} })));
            asyncWebClient
                .GetDataAsync(Arg.Any<Uri>())
                .Returns(Task.FromResult(new byte[0]));

            // act
            await MainWindow.GetArtistsAsync(asyncWebClient, cancellationTokenSource.Token,
                                       new TestProgress<Artist>(artists.Add), bytes => null);

            // assert
            Assert.Equal(1, artists.Count);
            Assert.Equal("Jake", artists[0].name);
        }
    }
}
