using CodingTracker.Arashi256.Classes;
using CodingTracker.Arashi256.Controllers;
using CodingTracker.Arashi256.Models;
using Spectre.Console;

namespace CodingTracker.Arashi256.Views
{
    internal class CodingGoalView
    {
        private const int GOALS_MAIN_MENU_OPTION_NUM = 5;
        private Table _tblCodingGoalMenu;
        private GoalController _goalController;
        private SessionController _sessionController;
        private string[] _goalOptions =
        {
            "Set new coding goal",
            "Update the existing coding goal",
            "Delete the existing coding goal",
            "Current progress to goal",
            "Return to Main Menu"
        };

        public CodingGoalView(SessionController sc)
        {
            _sessionController = sc;
            _goalController = new GoalController();
        }

        public void CodingGoalMenu()
        {
            _tblCodingGoalMenu = new Table();
            _tblCodingGoalMenu.AddColumn(new TableColumn("[white]CHOICE[/]").Centered());
            _tblCodingGoalMenu.AddColumn(new TableColumn("[white]OPTION[/]").LeftAligned());
            for (int i = 0; i < _goalOptions.Length; i++)
            {
                _tblCodingGoalMenu.AddRow($"[lightcyan3]{i + 1}[/]", $"[darkslategray1]{_goalOptions[i]}[/]");
            }
            _tblCodingGoalMenu.Alignment(Justify.Center);
            int selectedValue = 0;
            do
            {
                Console.WriteLine("\n");
                AnsiConsole.Render(new Text("C O D I N G   G O A L S   M E N U").Centered());
                AnsiConsole.Write(_tblCodingGoalMenu);
                selectedValue = CommonUI.MenuOption($"Enter a value between 1 and {_goalOptions.Length}: ", 1, _goalOptions.Length);
                ProcessCodingGoalMenu(selectedValue);
            } while (selectedValue != GOALS_MAIN_MENU_OPTION_NUM);
        }

        private void ProcessCodingGoalMenu(int option)
        {
            AnsiConsole.Markup($"[white]Coding goal option selected: {option}[/]\n");
            switch (option)
            {
                case 1:
                    // Set coding goal.
                    AddNewCodingGoal();
                    break;
                case 2:
                    // Update existing coding goal.
                    UpdateCodingGoal();
                    break;
                case 3:
                    // Delete existing coding goal.
                    DeleteCodingGoal();
                    break;
                case 4:
                    // Current goal progress.
                    DisplayCurrentCodingGoalProgress();
                    break;
            }
        }

