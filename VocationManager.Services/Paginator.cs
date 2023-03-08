using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocationManager.Services
{
    public class Paginator
    {
        public Paginator(int totalItems, int? page, int? pageSize, string mainPath, bool includeSearchBar)
        {
            // calculate total, start and end pages
            var actualPageSize = pageSize ?? 10;

            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)actualPageSize);
            var currentPage = page != null 
                ? (int)page 
                : 1;
            currentPage = currentPage <= 0
                ? 1
                : currentPage;

            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            IncludeSearchBar = includeSearchBar;
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = actualPageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            MainPath = mainPath;
        }

        public bool IncludeSearchBar { get; set; }
        public int TotalItems{ get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }
        public string MainPath { get; private set; }
    }
}
