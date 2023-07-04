using CodingTracker.alvaromosconi.Data;
using CodingTracker.alvaromosconi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi
{
    internal class CodingController
    {
        private CodeSessionLocalStorage cs = new CodeSessionLocalStorage();
        internal void SaveSession(DateTime start, DateTime end)
        {
            CodeSessionModel session = new CodeSessionModel
            {
                StartDateTime = start,
                EndDateTime = end
            };

            cs.SaveSession(session);
        }
    }
}
