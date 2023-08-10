namespace CodingTracker.MartinL_no.Models;

internal interface ICodingGoalRepository
{
    public List<CodingGoal> GetCodingGoals();

    public CodingGoal GetCodingGoal(int id);

    public bool InsertCodingGoal(CodingGoal codingGoal);
}
