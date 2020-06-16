using System.Collections.Generic;

namespace DG.Blog.Web.Response.Base.Paged
{
    public class ListResult<T> : IListResult<T>
    {
        private IReadOnlyList<T> item;

        public IReadOnlyList<T> Item
        {
            get => item ?? (item = new List<T>());
            set => item = value;
        }

        public ListResult()
        {
        }

        public ListResult(IReadOnlyList<T> item)
        {
            Item = item;
        }
    }
}