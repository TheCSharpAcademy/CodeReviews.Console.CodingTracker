using System.ComponentModel.DataAnnotations;

namespace CodingTracker.Golvi1124.UI;
internal class Enums
{
    internal enum MainMenuChoices
    {
        [Display(Name = "Run Stopwatch")]
        RunStopwatch,

        [Display(Name = "Add Record")]
        AddRecord,

        [Display(Name = "View Records")]
        ViewRecords,

        [Display(Name = "Delete Record")]
        DeleteRecord,

        [Display(Name = "Update Record")]
        UpdateRecord,

        [Display(Name = "Analyse Records")]
        AnalyseRecords,

        Quit
    }

    internal enum AnalyseMenuChoices
    {
        [Display(Name = "Filter records per period")]
        FilterPerPeriod,

        [Display(Name = "Show total and average stats per period")]
        ViewTotalAndAverage,

        [Display(Name = "Have I reached the goal?")] //set the goal and check if reached
        IsGoalReached,

        Back
    }

    internal enum PeriodType
    {
        Year,
        Month,
        Week,
        Day,
        Back
    }
}