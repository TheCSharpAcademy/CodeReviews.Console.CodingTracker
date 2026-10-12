using System.ComponentModel.DataAnnotations;

namespace CodingTracker;

internal class Enums
{
  internal enum Column
  {
    [Display(Name = "ID")]
    id,

    [Display(Name = "Activity")]
    activity,

    [Display(Name = "Start time")]
    startTime,

    [Display(Name = "End time")]
    endTime,

    [Display(Name = "Duration")]
    duration
  }

  internal enum MenuChoice
  {
    [Display(Name = "View entries")]
    viewItems,

    [Display(Name = "[green]Start[/] a session [green]now[/]")]
    addItemStartNow,

    [Display(Name = "[green]Add[/] a previous session")]
    addItem,

    [Display(Name = "[blue]Edit[/] an entry")]
    editItem,

    [Display(Name = "[red]Delete[/] an entry")]
    deleteItem,

    [Display(Name = "Exit")]
    exit,
  }
}