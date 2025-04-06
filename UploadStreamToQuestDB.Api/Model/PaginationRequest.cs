namespace UploadStreamToQuestDB.API.Model {
    public class PaginationRequest {
        public PaginationRequest() {
            // intentionally left blank
        }

        public PaginationRequest(
            int pageIndex,
            int pageSize,
            string startDate,
            string endDate) {
            PageIndex = pageIndex;
            PageSize = pageSize;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int? PageIndex { get; set; } = null;
        public int? PageSize { get; set; } = null;
        public string? StartDate { get; set; } = null;
        public string? EndDate { get; set; } = null;
    }
}
