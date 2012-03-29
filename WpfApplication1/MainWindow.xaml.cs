using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace WpfApplication1
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            Artists = new ObservableCollection<Artist>();
            DataContext = this;
            InitializeComponent();
        }

        private void GetArtists(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            var address = new Uri("http://ws.audioscrobbler.com/2.0/?method=chart.gethypedartists&api_key=b25b959554ed76058ac220b7b2e0a026&format=json");
            var result = wc.DownloadString(address);

            var wrapper = JsonConvert.DeserializeObject<Wrapper>(result);

            foreach (var artist in wrapper.Artists.artist)
            {
                Artists.Add(artist);
            }
        }

        public ObservableCollection<Artist> Artists { get; private set; }
    }

    public class Wrapper
    {
        public Artists Artists { get; set; }
    }

    public class Artists
    {
        public Artist[] artist { get; set; }
    }

    public class Artist
    {
        public string name { get; set; }
        public string percentagechange { get; set; }
        public string mbid { get; set; }
        public string url { get; set; }
        public string streamable { get; set; }
        public Image[] image { get; set; }

        public string LargeImage
        {
            get
            {
                var x = image.Where(i => i.size == "extralarge").FirstOrDefault();
                if (x != null) return x.text;
                return string.Empty;
            }
        }
    }

    public class Image
    {
        [JsonProperty(PropertyName = "#text")]
        public string text { get; set; }
        public string size { get; set; }
    }
}
