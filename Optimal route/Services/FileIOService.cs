using Newtonsoft.Json;
using Optimal_route.Models;
using System.ComponentModel;
using System.IO;

namespace Optimal_route.Services
{
    class FileIOService
    {
        private readonly string _filePath;

        public FileIOService(string path)
        {
            _filePath = path;
        }

        public async Task<BindingList<Attraction>> LoadDataAsync()
        {
            var fileExists = File.Exists(_filePath);
            if (!fileExists)
            {
                using (var file = File.CreateText(_filePath)) { }
                return new BindingList<Attraction>();
            }
            using (var reader = File.OpenText(_filePath))
            {
                var fileText = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<BindingList<Attraction>>(fileText);
            }
        }

        public async Task SaveDataAsync(object todoDataList)
        {
            using (var writer = File.CreateText(_filePath))
            {
                var output = JsonConvert.SerializeObject(todoDataList);
                await writer.WriteAsync(output);
            }
        }
    }
}