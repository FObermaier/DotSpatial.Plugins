using System;
using System.Collections.Generic;
using System.Reflection;
using BruTile;
using BruTile.Web;
using BruTile.Wmsc;
using BruTile.Wmts;

namespace DotSpatial.Plugins.BruTileLayer.Configuration
{
    internal static class ReflectionHelper
    {
        internal static Uri ReflectBaseUri(WmscRequest request)
        {
            var fi = typeof(WmscRequest).GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '_baseUri'", "request");

            return (Uri)fi.GetValue(request);
        }

        internal static Uri ReflectBaseUri(WmtsRequest request)
        {
            var fi = typeof(WmtsRequest).GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '_baseUri'", "request");

            return (Uri)fi.GetValue(request);
        }


        internal static IEnumerable<string> ReflectListItems(WmscRequest request, string field)
        {
            var fi = typeof(WmscRequest).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '" + field + "'", "request");

            return (IEnumerable<string>)fi.GetValue(request);
        }

        internal static IDictionary<string, string> ReflectDictionary(WmscRequest request, string field)
        {
            var fi = typeof(WmscRequest).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '" + field + "'", "request");

            return (IDictionary<string, string>)fi.GetValue(request);
        }

        internal static T Reflect<T>(TileSchema schema, string field)
        {
            var fi = typeof(TileSchema).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("TileSchema does not have a private field '" + field + "'", "field");

            return (T)fi.GetValue(schema);
        }

        internal static T Reflect<T>(WmscRequest request, string field)
        {
            var fi = typeof(WmscRequest).GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '" + field + "'", "request");

            return (T)fi.GetValue(request);
        }

        internal static T ReflectRequest<T>(WebTileProvider provider) where T : IRequest
        {
            var fi = typeof(WebTileProvider).GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Provider does not have a private field '_request'", "provider");

            return (T)fi.GetValue(provider);
        }

        public static IEnumerable<ResourceUrl> Reflect(WmtsRequest request)
        {
            var fi = typeof (WmtsRequest).GetField("_resourceUrls", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null)
                throw new ArgumentException("Request does not have a private field '_resourceUrls'", "request");
            return (IEnumerable<ResourceUrl>) fi.GetValue(request);
        }
    }
}