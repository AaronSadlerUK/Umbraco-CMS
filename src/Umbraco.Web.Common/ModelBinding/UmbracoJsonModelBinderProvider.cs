﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using System.Buffers;

namespace Umbraco.Web.Common.ModelBinding
{
    /// <summary>
    /// A custom body model binder that only uses a <see cref="NewtonsoftJsonInputFormatter"/> to bind body action parameters
    /// </summary>
    public class UmbracoJsonModelBinder : BodyModelBinder, IModelBinder
    {
        public UmbracoJsonModelBinder(ArrayPool<char> arrayPool, ObjectPoolProvider objectPoolProvider, IHttpRequestStreamReaderFactory readerFactory, ILoggerFactory loggerFactory)
            : base(GetNewtonsoftJsonFormatter(loggerFactory, arrayPool, objectPoolProvider), readerFactory, loggerFactory)
        {
        }

        private static IInputFormatter[] GetNewtonsoftJsonFormatter(ILoggerFactory logger, ArrayPool<char> arrayPool, ObjectPoolProvider objectPoolProvider)
        {
            var jsonOptions = new MvcNewtonsoftJsonOptions
            {
                AllowInputFormatterExceptionMessages = true
            };
            return new IInputFormatter[]
            {
                new NewtonsoftJsonInputFormatter(
                    logger.CreateLogger<UmbracoJsonModelBinder>(),
                    jsonOptions.SerializerSettings, // Just use the defaults
                    arrayPool,
                    objectPoolProvider,
                    new MvcOptions(), // The only option that NewtonsoftJsonInputFormatter uses is SuppressInputFormatterBuffering
                    jsonOptions)
            };
        }
    }
}
