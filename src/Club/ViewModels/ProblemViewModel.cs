using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class ProblemViewModel
    {
        public int Id { get; set; }
        public int Year { get; set; }

        [Display(Name = "Tema")]
        public string Topic { get; set; }

        [Display(Name = "Tema")]
        public int TopicId { get; set; }

        [Display(Name = "Nivel")]
        public string Level { get; set; }

        [Display(Name = "Nivel")]
        public int LevelId { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Sitio")]
        public string Site { get; set; }
        public string Link { get; set; }

        [Display(Name = "Dificultad")]
        [Range(1, 10)]
        public int Difficulty { get; set; }

        
        public bool Attempted { get; set; }
        
        public bool? Accepted { get; set; }

        [Display(Name = "Notas")]
        public string Notes { get; set; }
    }
}
