using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem.BOL.Model
{
    public class ThemesInTestDTO
    {
        public int Id { get; set; }

        public int ThemeId { get; set; }

        public int TestId { get; set; }

        public string ThemeName { set; get; }
    }
}
