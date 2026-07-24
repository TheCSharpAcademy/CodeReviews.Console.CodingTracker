using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal class Enums
    {
        internal enum MenuAction
        {
            [Display(Name = "View Records")]
            ViewRecords,

            [Display(Name = "Add Record")]
            AddRecord,

            [Display(Name = "Update Record")]
            UpdateRecord,

            [Display(Name = "Delete Record")]
            DeleteRecord,

            [Display(Name = "Stopwatch")]
            StopwatchService,
            Exit
        }
    }
}
