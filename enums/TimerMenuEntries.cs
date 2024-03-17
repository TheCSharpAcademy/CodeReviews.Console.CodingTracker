using System.ComponentModel.DataAnnotations;
using static CodingTracker.enums.EnumHelpers;

namespace CodingTracker.enums;

public enum TimerMenuEntries
{
    [Display(Name = "Start Timer"), Method("StartTimer")]
    Start,
    
    [Display(Name = "Last Session"), Method("ShowLastSession")]
    LastSession,
    
    [Display(Name = "Return to Main Menu")]
    BackToMainMenu
}