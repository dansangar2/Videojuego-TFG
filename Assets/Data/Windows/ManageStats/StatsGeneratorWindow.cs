using System.Globalization;
using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    public static class StatsGeneratorWindow
    {
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };

        #region TABLE

        public static void Table(string[][] content, GUILayoutOption[] options = null)
        {
            options ??= Options;
            EditorGUILayout.BeginVertical("Box");
            foreach (string[] cont in content)
            {
                EditorGUILayout.BeginHorizontal();
                foreach (string c in cont)
                {
                    GUILayout.Label(" | ");
                    GUILayout.Label(c, options);
                }
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region CHARACTER

        #region Set

        public static void GenerateStats(Character chara, GUILayoutOption[] options = null)
        {
            options ??= Options;

            float[] bases = chara.Bases;
            int[] plus = chara.Plus;
            float[] rate = chara.Rate;
            float[] learning = chara.Learning;
            int[] flat = chara.Flat;
            float[] max = chara.Max;
            bool[] yes = chara.Yes;
            int[] expData = chara.ExpData;

            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("Stats", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Base", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Plus", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Rate", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Flat", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Learning", options);
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            for (int i = 0; i < flat.Length; i++)
            { 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(" | ");
                switch (i)
                {
                    case 0: GUILayout.Label("MBP:", options);
                        break;
                    case 1: GUILayout.Label("MKP:", options);
                        break;
                    case 2: GUILayout.Label("ATK:", options);
                        break;
                    case 3: GUILayout.Label("DEF:", options);
                        break;
                    case 4: GUILayout.Label("SPI:", options);
                        break;
                    case 5: GUILayout.Label("MEN:", options);
                        break;
                    case 6: GUILayout.Label("AGI:", options);
                        break;
                }
                GUILayout.Label(" | ");
                bases[i] = EditorGUILayout.FloatField(bases[i], options);
                GUILayout.Label(" | ");
                plus[i] = EditorGUILayout.IntField(plus[i], options);
                GUILayout.Label(" | ");
                rate[i] = EditorGUILayout.FloatField(rate[i], options);
                GUILayout.Label(" | ");
                flat[i] = EditorGUILayout.IntField(flat[i], options);
                GUILayout.Label(" | ");
                learning[i] = EditorGUILayout.FloatField(learning[i], options);
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            for (int i = 7; i < flat.Length + max.Length; i++)
            { 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(" | ");
                switch (i)
                {
                    case 7: GUILayout.Label("REB:", options);
                        break;
                    case 8: GUILayout.Label("REK:", options);
                        break;
                }
                GUILayout.Label(" | ");
                bases[i] = EditorGUILayout.FloatField(bases[i], options);
                GUILayout.Label(" | ");
                plus[i] = EditorGUILayout.IntField(plus[i], options);
                GUILayout.Label(" | ");
                rate[i] = EditorGUILayout.FloatField(rate[i], options);
                GUILayout.Label(" | ");
                GUILayout.Label("Max:", options);
                GUILayout.Label(" | ");
                max[i-7] = EditorGUILayout.FloatField(max[i-7], options);
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            for (int i = 9; i < flat.Length + max.Length + yes.Length; i++)
            { 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(" | ");
                switch (i)
                {
                    case 9: GUILayout.Label("RXB:", options);
                        break;
                    case 10: GUILayout.Label("RXK:", options);
                        break;
                }
                GUILayout.Label(" | ");
                bases[i] = EditorGUILayout.FloatField(bases[i], options);
                GUILayout.Label(" | ");
                plus[i] = EditorGUILayout.IntField(plus[i], options);
                GUILayout.Label(" | ");
                rate[i] = EditorGUILayout.FloatField(rate[i], options);
                GUILayout.Label(" | ");
                GUILayout.Label("Yes?:", options);
                GUILayout.Label(" | ");
                yes[i-9] = EditorGUILayout.Toggle(yes[i-9], options);
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label(" | "); 
            GUILayout.Label("EXP:", options);
            for (int i = 0; i < expData.Length; i++)
            {
                GUILayout.Label(" | "); 
                expData[i] = EditorGUILayout.IntField(expData[i], options);
            }
            GUILayout.Label(" | ");
            if (GUILayout.Button("Help!", options))
            {
                StatsCharacterDefinition.Window();
            }
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            
            chara.SetAll(bases, plus, rate, flat, learning, max, yes, expData);

            EditorGUILayout.EndVertical();
        }

        #endregion
        
        #region Validator

        public static bool Validator(Character chara)
        {
            float[] bases = chara.Bases;
            int[] plus = chara.Plus;
            float[] rate = chara.Rate;
            float[] learning = chara.Learning;
            int[] flat = chara.Flat;
            float[] max = chara.Max;
            int[] expData = chara.ExpData;
            
            bool res = false;
            string message = "";

            for (int i = 0; i < bases.Length; i++)
            {
                res = res || bases[i] <= 0 || plus[i] < 0 || rate[i] <= 0;
            }
            if(res) message += "One or more parameter is equals(base/rate) or less(base/rate/plus) of 0, check it and change it by a number upper of 0.\n";
            res = false;
            
            foreach (float t in learning)
            {
                res = res || t > 1.2 || t < -0.8;
            }
            if (res) message += "One or more learning rates isn´t between -0.8 and 1.2, change it by one number between these values.\n";
            res = false;

            foreach (int t in flat)
            {
                res = res || t < 0;
            }
            if(res) message += "One or more flat parameter is equals or less of 0, check it and change it by a number upper of 0.\n";
            res = false;
            
            foreach (float t in max)
            {
                res = res || t < 0;
            }
            
            if(res) message += "One or more flat parameter is less of 0, check it and change it by a number upper of 0.\n";
            res = false;
            
            foreach (int t in expData)
            {
                res = res || t < 0;
            }
            
            if(res) message += "One or more exp parameter is less of 0, check it and change it by a number upper of 0.\n";

            if (!message.Equals("")) res = true;
            
            GUILayout.Label(message);

            return res;
        }

        #endregion

        #region Dispaly Stats

        public static void Display(Character character, GUILayoutOption[] options = null)
        {
            
            Character dummy = new Character(character);
            options ??= Options;
            Character[] characters = new Character[10];
            
            string[][] content =
            {
                new[] {"LV ","","","","","","","","","",""},
                new[] {"MBP","","","","","","","","","",""},
                new[] {"MKP","","","","","","","","","",""},
                new[] {"ATK","","","","","","","","","",""},
                new[] {"DEF","","","","","","","","","",""},
                new[] {"SPI","","","","","","","","","",""},
                new[] {"MEN","","","","","","","","","",""},
                new[] {"AGI","","","","","","","","","",""},
                new[] {"REB","","","","","","","","","",""},
                new[] {"REK","","","","","","","","","",""},
                new[] {"RXB","","","","","","","","","",""},
                new[] {"RXK","","","","","","","","","",""},
                new[] {"EXP","","","","","","","","","",""}
            };
            
            int levelInterval = dummy.MaxLevel / 9;
            Character newChar;
            int i = 1, j = 0;
            bool cont = true;

            while (cont)
            {
                if (i >= dummy.MaxLevel || j==9)
                {
                    i = character.MaxLevel;
                    cont = false;
                }
                
                dummy.Level = i;
                dummy.MaxLevel = character.MaxLevel;
                newChar = new Character( dummy);
                characters[j] = newChar;
                i += levelInterval;
                j++;
            }

            for (i = 1; i <=characters.Length; i++)
            {
                content[0][i] = characters[i-1].Level.ToString();
                content[1][i] = characters[i-1].MaxBloodPoints.ToString();
                content[2][i] = characters[i-1].MaxKarmaPoints.ToString();
                content[3][i] = characters[i-1].Attack.ToString();
                content[4][i] = characters[i-1].Defense.ToString();
                content[5][i] = characters[i-1].Spirit.ToString();
                content[6][i] = characters[i-1].Mentality.ToString();
                content[7][i] = characters[i-1].Agility.ToString();
                content[8][i] = characters[i-1].BloodRecoveryPlus*100 + "%";
                content[9][i] = characters[i-1].KarmaRecoveryPlus*100 + "%";
                content[10][i] = characters[i-1].Regeneration*100 + "%";
                content[11][i] = characters[i-1].KarmaRegeneration*100 + "%";
                content[12][i] = characters[i-1].NedExp.ToString();
            }
            
            EditorGUILayout.BeginHorizontal();
            Table(content, options);
            EditorGUILayout.EndHorizontal();
        }

#endregion

        #endregion
        
        #region ABILITY

        #region Set

        public static void GenerateStats(Ability item, GUILayoutOption[] options = null)
        {
            options ??= Options;

            float[] bases = item.Bases;
            float[] rate = item.Rate;
            float[] learning = item.Learning;
            int[] expData = item.ExpData;

            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("Stats", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Base", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Rate", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Learning", options);
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            for (int i = 0; i < bases.Length; i++)
            { 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(" | ");
                switch (i)
                {
                    case 0: GUILayout.Label("Power increment:", options);
                        break;
                    case 1: GUILayout.Label("Down Interval:", options);
                        break;
                    case 2: GUILayout.Label("Up Interval:", options);
                        break;
                }
                GUILayout.Label(" | ");
                bases[i] = EditorGUILayout.FloatField(bases[i], options);
                GUILayout.Label(" | ");
                rate[i] = EditorGUILayout.FloatField(rate[i], options);
                GUILayout.Label(" | ");
                learning[i] = EditorGUILayout.FloatField(learning[i], options);
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.BeginHorizontal(); 
            GUILayout.Label(" | "); 
            GUILayout.Label("EXP:", options);
            for (int i = 0; i < expData.Length; i++)
            {
                GUILayout.Label(" | "); 
                expData[i] = EditorGUILayout.IntField(expData[i], options);
            }
            GUILayout.Label(" | ");
            if (GUILayout.Button("Help!", options))
            {
                StatsAbilityDefinition.Window();
            }
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();

            item.SetAll(bases, rate, learning, expData, 1);

            EditorGUILayout.EndVertical();
        }
        

        #endregion

        #region Validator

        public static bool Validator(Ability item)
        {
            float[] bases = item.Bases;
            float[] rate = item.Rate;
            float[] learning = item.Learning;
            int[] expData = item.ExpData;
            
            bool res = false;
            string message = "";

            for (int i = 0; i < bases.Length; i++)
            {
                res = res || bases[i] <= 0 || rate[i] <= 0;
            }
            if(res) message += "One or more parameter is equals(base/rate) or less(base/rate/plus) of 0, check it and change it by a number upper of 0.\n";
            res = false;
            
            foreach (float t in learning)
            {
                res = res || t > 1.2 || t < -0.8;
            }
            if (res) message += "One or more learning rates isn´t between -0.8 and 1.2, change it by one number between these values.\n";
            res = false;

            foreach (int t in expData)
            {
                res = res || t < 0;
            }
            if(res) message += "One or more exp parameter is less of 0, check it and change it by a number upper of 0.\n";

            if (!message.Equals("")) res = true;
            
            GUILayout.Label(message);

            return res;
        }

        #endregion

        #region Display

        public static void Display(Ability item, GUILayoutOption[] options = null)
        {
            
            Ability dummy = new Ability(item);
            options ??= Options;
            Ability[] characters = new Ability[6];
            int[] levels = new int[6];
            
            string[][] content =
            {
                new[] {"LV ","","","","","",""},
                new[] {"PIC","","","","","",""},
                new[] {"DIV","","","","","",""},
                new[] {"UIV","","","","","",""},
                new[] {"EXP","","","","","",""}
            };
            
            int levelInterval = dummy.MaxLevel / 5;
            Ability item2;
            int i = 1, j = 0;
            bool cont = true;

            while (cont)
            {
                if (i >= dummy.MaxLevel || j==5)
                {
                    i = item.MaxLevel;
                    cont = false;
                }
                dummy.MaxLevel = item.MaxLevel;
                item2 = new Ability(dummy, i);
                characters[j] = item2;
                levels[j] = i;
                i += levelInterval;
                j++;
            }

            for (i = 1; i <=characters.Length; i++)
            {
                content[0][i] = levels[i-1].ToString();
                content[1][i] = characters[i-1].PowerIncrement.ToString(CultureInfo.InvariantCulture);
                content[2][i] = characters[i-1].DownInterval.ToString(CultureInfo.InvariantCulture);
                content[3][i] = characters[i-1].UpperInterval.ToString(CultureInfo.InvariantCulture);
                content[4][i] = characters[i-1].NedExp.ToString();
            }
            
            EditorGUILayout.BeginHorizontal();
            Table(content, options);
            EditorGUILayout.EndHorizontal();
        }

        #endregion
        
        #endregion
        
    }
}