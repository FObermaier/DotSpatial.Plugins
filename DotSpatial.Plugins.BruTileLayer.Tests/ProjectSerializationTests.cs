using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using BruTile.Predefined;
using DotSpatial.Controls;
using DotSpatial.Projections;
using NUnit.Framework;
using BTL = DotSpatial.Plugins.BruTileLayer.BruTileLayer ;

namespace DotSpatial.Plugins.BruTileLayer
{
    [TestFixture]
    public class ProjectSerializationTests
    {
        protected readonly Dictionary<KnownTileSource, string> ApiKeys = new Dictionary<KnownTileSource, string>();

        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl _shell;

        private readonly AppManager _appManager = new AppManager();
        private readonly BruTileLayerPlugin _btlPlugin = new BruTileLayerPlugin();

        protected virtual void InitializeApiKeys()
        {
            // If you happen to have api keys for any of the known tile services
            // feel free to derive this class and override this method to provide them
        }
        
        private string GetApiKey(KnownTileSource kts)
        {
            string res;
            ApiKeys.TryGetValue(kts, out res);
            return res;
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            InitializeApiKeys();
            _shell = new ContainerControl();
            _appManager.Map = new Map();
            _btlPlugin.App = _appManager;
            _btlPlugin.Activate();
            //_appManager.LoadExtensions();
        }

        [Test]
        public void TestKnownTileSources()
        {
            var serManager = _appManager.SerializationManager;
            var tmpPath = System.IO.Path.GetTempFileName();
            tmpPath = System.IO.Path.ChangeExtension(tmpPath, "dspx");
            Console.WriteLine(tmpPath);

            var map = _appManager.Map;
            
            var dict = new Dictionary<KnownTileSource, bool>();
            foreach (KnownTileSource kts in Enum.GetValues(typeof(KnownTileSource)))
            {
                map.Layers.Clear();
                try
                {
                    var lyr1 = BTL.CreateKnownLayer(kts, GetApiKey(kts));
                    var lyrT = (BTL)lyr1.Clone();

                    map.Layers.Add(lyr1);
                    map.Projection = ProjectionInfo.FromAuthorityCode("EPSG", 4326);

                    map.ZoomToMaxExtent();
                    try
                    {
                        serManager.SaveProject(tmpPath);
                        serManager.OpenProject(tmpPath);
                        if (_appManager.Map.Layers.Count < 1) 
                            throw new Exception("Layer not loaded");
                        if (!(_appManager.Map.Layers[0] is BTL))
                            throw new Exception("Layer is not BruTileLayer");
                        
                        var lyr2 = (BTL)_appManager.Map.Layers[0];
                        List<string> mismatched;
                        if (lyrT.Matches(lyr2, out mismatched))
                        {
                            Console.WriteLine("Deserialized '{0}' Mismatches in", kts);
                            foreach (var mis in mismatched)
                                Console.WriteLine("- {0}", mis);
                            Console.WriteLine();
                            dict.Add(kts, false);
                        }
                        else
                        {
                            dict.Add(kts, true);
                        }


                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Deserialized '{0}' failed", kts);
                        dict.Add(kts, false);
                    }

                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to create BruTileLayer for '{0}'", kts);
                }

            }

            System.IO.File.Delete(tmpPath);
        }
    }
}
