﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Umbraco.Core.WebAssets
{
    /// <summary>
    /// Used for bundling and minifying web assets at runtime
    /// </summary>
    public interface IRuntimeMinifier
    {
        /// <summary>
        /// Returns the cache buster value
        /// </summary>
        string CacheBuster { get; }

        /// <summary>
        /// Creates a css bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="filePaths"></param>
        void CreateCssBundle(string bundleName, params string[] filePaths);

        /// <summary>
        /// Renders the html link tag for the bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns>
        /// An html encoded string
        /// </returns>
        string RenderCssHere(string bundleName);

        /// <summary>
        /// Creates a JS bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="filePaths"></param>
        void CreateJsBundle(string bundleName, params string[] filePaths);

        /// <summary>
        /// Renders the html script tag for the bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns>
        /// An html encoded string
        /// </returns>
        string RenderJsHere(string bundleName);

        /// <summary>
        /// Returns the asset paths for the bundle name
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns>
        /// If debug mode is enabled this will return all asset paths (not bundled), else it will return a bundle URL
        /// </returns>
        Task<IEnumerable<string>> GetAssetPathsAsync(string bundleName);

        Task<string> MinifyAsync(string fileContent, AssetType assetType);

        void Reset();

    }
}
