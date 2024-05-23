using CodingTracker.ConsoleInteraction;
using CodingTracker.Controllers;
using CodingTracker.DataRepository;
using CodingTracker.IDataRepository;
using CodingTracker.Services;

DataConfig dataConfig = new();
IUserInteraction userInteraction= new UserInteraction();
ICodingSessionRepository codingSessionRepository= new CodingSessionRepository(userInteraction);
ICodingSessionService codingSessionService= new CodingSessionService(codingSessionRepository);
CodingSessionController codingSessionController= new(userInteraction, codingSessionService, dataConfig);

codingSessionController.Run();