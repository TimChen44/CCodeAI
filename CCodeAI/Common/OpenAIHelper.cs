using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.VisualStudio.VCProjectEngine;

namespace CCodeAI.Common
{
    public class OpenAIHelper
    {

    }


    public class GptRequest
    {
        public string prompt { get; set; }
        public int max_tokens { get; set; }
        public double temperature { get; set; }
        public double frequency_penalty { get; set; }
        public double presence_penalty { get; set; }
        public double top_p { get; set; }
        public string[] stop { get; set; }
    }

    public class TextCompletion
    {
        public string id { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public List<CompletionChoice> choices { get; set; }
        public Usage usage { get; set; }
    }

    public class CompletionChoice
    {
        public string text { get; set; }
        public int index { get; set; }
        public string finish_reason { get; set; }
        public object logprobs { get; set; }
    }

    public class Usage
    {
        public int completion_tokens { get; set; }
        public int prompt_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}
