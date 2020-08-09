//BY DX4D
using UnityEngine;

namespace OpenMMO
{
    /// <summary>Input Axis are set from Edit>PlayerSettings>Input</summary>
    public enum InputAxis
    {
        /// <summary>Horizontal Input axis. Corresponds to Left/Right Arrows or A and D Keys.</summary>
        [Tooltip("Horizontal Input axis. Corresponds to Left/Right Arrows or A and D Keys.")]
        Horizontal,
        /// <summary>Vertical Input axis. Corresponds to Up/Down Arrows or W and S Keys.</summary>
        [Tooltip("Vertical Input axis. Corresponds to Up/Down Arrows or W and S Keys.")]
        Vertical,
        /// <summary>Right Trigger axis. Corresponds to Left Ctrl and Left Mouse Button.</summary>
        [Tooltip("Right Trigger axis. Corresponds to Left Ctrl and Left Mouse Button.")]
        Fire1,
        /// <summary>Left Trigger axis. Corresponds to Left Alt and Right Mouse Button.</summary>
        [Tooltip("Left Trigger axis. Corresponds to Left Alt and Right Mouse Button.")]
        Fire2,
        /// <summary>Corresponds to Left Shift and Middle Mouse Button.</summary>
        [Tooltip("Corresponds to Left Shift and Middle Mouse Button.")]
        Fire3,
        /// <summary>Action Button. Corresponds to Enter Key and Gamepad Button 0.</summary>
        [Tooltip("Action Button. Corresponds to Enter Key and Gamepad Button 0.")]
        Submit,
        /// <summary>Cancel Button. Corresponds to Escape Key and Gamepad Button 1.</summary>
        [Tooltip("Cancel Button. Corresponds to Escape Key and Gamepad Button 1.")]
        Cancel
    }

    [CreateAssetMenu(menuName = "OpenMMO/Controls/Player Movement Config")]
    public class PlayerControlConfig : ScriptableObject
    {

        [Header("Movement Input Keys")]
        public KeyCode runKey = KeyCode.Slash;

        //MOVE
        public InputAxis moveAxisHorizontal = InputAxis.Horizontal;
        public InputAxis moveAxisVertical = InputAxis.Vertical;
        public KeyCode jumpKey = KeyCode.Space;

        public KeyCode targetKey = KeyCode.Q;

        [Header("Move Speed")]
        [Tooltip("How fast you can move.")]
        [Range(0, 10)] public float moveSpeedMultiplier = 1.0f;

        [Tooltip("How fast you can jump.")]
        [Range(0, 10)] public float jumpSpeedMultiplier = 3.0f;

        [Header("Move Speed Scale")]
        //WALK
        [Tooltip("Scales speed while walking. 1.0f = normal speed")]
        [Range(0, 10)] public float walkSpeedScale = 1.0f;
        //RUN
        [Tooltip("Scales speed while running. 1.0f = normal speed")]
        [Range(0, 10)] public float runSpeedScale = 1.5f;
        //JUMP
        [Tooltip("Scales speed while running. 1.0f = normal speed")]
        [Range(0, 10)] public float jumpSpeedScale = 2f;

        [Header("Skills Input Keys")]
        public KeyCode skillbarSlot_0 = KeyCode.Keypad1;

    }
}