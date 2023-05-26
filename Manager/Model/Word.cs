using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Model
{
    internal class Word
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? Words { get; set; }
        public string? Transcriptions { get; set; }
        public string? Sentence { get; set; }
        public string? TranslateWords { get; set; }
        public string? TransSentence { get; set; }
    }
}
