
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // SkillTemplate
    // ===================================================================================
    public abstract partial class SkillTemplate : BaseTemplate
    {

        public static string _folderName = "";

        static SkillTemplateDictionary _data;

        [Header("Skill Info")]
        public float cooldown;

        // base amount of damage / healing
        public int baseImpact;

        public List<ClassTemplate> availableForClasses;

        public string animationName;

        // -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
        public static ReadOnlyDictionary<int, SkillTemplate> data
        {
            get
            {
                SkillTemplate.BuildCache();
                return _data.data;
            }
        }

        // -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
        public static void BuildCache(bool forced = false)
        {
            if (_data == null || forced)
                _data = new SkillTemplateDictionary(SkillTemplate._folderName);
        }

        // -------------------------------------------------------------------------------
        // OnEnable
        // -------------------------------------------------------------------------------
        public void OnEnable()
        {
            if (_folderName != folderName)
                _folderName = folderName;

            _data = null;

        }

        // -------------------------------------------------------------------------------
        // OnValidate
        // You can add custom validation checks here
        // -------------------------------------------------------------------------------
        public override void OnValidate()
        {
            base.OnValidate();
        }

        // -------------------------------------------------------------------------------

    }

}

// =======================================================================================