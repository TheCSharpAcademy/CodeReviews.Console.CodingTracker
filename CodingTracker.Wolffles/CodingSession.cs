using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.Wolffles;

internal class CodingSession : ISession
{
	public int Id { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public TimeSpan Duration { get; set; }

	public CodingSession(int id, DateTime startDate, DateTime endDate, TimeSpan duration)
	{
		Id = id;
		StartDate = startDate;
		EndDate = endDate;
		Duration = duration;
	}	
}
