using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Application.Core
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage,int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new
            {
                currentPage,
                totalItems,
                itemsPerPage,
                totalPages
            };
            response.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}