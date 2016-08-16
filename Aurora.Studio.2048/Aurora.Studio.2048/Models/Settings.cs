using System;
using System.Collections.Generic;
using Com.Aurora.Shared.Helpers;

namespace Aurora.Studio._2048.Models
{
    public class Settings
    {
        public bool IsDarkMode { get; set; } = false;
        public bool IgnoreGameEnd { get; set; } = false;
        public uint HighScore { get; set; } = 0u;
        public uint Score { get; set; } = 0u;
        public uint[] Data { get; set; } = new uint[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static Settings Get()
        {
            Settings s;
            if (RoamingSettingsHelper.GetContainer("settings").ReadGroupSettings(out s))
            {

                try
                {
                    var str = (string)RoamingSettingsHelper.ReadSettingsValue("data");
                    var arr = str.Split(',');
                    for (int i = 0; i < 16; i++)
                    {
                        s.Data[i] = uint.Parse(arr[i]);
                    }
                }
                catch (System.Exception)
                {
                }
                return s;
            }
            return new Settings();
        }

        public void Save()
        {
            RoamingSettingsHelper.GetContainer("settings").WriteGroupSettings(this);
            string str = "";
            for (int i = 0; i < 16; i++)
            {
                str += Data[i].ToString() + ',';
            }
            str = str.Remove(str.Length - 1);
            RoamingSettingsHelper.WriteSettingsValue("data", str);
        }

        internal void WriteData(List<TileItem> tiles)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = 0u;
            }
            foreach (var item in tiles)
            {
                Data[item.Row * 4 + item.Col] = item.Data;
            }
        }
    }
}
