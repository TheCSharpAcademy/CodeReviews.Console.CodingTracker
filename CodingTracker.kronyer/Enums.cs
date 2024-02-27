using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Enums 
    {
        public enum MenuChoices
        {
            [Display(Name = "Add Record")]
            AddRecord,

            [Display(Name = "View Records")]
            ViewRecords,

            [Display(Name = "Delete Record")]
            DeleteRecord,

            [Display(Name = "Update Record")]
            UpdateRecord,

            Quit
        }
    }
}
