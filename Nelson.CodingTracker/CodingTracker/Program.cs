using CodingTracker.ConsoleInteraction;
using CodingTracker.Controllers;
using CodingTracker.DataRepository;
using CodingTracker.IDataRepository;
using CodingTracker.Services;
using CodingTracker.Utilities;

DataConfig dataConfig = new();
IUserInteraction userInteraction= new UserInteraction();
ICodingSessionRepository codingSessionRepository= new CodingSessionRepository(userInteraction);
IUtils utils = new Utils(userInteraction);
ICodingSessionService codingSessionService= new CodingSessionService(codingSessionRepository, utils);
CodingSessionController codingSessionController= new(userInteraction, codingSessionService, dataConfig);

codingSessionController.Run();