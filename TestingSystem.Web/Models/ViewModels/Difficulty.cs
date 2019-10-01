using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestingSystem.Web.Models.ViewModels
{
    public class Difficulty
    {
        public int Value { set; get; }
        public string Text { set; get; }

        public Difficulty() { }
        public Difficulty(int v, string t)
        {
            Value = v;
            Text = t;
        }
    }
}