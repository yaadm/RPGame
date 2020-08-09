//by Fhiz
using System;
using UnityEngine;
using OpenMMO;

namespace OpenMMO
{

    /// <summary>
    /// Abstract partial class BaseTemplate where most Scriptable Objects should derive from. It offers basic data like Icon, sort Order, Category and Description that is used often.
    /// </summary>
    public abstract partial class BaseTemplate : ScriptableTemplate
    {

        [Header("General Info")]
        [Tooltip("Used to categorize templates into groups (commonly used by Items, Skills, Buffs etc.)")]
        public string sortCategory;
        [Tooltip("Used to determine sort order of lists (0 = high, 1+ = lower)")]
        public int sortOrder;


        /// <summary>
        /// If the title is empty, we simply copy the object name into the title.
        /// </summary>
        /// <remarks>
        /// Note that we cannot cache the folderName right here, because it would the same for all objects of this type.
        /// </remarks>
        public override void OnValidate()
        {
            base.OnValidate();
        }

    }

}