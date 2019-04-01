using System.Collections.Generic;

namespace WebClient.Core.Paging
{
    /// <summary>
    /// Paging request
    /// </summary>
    public class PagingRequest<TFilter> where TFilter : class
    {
        /// <summary>
        /// Draw counter
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Paging first record indicator. Default: 0
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// An array defining all columns in the table
        /// </summary>
        public IEnumerable<Column> Columns { get; set; }

        /// <summary>
        /// Is an array defining how many columns are being ordered
        /// </summary>
        public IEnumerable<Order> Order { get; set; }

        /// <summary>
        /// filter
        /// </summary>
        public TFilter Filter { get; set; }
    }

    /// <summary>
    /// Define a order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Column to which ordering should be applied
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Ordering direction for this column. It will be asc or desc to indicate ascending ordering or descending ordering, respectively.
        /// </summary>
        public string Dir { get; set; }
    }

    public class Search
    {
        /// <summary>
        /// Search value to apply to this specific column.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Flag to indicate if the search term for this column should be treated as regular expression (true) or not (false). As with global search, normally server-side processing scripts will not perform regular expression searching for performance reasons on large data sets, but it is technically possible and at the discretion of your script.
        /// </summary>
        public bool Regex { get; set; }

    }

    /// <summary>
    /// Define a column in the table
    /// </summary>
    public class Column
    {
        public string Name { get; set; }
        public string Data { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public Search Search { get; set; }
    }
}