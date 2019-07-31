using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baza.Models
{
    public interface ILinksService
    {
        Task<List<Links>> GetPaginatedResult(int currentPage, int pageSize = 10);
        Task<int> GetCount();
    }

    public class LinksService : ILinksService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public LinksService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<Links>> GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            var data = await GetData();
            return data.OrderBy(d => d.LinkId).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetCount()
        {
            var data = await GetData();
            return data.Count;
        }

        private async Task<List<Links>> GetData()
        {
            var json = await File.ReadAllTextAsync(Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "paging.json"));
            return JsonConvert.DeserializeObject<List<Links>>(json);
        }
    }
}
