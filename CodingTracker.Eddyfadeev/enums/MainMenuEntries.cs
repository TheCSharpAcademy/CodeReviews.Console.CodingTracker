using System.ComponentModel.DataAnnotations;
using static CodingTracker.enums.EnumHelpers;

namespace CodingTracker.enums;

internal enum MainMenuEntries
{
    [Display(Name = "Timer")]
    Timer,
    
    [Display(Name = "Add record"), Method("AddRecord")]
    AddRecord,
    
    [Display(Name = "View Records"), Method("ViewRecords")]
    ViewRecords,
    
    [Display(Name = "Delete Record"), Method("DeleteRecord")]
    DeleteRecord,
    
    [Display(Name = "Update Record"), Method("UpdateRecord")]
    UpdateRecord,
    
    [Display(Name = "Reports")]
    Reports,
    
    Quit
}