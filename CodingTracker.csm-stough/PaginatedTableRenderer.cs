using ConsoleUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker.csm_stough
{
    public abstract class PaginatedTableRenderer
    {
        protected int currentPage;
        protected int lastPage;
        protected int resultsPerPage;
        protected string between;
        protected string low;
        protected string high;

        public PaginatedTableRenderer(int limit = int.MaxValue, int offset = 0, string between = "", string low = "", string high = "")
        {
            resultsPerPage = limit;
            currentPage = offset;
            lastPage = 10;
            this.between = between;
            this.low = low;
            this.high = high;
        }

        public virtual void DisplayTable()
        {
            Menu recordsMenu = new Menu("Records Menu");

            if (currentPage != 0)
            {
                recordsMenu.AddOption("F", "First Page...", () => { currentPage = 0; DisplayTable(); });
                recordsMenu.AddOption("P", "Previous Page...", () => { currentPage--; DisplayTable(); });
            }

            if (currentPage != lastPage)
            {
                recordsMenu.AddOption("N", "Next Page...", () => { currentPage++; DisplayTable(); });
                recordsMenu.AddOption("L", "Last Page...", () => { currentPage = lastPage; DisplayTable(); });
            }

            recordsMenu.AddOption("B", "Go Back...", () => { });

            recordsMenu.SelectOption(false);
        }

        protected abstract List<List<Object>> translateData<T>(List<T> data);

    }
}
