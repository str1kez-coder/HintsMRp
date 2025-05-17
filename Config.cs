using System.ComponentModel;
using System.Collections.Generic;
using Exiled.API.Interfaces;

namespace HintsMRp
{
    public class Config : IConfig
    {
        [Description("Plugin enable?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Show debug message?")]
        public bool Debug { get; set; } = false;

        [Description("Hint Duration (seconds)")]
        public float HintDuration { get; set; } = 15f;

        [Description("Color for hints")]
        public string TextColor { get; set; } = "#FFA500"; 

        [Description("Hints Size")]
        public int TextSize { get; set; } = 18;

        [Description("interval for appearing hints for tutorials/observers")]
        public float ObserverInterval { get; set; } = 30f;

        [Description("interval for appearing hints for SCP/People")]
        public float PlayerInterval { get; set; } = 60f;

        [Description("Hints for tutorials/observers")]
        public List<string> ObserverHints { get; set; } = new List<string>
        {
            " hint 0",
            " hint 1"
        };

        [Description("Hints for SCP/People")]
        public List<string> PlayerHints { get; set; } = new List<string>
        {
            " hint 01",
            " hint 02"
        };
    }
}