using System;
using System.Reflection;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Projections;

namespace DotSpatial.Plugins.BruTileLayer
{
    public class BruTileLayerPlugin : Extension
    {
        private const string BtlRoot = "BtlRoot";
        
        private Configuration.Forms.BruTileLayerDialog _bruTileLayerDialog;

        internal static readonly BruTileLayerSettings Settings = new BruTileLayerSettings();
        //private readonly TypeNameManager _typeNameManager;

        public BruTileLayerPlugin()
        {
            //_typeNameManager = new TypeNameManager(Assembly.GetExecutingAssembly());
            var asm = Assembly.GetAssembly(typeof (BruTile.Web.BingMapType));
            System.Diagnostics.Debug.Assert(!asm.IsDynamic);
            System.Diagnostics.Debug.Assert(asm.IsFullyTrusted);
            asm = Assembly.GetExecutingAssembly();

            System.Diagnostics.Debug.Assert(!asm.IsDynamic);
            System.Diagnostics.Debug.Assert(asm.IsFullyTrusted);

            AppDomain.CurrentDomain.AssemblyResolve += HandleAssemblyResolve;
            //AppDomain.CurrentDomain.TypeResolve += HandleTypeResolve;
        }

        private Assembly _assembly;
        private Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var asm = _assembly ?? Assembly.GetExecutingAssembly();
            _assembly = asm;
            if (args.Name == asm.FullName.Split(',')[0])
            {
                return asm;
            }
            return null;
        }

        public override void Activate()
        {
            App.SerializationManager.Serializing += HandleSerializing;
            App.SerializationManager.Deserializing += HandleDeserializing;
            //App.SerializationManager.QueryType += HandleQueryType;
            //AppDomain.CurrentDomain.Load(Assembly.GetExecutingAssembly().GetName());
            AddMenus(App.HeaderControl);
            base.Activate();
        }

        /*
        private void HandleQueryType(object sender, QueryTypeEventArgs e)
        {
            // If type was found elsewere, go ahead!
            if (e.QueriedType != null)
                return;

            Console.WriteLine("Query for type {0}", e.TypeName);
            var t = Type.GetType(e.TypeName);
            if (t != null)
            {
                e.QueriedType = t;
                return;
            }

            var typeName = _typeNameManager.UpdateTypename(e.TypeName);
            t = Type.GetType(typeName);
            e.QueriedType = t;
        }
        */
        private void HandleDeserializing(object sender, SerializingEventArgs e)
        {
            var manager = sender as SerializationManager;
            if (manager == null)
                throw new InvalidOperationException("No serialization manager");

            Settings.Deserialize(manager);
        }

        private void HandleSerializing(object sender, SerializingEventArgs e)
        {
            var manager = sender as SerializationManager;
            if (manager == null)
                throw new InvalidOperationException("No serialization manager");

            Settings.Serialize(manager);
        }

        private void AddMenus(IHeaderControl headerControl)
        {
            var sai = new SimpleActionItem("Add BruTileLayer ...", OnInvokeBruTileLayerDialog);
            //sai.LargeImage = Properties.Resources.BruTileLogoBig;
            //sai.SmallImage = Properties.Resources.BruTileLogoSmall;
            sai.Caption = "Add BruTileLayer ...";
            sai.GroupCaption = "Add Layer";
            //sai.ShowInQuickAccessToolbar = true;
            headerControl.Add(sai);
        }

        private void OnInvokeBruTileLayerDialog(object sender, EventArgs e)
        {
            using (var frm = new Configuration.Forms.BruTileLayerDialog())
            {
                frm.Map = App.Map;
                frm.ShowDialog();
            }
        }

        public override void Deactivate()
        {
            if (_bruTileLayerDialog != null)
            {
                _bruTileLayerDialog.Close();
                _bruTileLayerDialog = null;
            }

            App.SerializationManager.Serializing -= HandleSerializing;
            App.SerializationManager.Deserializing -= HandleDeserializing;
            //App.SerializationManager.QueryType -= HandleQueryType;
            RemoveMenus(App.HeaderControl);

            base.Deactivate();

        }

        private void RemoveMenus(IHeaderControl headerControl)
        {
            headerControl.Remove(BtlRoot);
        }
    }
}
