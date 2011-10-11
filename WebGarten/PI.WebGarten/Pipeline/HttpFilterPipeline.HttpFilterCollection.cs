namespace PI.WebGarten.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class HttpFilterPipeline
    {
        internal class FilterCollection
        {
            LinkedList<IHttpFilter> _filtersCollection = new LinkedList<IHttpFilter>();
            private IHttpFilter _terminatorFilter;

            private IHttpFilter _filterChainStart = null;

            public void AddFirst(IHttpFilter filter)
            {
                this.RemoveFilterIfExistsAndInvalidateFilterChain(filter);
                this._filtersCollection.AddFirst(filter);
            }

            public void AddLast(IHttpFilter filter)
            {
                this.RemoveFilterIfExistsAndInvalidateFilterChain(filter);
                this._filtersCollection.AddLast(filter);
            }

            public void AddBefore(IHttpFilter filter, string nextFilterName)
            {
                this.RemoveFilterIfExistsAndInvalidateFilterChain(filter);
                LinkedListNode<IHttpFilter> referenceFilter = GetFilterByName(nextFilterName);
                this._filtersCollection.AddBefore(referenceFilter, filter);
            }

            public void AddAfter(IHttpFilter filter, string previousFilterName)
            {
                this.RemoveFilterIfExistsAndInvalidateFilterChain(filter);
                LinkedListNode<IHttpFilter> referenceFilter = GetFilterByName(previousFilterName);
                this._filtersCollection.AddAfter(referenceFilter, filter);
            }

            internal IHttpFilter GetFilterChain()
            {
                if (this._filterChainStart == null)
                {
                    LinkedListNode<IHttpFilter> currentNode = _filtersCollection.First;
                    while(currentNode != null && currentNode.Next != null)
                    {
                        currentNode.Value.SetNextFilter(currentNode.Next.Value);
                        currentNode = currentNode.Next;
                    }

                    if(_filtersCollection.Last != null) {
                        // Set terminatorFilter as the _filtersCollection.Last nest
                        _filterChainStart = _filtersCollection.First.Value;
                        _filtersCollection.Last.Value.SetNextFilter(_terminatorFilter);
                    }  
                    else {
                        _filterChainStart = _terminatorFilter;
                    }
                }

                return _filterChainStart;
            }


            internal void SetTerminatorFilter(IHttpFilter terminatorFilter)
            {
                if (_terminatorFilter != null)
                {
                    throw new ArgumentException("Already exists a termination filter", "lastFilter");
                }
                _terminatorFilter = terminatorFilter;
                this._filterChainStart = null;
            }

            private void RemoveFilterIfExistsAndInvalidateFilterChain(IHttpFilter filter)
            {
                var existantFilter = GetFilterByName(filter.Name);
                if (existantFilter != null)
                {
                    this._filtersCollection.Remove(existantFilter);
                }

                this._filterChainStart = null;
            }

            private LinkedListNode<IHttpFilter> GetFilterByName(string filterName)
            {

                //// TODO: This code is so ugly that I hope nobody sees it. Two searches in the list to get the node? 
                //// Shame on the LinkedList. Its interface made me to write this crap.
                //// Have to come here after to solve this mess. For now... It stays like this! I'm really ashamed of this code
                LinkedListNode<IHttpFilter> filterNode = _filtersCollection.Find(_filtersCollection.SingleOrDefault(f => f.Name == filterName));
                return filterNode;
            }
        }

    }
}