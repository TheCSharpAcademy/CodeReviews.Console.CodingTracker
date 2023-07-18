using System;
using System.Linq;

namespace CodingTracker.Wolffles;

internal class CodingSession : ISession
{
	public int Id { get; set; }
	public DateTime StartDate 
	{
		get;
		set;
	}
	public DateTime EndDate
	{
		get;
		set;
	}
	public TimeSpan Duration { get; set; }

	public CodingSession(int id, DateTime startDate, DateTime endDate)
	{
		Id = id;
		StartDate = startDate;
		EndDate = endDate;
		CalculateDuration();
	}	

	public void CalculateDuration()
	{
		Duration = EndDate - StartDate;
	}
}
