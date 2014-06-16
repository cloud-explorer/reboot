using System;
using System.Collections.Generic;
using System.Linq;
using Projects.Common.Core;
using Projects.Models.Glass;
using Projects.Reboot.Common;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.IDTables;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;

namespace Projects.Reboot.Core.Commands
{
    public class DiscoverModels : Command
    {
        

        public override void Execute(CommandContext context)
        {
            IEnumerable<Type> allGlassBasedModels = null;
            allGlassBasedModels = ReflectionHelper.GetAllTypesThatImplement<GlassBase>(
                new List<string>
                    {
                        "Projects.Common.Models", 
                        "Projects.Reboot.Models"
                    }
                );
            Database database = Factory.GetDatabase("master");
            Item modelFolder = database.GetItem(RebootConstants.ModelRootId);
            foreach (var glassModel in allGlassBasedModels.Where(t => !t.IsAbstract))
            {
                Log.Info(string.Format("Adding model {0} to Sitecore", glassModel.FullName), context);
                string displayName = glassModel.Name;
                string name = ItemUtil.ProposeValidItemName(glassModel.FullName.Replace(".", "_"));
                string prefix = RebootConstants.ModelTemplateId.ToString();
                string key = name;
                IDTableEntry idTableEntry = IDTable.GetID(prefix, key);
                if (idTableEntry != null) continue;
                Item model = modelFolder.Add(name, new TemplateID(RebootConstants.ModelTemplateId));
                if (model == null) continue;
                using (new SecurityDisabler())
                {
                    model.Editing.BeginEdit();
                    try
                    {
                        model.Appearance.DisplayName = displayName;
                        model["Model Type"] = glassModel.AssemblyQualifiedName;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("An error occured while editing Model " + displayName, context);
                        model.Editing.CancelEdit();
                    }
                    finally
                    {
                        model.Editing.EndEdit();
                    }
                }
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            return context.Items[0].ID.Equals(RebootConstants.ModelRootId)
                       ? CommandState.Enabled
                       : CommandState.Hidden;
        }
    }
}