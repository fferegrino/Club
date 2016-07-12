using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Club.ViewModels
{
    public class TopicViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Nivel")]
        public string UserLevel { get; set; }

        [Display(Name = "Nivel")]
        public int UserLevelId { get; set; }
    }
}
