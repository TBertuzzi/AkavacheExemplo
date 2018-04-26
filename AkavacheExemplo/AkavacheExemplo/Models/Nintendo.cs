using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkavacheExemplo.Models
{
    public partial class Nintendo
    {
        [JsonProperty("games")]
        public Games Games { get; set; }
    }
}
