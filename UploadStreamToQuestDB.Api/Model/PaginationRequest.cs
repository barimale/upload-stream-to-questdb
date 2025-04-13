namespace UploadStreamToQuestDB.API.Model {
    /// <summary>
    /// Represents a pagination request with page index, page size, and date range.
    /// </summary>
    public class PaginationRequest {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationRequest"/> class.
        /// </summary>
        public PaginationRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationRequest"/> class with specified parameters.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public PaginationRequest(
            int? pageIndex,
            int? pageSize,
            string? startDate,
            string? endDate) =>
            (PageIndex, PageSize, StartDate, EndDate) = (pageIndex, pageSize, startDate, endDate);

        /// <summary>
        /// Gets or sets the page index.
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public string? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public string? EndDate { get; set; }
    }
}
