
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // ClassTemplate
    // ===================================================================================
    [CreateAssetMenu(fileName = "New Class", menuName = "OpenMMO - Templates/New Class", order = 999)]
    public partial class ClassTemplate : IterateableTemplate
    {

        public List<SkillTemplate> skillsList;

        public string clothesPrefabNameDefault;

        public string prefabDefaultWeaponName;

        // -------------------------------------------------------------------------------

        public static string _folderName = "";

        static ClassTemplateDictionary _data;

        // -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
        public static ReadOnlyDictionary<int, ClassTemplate> data
        {
            get
            {
                ClassTemplate.BuildCache();
                return _data.data;
            }
        }

        // -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
        public static void BuildCache(bool forced = false)
        {
            if (_data == null || forced)
                _data = new ClassTemplateDictionary(ClassTemplate._folderName);
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