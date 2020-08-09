
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using OpenMMO;
using OpenMMO.Debugging;

namespace OpenMMO
{

    // ===================================================================================
    // ItemsTemplateDictionary
    // ===================================================================================
    public partial class ItemsTemplateDictionary
    {

        public readonly ReadOnlyDictionary<int, ItemTemplate> data;

        // -------------------------------------------------------------------------------
        public ItemsTemplateDictionary(string folderName = "")
        {
            List<ItemTemplate> templates = Resources.LoadAll<ItemTemplate>(folderName).ToList();

            if (templates.HasDuplicates())
                DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
            else
                data = new ReadOnlyDictionary<int, ItemTemplate>(templates.ToDictionary(x => x.hash, x => x));
        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================
