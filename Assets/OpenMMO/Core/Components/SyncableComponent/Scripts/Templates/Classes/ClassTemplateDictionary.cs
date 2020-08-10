
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
    // ClassTemplateDictionary
    // ===================================================================================
    public partial class ClassTemplateDictionary
    {

        public readonly ReadOnlyDictionary<int, ClassTemplate> data;

        // -------------------------------------------------------------------------------
        public ClassTemplateDictionary(string folderName = "")
        {
            List<ClassTemplate> templates = Resources.LoadAll<ClassTemplate>(folderName).ToList();

            if (templates.HasDuplicates())
                DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
            else
                data = new ReadOnlyDictionary<int, ClassTemplate>(templates.ToDictionary(x => x.hash, x => x));
        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================
