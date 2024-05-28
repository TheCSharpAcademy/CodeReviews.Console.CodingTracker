using CodingTracker.ConsoleInteraction;
using CodingTracker.Controllers;
using CodingTracker.DataRepository;
using CodingTracker.IDataRepository;
using CodingTracker.Services;
using CodingTracker.Utilities;

DataConfig dataConfig = new();
IUserInteraction userInteraction= new UserInteraction();
IUtils utils = new Utils(userInteraction);
ICodingSessionRepository codingSessionRepository= new CodingSessionRepository(userInteraction, utils);
ICodingSessionService codingSessionService= new CodingSessionService(codingSessionRepository, utils, userInteraction);
CodingSessionController codingSessionController= new(userInteraction, codingSessionService, dataConfig);

codingSessionController.Run();