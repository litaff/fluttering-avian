using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Godot;
using Environment = System.Environment;
using Thread = System.Threading.Thread;

// ReSharper disable once CheckNamespace
namespace RiderTestRunner
{
    // ReSharper disable once UnusedType.Global
    public partial class NetCoreRunner : Node // for GodotXUnit use: public partial class Runner : GodotTestRunner. https://github.com/fledware/GodotXUnit/issues/8#issuecomment-929849478
    {
        private string runnerAssemblyPath;
    
        public override void _Ready()
        {
            var textNode = GetNode<RichTextLabel>("Control/RichTextLabel");
        
            foreach (var arg in OS.GetCmdlineArgs())
            {
                textNode.Text += Environment.NewLine + arg;
            }

            if (OS.GetCmdlineArgs().Length < 4) return;
            
            var unitTestArgs = OS.GetCmdlineArgs()[4].Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            runnerAssemblyPath = OS.GetCmdlineArgs()[2];
            
            var runnerLoadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
            runnerLoadContext?.LoadFromAssemblyPath(runnerAssemblyPath);
            
            if(runnerLoadContext == null) return;
        
            runnerLoadContext.Resolving += CurrentDomainOnAssemblyResolve;
            AssemblyLoadContext.Default.Resolving += CurrentDomainOnAssemblyResolve;

            var thread = new Thread(() =>
            {
                AppDomain.CurrentDomain.ExecuteAssembly(runnerAssemblyPath, unitTestArgs);
                GetTree().Quit();
            });
            thread.Start();
        }

        private Assembly CurrentDomainOnAssemblyResolve(AssemblyLoadContext loadContext, AssemblyName assemblyName)
        { 
            // not sure, if this is needed
            var alreadyLoadedMatch = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(loadedAssembly =>
            {
                var name = loadedAssembly.GetName().Name;
                return name != null &&
                       name.Equals(assemblyName.Name);
            });
            
            if (alreadyLoadedMatch != null)
            {
                return alreadyLoadedMatch;
            }
            
            var dir = new FileInfo(runnerAssemblyPath).Directory;
            if (dir == null) return null;
            var file = new FileInfo(Path.Combine(dir.FullName, $"{assemblyName.Name}.dll"));
            return file.Exists ? loadContext.LoadFromAssemblyPath(file.FullName) : null;
        }
    }
}
