using CodingTracker.Models;

namespace CodingTracker.DataAccess;

internal interface IDataAccess
{
    public enum Order
    {
        Ascending,
        Descending
    }

    void Insert(CodingSession session);
    CodingSession? Get(int id);
    IList<CodingSession> GetAll(Order order = Order.Ascending, int skip = 0, int limit = int.MaxValue);
    int Count();
    void Update(CodingSession session);
    void Delete(int id);
    IList<CodingSession> CheckForOverlap(CodingSession session);
}
