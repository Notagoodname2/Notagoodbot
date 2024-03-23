using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestBot
{
    internal class JsonRead
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public async Task ReadJson()
        { 
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                JsonStruct data = JsonConvert.DeserializeObject<JsonStruct>(json);
                this.token = data.token;
                this.prefix = data.prefix;
            }
        }
    }

    internal sealed class JsonStruct
    {
        public string token { get; set; }
        public string prefix { get; set; }
    }
}
