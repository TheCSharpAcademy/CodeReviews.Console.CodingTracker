using Buutyful.Coding_Tracker;
using Buutyful.Coding_Tracker.State;



DbAccess dataAccess = new();
dataAccess.CreateDatabase();
var stateManager = new StateManager(dataAccess);
stateManager.Run(new MainMenuState(stateManager));