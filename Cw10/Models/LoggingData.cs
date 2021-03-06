﻿using Newtonsoft.Json;

namespace Cw10.ModelsManual
{
    public class LoggingData
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
        public string QueryString { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}