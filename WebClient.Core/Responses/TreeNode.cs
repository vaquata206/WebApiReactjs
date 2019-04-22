using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Responses
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Type { get; set; }
        public bool Children { get; set; }
    }
}
