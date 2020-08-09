
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

    // ===================================================================================
    // BasicSwordTemplate
    // ===================================================================================
    [CreateAssetMenu(fileName = "New BasicSword", menuName = "OpenMMO - Items/New BasicSword", order = 999)]
    public partial class BasicSwordTemplate : ItemTemplate
    {
        public void Awake()
        {
            // when creating new asset, set its default type. (so we wond need to do it in Unity Editor)
            itemtype = Itemtype.Default;
        }

    }

}

// =======================================================================================