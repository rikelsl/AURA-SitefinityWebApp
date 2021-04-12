using System;

namespace icpas_store.Models
{
    public class BaseSearchViewModel
    {
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int Id { get; set; }
        public string SearchText { get; set; }


        public bool IsKeywordSearch
        {
            get { return !string.IsNullOrWhiteSpace(SearchText); }
        }

        public bool IsDateSearch
        {
            get { return StartDate.HasValue | EndDate.HasValue; }
        }
    }
}