using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastforward.Models
{
    public class Checkbox
    {
        [Key]
        public string CheckboxId { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

        public Checkbox()
        {
        }

        public Checkbox(string name, bool check)
        {
            Name = name;
            Checked = check;
        }
    }
}
