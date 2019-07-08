using System.Collections.Generic;
using System.Net;
using System.Web.Script.Serialization;

namespace dnSpy_update_checker.Github {
    class GithubApi {
        
        public GithubApiResult GetResult(string author, string repo) {
            var Url = $"https://api.github.com/repos/{author}/{repo}/releases";

            using (var client = new WebClient()) {
                client.Headers.Add("User-Agent", "Nothing");

                var content = client.DownloadString(Url);
                return ReadToObject(content);
            }
        }


        //// Deserialize a JSON stream to a User object.  
        private GithubApiResult ReadToObject(string json) {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<IList<GithubApiResult>>(json)[0];
        }
    }


    
}
