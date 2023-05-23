using ConsoleUtilities;

namespace CodeTracker.csm_stough
{
    public abstract class PaginatedTableRenderer
    {
        public delegate List<Object> GetPage(int limit, int offset);
        public delegate int GetCount();
        public GetPage getPage;
        public GetCount getCount;

        protected int currentPage;
        protected int lastPage;
        protected int resultsPerPage;

        public PaginatedTableRenderer(GetPage getPage, GetCount getDataCount, int limit = int.MaxValue, int offset = 0)
        {
            this.getPage = getPage;
            this.getCount = getDataCount;
            resultsPerPage = limit;
            currentPage = offset;
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

        protected abstract List<List<Object>> TranslateData<T>(List<T> data);
    }
}
