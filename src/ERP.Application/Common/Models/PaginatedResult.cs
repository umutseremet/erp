﻿namespace ERP.Application.Common.Models
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedResult()
        {
        }

        public PaginatedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        }

        public static PaginatedResult<T> Success(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            return new PaginatedResult<T>(items, totalCount, pageNumber, pageSize);
        }

        public static PaginatedResult<T> Empty(int pageNumber, int pageSize)
        {
            return new PaginatedResult<T>(new List<T>(), 0, pageNumber, pageSize);
        }
    }
}