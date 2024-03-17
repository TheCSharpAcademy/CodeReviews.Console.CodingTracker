using System.ComponentModel.DataAnnotations;

namespace CodingTracker.enums;

internal enum ReportTypes
{
    [Display(Name = "Date to Today")]
    DateToToday,
    
    [Display(Name = "Date Range")]
    DateRange,
    
    [Display(Name = "Total")]
    Total,
    
    [Display(Name = "Total for Month")]
    TotalForMonth,
    
    [Display(Name = "Total for Year")]
    TotalForYear,
    
    [Display(Name = "Return to Main Menu")]
    BackToMainMenu
}