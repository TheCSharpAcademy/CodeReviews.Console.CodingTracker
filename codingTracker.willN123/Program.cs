namespace CodingTracker;

class Program
{
    private static void Main()
    {
        CrudOperations crud = new();
        InputValidation valid = new();
        CodeOperations code = new();
        SpectreDisplay spec = new();

        string? input;

        crud.CreateTable();

        MainMenu();

        void MainMenu()
        {
            while (true)
            {
                GoToSelection(spec.MainMenuSelection());
            }
        }

        void GoToSelection(string input)
        {
            switch (input)
            {
                case "Exit":
                    Environment.Exit(0);
                    break;
                case "View":
                    ViewScreen();
                    break;
                case "Add":
                    AddScreen();
                    break;
                case "Update":
                    UpdateScreen();
                    break;
                case "Delete":
                    DeleteScreen();
                    break;
                default:
                    break;
            }
        }

        void ViewScreen()
        {
            spec.DisplayTable(crud.ReadTable());

            spec.WaitForEnter();
        }

        void AddScreen(string id = "")
        {
            string validEnd = "";
            string validStart = "";
            string validDate = "";

            while (!valid.ValidDateInput(input = spec.DatePrompt()))
            {
                if (input == "0")
                {
                    return;
                }

                spec.InvalidEntry();
            }
            validDate = input;

            do
            {
                while (!valid.ValidTimeInput(input = spec.StartTimePrompt()))
                {
                    if (input == "0")
                    {
                        return;
                    }

                    spec.InvalidEntry();
                }
                validStart = $"{input[..2]}:{input.Substring(2, 2)}";

                while (!valid.ValidTimeInput(input = spec.EndTimePrompt()))
                {
                    if (input == "0")
                    {
                        return;
                    }

                    spec.InvalidEntry();
                }
                validEnd = $"{input[..2]}:{input.Substring(2, 2)}";
            }while (!code.CompareTime(validStart, validEnd));

            string total = code.GetTotalTime(validStart, validEnd);

            spec.ShowRecordInput(validDate, validStart, validEnd, total);

            if (spec.ConfirmEntry())
            {
                if(id == "")
                {
                    crud.AddToTable(validDate, validStart, validEnd, total);
                }
                else
                {
                    crud.UpdateTable(validDate, validStart, validEnd, total, id);
                }
            }            
        }

        void UpdateScreen()
        {
            spec.DisplayTable(crud.ReadTable());

            while(!valid.ValidIdInput(crud.ReadTable(), input = spec.IdPrompt()))
            {
                if(input == "0")
                {
                    return;
                }

                spec.InvalidEntry();
            }
            string validId = input;

            AddScreen(validId);
        }

        void DeleteScreen()
        {
            spec.DisplayTable(crud.ReadTable());

            while (!valid.ValidIdInput(crud.ReadTable(), input = spec.IdPrompt()))
            {
                if(input == "0")
                {
                    return;
                }

                spec.InvalidEntry();
            }
            string validId = input;

            spec.ConfirmEntry();

            crud.DeleteFromTable(validId);
        }
    }
}