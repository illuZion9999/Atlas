using dnlib.DotNet;

namespace Atlas.Renamer.Analysis.Analysers
{
    //Rename the app's resources as well,
    //it will break the app if we don't.
    //Plus it won't expose the original names...
    class WinFormsAnalyser : IAnalyser
    {
        public void Analyse(IDnlibDef def, RenamerContext ctx)
        {
            if (!(def is TypeDef type) || type.BaseType is null) return;
            if (type.BaseType.FullName != "System.Windows.Forms.Form") return;
            
            //We definitely have a Form class that could have a resource linked to it
            //that has to be renamed as well.
            var resource = FindResource(type);
            if (resource is null) return;

            //Link the form to its resource, so they will get the same name.
            ctx.Link(type, resource);
        }

        static IMDTokenProvider FindResource(IMemberRef member)
        {
            var mod = member.Module;
            return mod.Resources.FindEmbeddedResource(member.FullName + ".resources");
        }
    }
}