using CodingTracker.Database;
using CodingTracker.Models;
using CodingTracker.Services;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace CodingTracker.DAO;

public class CodingGoalDao
{
    private readonly DatabaseContext _dbContext;

    public CodingGoalDao(DatabaseContext context)
    {
        _dbContext = context;
    }

    public int InsertNewGoal(CodingGoalModel goal)
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = @"
                    INSERT INTO tb_CodingGoals (DateCreated, DateCompleted, TargetDuration, CurrentProgress, Description, IsCompleted)
                    VALUES (@DateCreated, @DateCompleted, @TargetDuration, @CurrentProgress, @Description,@IsCompleted);
                    SELECT last_insert_rowid();";

                var id = connection.ExecuteScalar<int>(sql, goal);
                return id;  // Returns the auto-incremented Id
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error inserting new goal.", ex.Message);
            return -1;  // Indicate failure
        }
    }

    public List<CodingGoalModel> GetAllGoalRecords()
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "SELECT * FROM tb_CodingGoals";
                var goals = connection.Query<CodingGoalModel>(sql).ToList();
                return goals;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving goals", ex.Message);
            return new List<CodingGoalModel>();  // Return an empty list in case of error
        }
    }

    public bool UpdateCodingGoal(CodingGoalModel goal, SQLiteConnection connection, SQLiteTransaction transaction)
    {
        try
        {
            string sql = @"
                UPDATE tb_CodingGoals
                SET CurrentProgress = @CurrentProgress,
                    IsCompleted = @IsCompleted
                WHERE Id = @Id";

            int rowsAffected = connection.Execute(sql, goal, transaction: transaction);
            
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Error updating coding goal: ", ex.Message);
            throw;  
        }
    }

    public List<CodingGoalModel> GetInProgressCodingGoals()
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "SELECT * FROM tb_CodingGoals WHERE IsCompleted = 0";
                var goals = connection.Query<CodingGoalModel>(sql).ToList();
                return goals;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving in-progress goals", ex.Message);
            return new List<CodingGoalModel>();  // Return an empty list in case of error
        }
    }   
    
    public bool DeleteGoal(int id)
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "DELETE FROM tb_CodingGoals WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
                return true;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error deleting goal", ex.Message);
            return false;
        }
    }

    public bool DeleteAllGoals()
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "DELETE FROM tb_CodingGoals";
                int result = connection.Execute(sql);
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error deleting all goals", ex.Message);
            return false;
        }
    }
}
