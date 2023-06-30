using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.Wolffles;

internal class CodingSession : ISession
{
	private int Id { get; set; }
	private DateTime StartDate { get; set; }
	private DateTime EndDate { get; set; }
	private string Duration { get; set; }
	private string SessionTable = "CodingSessions";

	CodingSession(int id, DateTime startDate, DateTime endDate, string duration)
	{
		Id = id;
		StartDate = startDate;
		EndDate = endDate;
		Duration = duration;
	}	
}
