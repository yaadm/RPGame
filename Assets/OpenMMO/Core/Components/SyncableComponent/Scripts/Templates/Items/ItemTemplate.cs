
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // ItemTemplate
    // ===================================================================================
    public abstract partial class ItemTemplate : BaseTemplate
    {

        public static string _folderName = "";

        static ItemsTemplateDictionary _data;

        [Tooltip("Icon used to visualize this template (Item icon, Buff icon etc.)")]
        public Sprite smallIcon;
        [Tooltip("Background of icon used to visualize this template (background will be visible on icons with transparency)")]
        public Sprite backgroundIcon;
        [Tooltip("Rarity of the template (commonly used by Items, Currencies, Equipment etc.)")]
        public RarityTemplate rarity;

        [Tooltip("Description of the template used as part of it's tooltip")]
        [TextArea(15, 20)]
        public string description;

        [Tooltip("3D World representation of the item")]
        public GameObject prefab;

        public Itemtype itemtype;

        // -------------------------------------------------------------------------------
        // data
        // -------------------------------------------------------------------------------
        public static ReadOnlyDictionary<int, ItemTemplate> data
        {
            get
            {
                ItemTemplate.BuildCache();
                return _data.data;
            }
        }

        // -------------------------------------------------------------------------------
        // BuildCache
        // -------------------------------------------------------------------------------
        public static void BuildCache(bool forced = false)
        {
            if (_data == null || forced)
                _data = new ItemsTemplateDictionary(ItemTemplate._folderName);
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