﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Umbraco.Core.Hosting;
using Formatting = Newtonsoft.Json.Formatting;

namespace Umbraco.Core.Logging.Viewer
{
    public class LogViewerConfig : ILogViewerConfig
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private const string _pathToSearches = "~/Config/logviewer.searches.config.js";
        private readonly FileInfo _searchesConfig;

        public LogViewerConfig(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            var trimmedPath = _pathToSearches.TrimStart('~', '/').Replace('/', Path.DirectorySeparatorChar);
            var absolutePath = Path.Combine(_hostingEnvironment.ApplicationPhysicalPath, trimmedPath);
            _searchesConfig = new FileInfo(absolutePath);
        }

        public IReadOnlyList<SavedLogSearch> GetSavedSearches()
        {
            //Our default implementation

            //If file does not exist - lets create it with an empty array
            EnsureFileExists();

            var rawJson = System.IO.File.ReadAllText(_searchesConfig.FullName);
            return JsonConvert.DeserializeObject<SavedLogSearch[]>(rawJson);
        }

        public IReadOnlyList<SavedLogSearch> AddSavedSearch(string name, string query)
        {
            //Get the existing items
            var searches = GetSavedSearches().ToList();

            //Add the new item to the bottom of the list
            searches.Add(new SavedLogSearch { Name = name, Query = query });

            //Serialize to JSON string
            var rawJson = JsonConvert.SerializeObject(searches, Formatting.Indented);

            //If file does not exist - lets create it with an empty array
            EnsureFileExists();

            //Write it back down to file
            System.IO.File.WriteAllText(_searchesConfig.FullName, rawJson);

            //Return the updated object - so we can instantly reset the entire array from the API response
            //As opposed to push a new item into the array
            return searches;
        }

        public IReadOnlyList<SavedLogSearch> DeleteSavedSearch(string name, string query)
        {
            //Get the existing items
            var searches = GetSavedSearches().ToList();

            //Removes the search
            searches.RemoveAll(s => s.Name.Equals(name) && s.Query.Equals(query));

            //Serialize to JSON string
            var rawJson = JsonConvert.SerializeObject(searches, Formatting.Indented);

            //Write it back down to file            
            System.IO.File.WriteAllText(_searchesConfig.FullName, rawJson);

            //Return the updated object - so we can instantly reset the entire array from the API response
            return searches;
        }

        private void EnsureFileExists()
        {
            if (_searchesConfig.Exists) return;
            using (var writer = _searchesConfig.CreateText())
            {
                writer.Write("[]");
            }
        }
    }
}
