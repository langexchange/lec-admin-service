using System;
using System.Collections.Generic;

namespace LE.AdminService.Models.Responses
{
    public class PageListItemResponse<T> where T : class
    {
        private int _pageSize;

        public PageListItemResponse(List<T> items, int total, int pageSize = 20)
        {
            Items = items;
            Total = total;
            _pageSize = pageSize;
        }

        public List<T> Items { get; set; }
        public int Total { get; set; }
        public int TotalPages
        {
            get
            {
                if (Total == 0 || _pageSize == 0)
                    return 0;

                return (int)Math.Ceiling((decimal)Total / _pageSize);
            }
        }
    }
}
