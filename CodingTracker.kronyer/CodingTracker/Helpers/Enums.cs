
using System.ComponentModel.DataAnnotations;


namespace CodingTracker.Helpers
{
    internal class Enums
    {
        public enum MenuChoices
        {
            [Display(Name = "Add Habit")]
            AddHabit,

            [Display(Name = "Delete Habit")]
            DeleteHabit,

            [Display(Name = "Update Habit")]
            UpdateHabit,

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