        private void AddNewCodingGoal()
        {
            DateTime? deadlineDateTime;
            DateTime startDateTime = DateTime.Now;
            int hours = 0;
            bool isValid = false;
            if (_goalController.HasCodingGoal())
            {
                AnsiConsole.MarkupLine("[yellow]You already have a coding goal set. Only one coding goal may be active at one time.[/]");
            }
            else
            {
                do
                {
                    do
                    {
                        AnsiConsole.MarkupLine("[white]What is the deadline for this coding goal?[/]");
                        deadlineDateTime = CommonUI.GetUserDateTimeDialog();
                        if (deadlineDateTime != null)
                        {
                            AnsiConsole.MarkupLine($"[white]You entered[/] [green]{deadlineDateTime:dd-MM-yy HH:mm:ss}[/] [white]as the goal deadline.[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[yellow]Operation cancelled.[/]");
                            return;
                        }
                        if (startDateTime > deadlineDateTime) AnsiConsole.MarkupLine($"[red]Coding goal deadline date cannot be in the past! Please try again.[/]");
                    } while (startDateTime > deadlineDateTime);
                    hours = CommonUI.GetHoursDialog();
                    if (hours != 0)
                    {
                        if (Utility.GetValidCodingHoursGoal(startDateTime, deadlineDateTime, hours))
                        {
                            isValid = true;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]This is not a realistic goal - you would need to code more than 24 hours a day![/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Operation cancelled.[/]");
                        return;
                    }
                } while (!isValid);
                if (isValid)
                {
                    AnsiConsole.MarkupLine($"[lime]{_goalController.AddCodingGoal(startDateTime, deadlineDateTime, hours)}[/]");
                    AnsiConsole.MarkupLine($"[cyan2]You will have to code a minimum of {Utility.CalculateRequiredCodingHoursPerDay(startDateTime, (DateTime)deadlineDateTime, hours)} hours per day to reach this goal.[/]");
                }
            }
            CommonUI.Pause();
        }

        private void UpdateCodingGoal()
        {
            DateTime? deadlineDateTime;
            int hours = 0;
            bool isValid = false;
            CodingGoal codingGoal = _goalController.GetCurrentCodingGoal();
            DisplayCodingGoal(codingGoal);
            if (AnsiConsole.Confirm("[yellow]Do you want to update this coding goal?[/]", false))
            {
                do
                {
                    do
                    {
                        AnsiConsole.MarkupLine("[white]What is the new deadline for this coding goal?[/]");
                        deadlineDateTime = CommonUI.GetUserDateTimeDialog();
                        if (deadlineDateTime != null)
                        {
                            AnsiConsole.MarkupLine($"[white]You entered[/] [green]{deadlineDateTime:dd-MM-yy HH:mm:ss}[/] [white]as the goal deadline.[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[yellow]Operation cancelled.[/]");
                            return;
                        }
                        if (codingGoal.StartDateTime > deadlineDateTime) AnsiConsole.MarkupLine($"[red]Coding goal deadline date cannot be in the past! Please try again.[/]");
                    } while (codingGoal.StartDateTime > deadlineDateTime);
                    hours = CommonUI.GetHoursDialog();
                    if (hours != 0)
                    {
                        if (Utility.GetValidCodingHoursGoal(codingGoal.StartDateTime, deadlineDateTime, hours))
                        {
                            isValid = true;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]This is not a realistic goal - you would need to code more than 24 hours a day![/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Operation cancelled.[/]");
                        return;
                    }
                } while (!isValid);
                if (isValid)
                {
                    codingGoal.DeadlineDateTime = (DateTime)deadlineDateTime;
                    codingGoal.Hours = hours;
                    AnsiConsole.MarkupLine($"[lime]{_goalController.UpdateCodingGoal(codingGoal)}[/]");
                    AnsiConsole.MarkupLine($"[cyan2]You will have to code a minimum of {Utility.CalculateRequiredCodingHoursPerDay(codingGoal.StartDateTime, (DateTime)deadlineDateTime, hours)} hours per day to reach this goal.[/]");
                }
            }
            CommonUI.Pause();
        }

        private void DeleteCodingGoal()
        {
            CodingGoal codingGoal = _goalController.GetCurrentCodingGoal();
            DisplayCodingGoal(codingGoal);
            if (AnsiConsole.Confirm("[yellow]Are you sure you want to delete this coding goal?[/]", false))
            {
                AnsiConsole.MarkupLine($"\n[lime]{_goalController.DeleteCodingGoal()}[/]");
            }
            CommonUI.Pause();
        }

        private void DisplayCurrentCodingGoalProgress()
        {
            DateTime currentDateTime = DateTime.Now;
            CodingGoal currentCodingGoal = _goalController.GetCurrentCodingGoal();
            DisplayCodingGoal(currentCodingGoal);
            if (currentCodingGoal != null)
            {
                List<CodingSession> codingSessions = _sessionController.GetCodingSessionsBetweenDates(currentCodingGoal.StartDateTime, currentDateTime, Utility.SortOrder.ASC);
                string hoursCompleted = Utility.SumCodingSessions(codingSessions.ToArray());
                double hoursRemaining = Utility.RoundToZero(currentCodingGoal.Hours - Utility.ConvertTimeStringToHours(hoursCompleted));
                double hoursRemainingPerDay = Utility.RoundToZero(Utility.CalculateRemainingHoursPerDay(currentDateTime, currentCodingGoal.DeadlineDateTime, currentCodingGoal.Hours, hoursCompleted));
                AnsiConsole.MarkupLine($"[skyblue1]Time completed so far:[/]\t\t[white]{hoursCompleted}[/]");
                AnsiConsole.MarkupLine($"[skyblue1]Time remaining:[/]\t\t\t[white]{Utility.ConvertHoursToTimeString(hoursRemaining)}[/]");
                AnsiConsole.MarkupLine($"[skyblue1]New hours per day required:[/]\t[white]{Utility.ConvertHoursToTimeString(hoursRemainingPerDay)}[/]");
                DisplayProgress(currentCodingGoal.Hours, Utility.ConvertTimeStringToHours(hoursCompleted));
                if (Utility.ConvertTimeStringToHours(hoursCompleted) >= currentCodingGoal.Hours)
                {
                    AnsiConsole.MarkupLine($"\n[lime]You have completed this coding goal! CONGRATULATIONS![/]\n");
                    if (AnsiConsole.Confirm("\n[yellow]Do you want to delete this coding goal?[/]"))
                    {
                        AnsiConsole.MarkupLine($"\n[lime]{_goalController.DeleteCodingGoal()}[/]");
                    }
                }
            }
            CommonUI.Pause();
        }
        private void DisplayProgress(double targetHours, double completedHours)
        {
            int totalLength = 40;
            int completedLength = (int)Math.Round((completedHours / targetHours) * totalLength);
            int remainingLength = totalLength - completedLength;
            if (remainingLength < 0) remainingLength = 0;
            var progressBar = new string('>', completedLength) + new string('-', remainingLength);
            AnsiConsole.MarkupLine($"\nProgress: [[{Utility.ColorizeProgressBar(progressBar)}]]\n");
        }

        private void DisplayCodingGoal(CodingGoal codingGoal)
        {
            if (codingGoal != null)
            {
                Table tblGoal = new Table();
                tblGoal.AddColumn(new TableColumn($"[skyblue1]ID[/]").LeftAligned());
                tblGoal.AddColumn(new TableColumn($"[white]{codingGoal.Id}[/]").RightAligned());
                tblGoal.AddRow($"[skyblue1]Start Date/Time[/]", $"[white]{codingGoal.StartDateTime:dd-MM-yy HH:mm:ss}[/]");
                tblGoal.AddRow($"[skyblue1]Deadline Date/Time[/]", $"[white]{codingGoal.DeadlineDateTime:dd-MM-yy HH:mm:ss}[/]");
                tblGoal.AddRow($"[skyblue1]Hours[/]", $"[white]{codingGoal.Hours}[/]");
                tblGoal.Alignment(Justify.Left);
                AnsiConsole.Write(tblGoal);
            }
            else
            {
                AnsiConsole.Render(new Panel("[red]There is no current coding goal set[/]"));
            }
        }
    }
}
