namespace PI.WebGarten.Pipeline
{
    using System;
    using System.Net;

    /// <summary>
    /// This class supports the filters Pipeline. Through this class filters can be registred to run in each resuqst pipeline.
    /// Each filter must implement <see cref="IHttpFilter"/>
    /// </summary>
    public partial class HttpFilterPipeline
    {
        FilterCollection _filtersCollection = new FilterCollection();

        public HttpFilterPipeline(IHttpFilter lastFilter)
        {
            this._filtersCollection.SetTerminatorFilter(lastFilter);
        }

        /// <summary>
        /// Adds the filter in the first position in the Pipeline. 
        /// </summary>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <remarks>If a filter with the same name already exists, it is removed.</remarks>
        public void AddFilterFirst(string filterName, Type filterType)
        {
            var filter = CreateFilter(filterName, filterType);
            this._filtersCollection.AddFirst(filter);
        }

        /// <summary>
        /// Adds the filter with <paramref name="filterType"/> in the last position in the Pipeline. 
        /// </summary>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <remarks>If a filter with the same name already exists, it is removed.</remarks>
        public void AddFilterLast(string filterName, Type filterType)
        {
            var filter = CreateFilter(filterName, filterType);
            this._filtersCollection.AddLast(filter);
        }

        /// <summary>
        /// Adds the filter with <paramref name="filterType"/> before the filter with <paramref name="nextFilterName"/> in the Pipeline. 
        /// </summary>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="nextFilterName">The name of the next filter.</param>
        /// <remarks>If a filter with the same name already exists, it is removed.</remarks>
        public void AddFilterBefore(string filterName, Type filterType, string nextFilterName)
        {
            var filter = CreateFilter(filterName, filterType);
            this._filtersCollection.AddBefore(filter, nextFilterName);
        }


        /// <summary>
        /// Adds the filter with <paramref name="filterType"/> after the filter with <paramref name="previousFilterName"/> in the Pipeline. 
        /// </summary>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="previousFilterName">The name of the next filter.</param>
        /// <remarks>If a filter with the same name already exists, it is removed.</remarks>
        public void AddFilterAfter(string filterName, Type filterType, string previousFilterName)
        {
            var filter = CreateFilter(filterName, filterType);
            this._filtersCollection.AddAfter(filter, previousFilterName);
        }



        /// <summary>
        /// Executes the specified Pipeline with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal HttpResponse Execute(HttpListenerContext context)
        {
            return this._filtersCollection.GetFilterChain().Process(context);

        }

        private static IHttpFilter CreateFilter(string filterName, Type filterType)
        {
            if (String.IsNullOrEmpty(filterName))
            {
                throw new ArgumentException("Filters must have a name", "filterName");
            }

            if (typeof(IHttpFilter).IsAssignableFrom(filterType) == false)
            {
                throw new ArgumentException("The filter type does not implement IHttpFilter", "filterType");
            }

            IHttpFilter filter = (IHttpFilter)Activator.CreateInstance(filterType, new object[] { filterName });
            return filter;
        }
    }
}