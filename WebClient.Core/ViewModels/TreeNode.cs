﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.ViewModels
{
    public class TreeNode
    {
        public int Id { get; set; }
        public bool Children { get; set; }
        public string Title { get; set; }
        public string TypeNode { get; set; }
    }
}
