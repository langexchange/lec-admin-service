using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LE.AdminService.Dtos
{
    public class PagingDto<T> where T : class
    {
        public PagingDto(int total, List<T> items)
        {
            Total = total;
            Items = items;
        }

        public int Total { get; set; }
        public List<T> Items { get; set; }
    }
}
