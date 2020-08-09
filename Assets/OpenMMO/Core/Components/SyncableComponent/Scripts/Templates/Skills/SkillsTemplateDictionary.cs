
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
    // SkillTemplateDictionary
    // ===================================================================================
    public partial class SkillTemplateDictionary
    {

        public readonly ReadOnlyDictionary<int, SkillTemplate> data;

        // -------------------------------------------------------------------------------
        public SkillTemplateDictionary(string folderName = "")
        {
            List<SkillTemplate> templates = Resources.LoadAll<SkillTemplate>(folderName).ToList();

            if (templates.HasDuplicates())
                DebugManager.LogWarning("[Warning] Skipped loading due to duplicate(s) in Resources subfolder: " + folderName);
            else
                data = new ReadOnlyDictionary<int, SkillTemplate>(templates.ToDictionary(x => x.hash, x => x));
        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================
