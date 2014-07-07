using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Flickr
    {
        public Flickr()
        {
            
        }

        private string flickerRU(String args)
        {
            return "https://api.flickr.com/services/rest/?format=json" +
                    "&nojsoncallback=1&api_key=256663858aa10e52a838a58b7866d858" + args;
        }

        private string tagsRU(String tags)
        {
            return flickerRU("&method=flickr.photos.search&sort=random&per_page=10&tags=" + tags);
        }

        private string getOne(String id)
        {
            return flickerRU("&method=flickr.photos.getSizes&photo_id=" + id);
        }

        public String getImageUrlForTag(String imageTag)
        {
            WebClient webClient = new WebClient();
            String json = webClient.DownloadString(tagsRU(Uri.EscapeUriString(imageTag)));
            dynamic a = JsonConvert.DeserializeObject(json);
            String id = a.photos.photo[1].id;
            Console.Out.WriteLine(id);

            String jsonImage = webClient.DownloadString(getOne(id));
            Console.Out.WriteLine(jsonImage);

            dynamic b = JsonConvert.DeserializeObject(jsonImage);

            String source = b.sizes.size[0].source;
            Console.Out.WriteLine(source);
            return source;
        }

    }
}
