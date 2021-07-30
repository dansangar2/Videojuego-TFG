using Entities;
using UnityEditor;
using UnityEngine;

namespace Data.Windows.ManageStats
{
    public static class StatsGeneratorWindow
    {
        private static readonly GUILayoutOption[] Options = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(20f) };

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

        //=============================================================================
        
        public static void CharacterStats(Character chara, GUILayoutOption[] options = null)
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
                StatsDefinition.Window();
            }
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            
            chara.SetAll(bases, plus, rate, flat, learning, max, yes, expData);

            EditorGUILayout.EndVertical();
        }
        
        //=============================================================================
        
        /*public static void AbilityStats(Ability ability, GUILayoutOption[] options = null)
        {
            
            options ??= Options;

            int[] user = ability.GetStatsArray();
            int[] target = ability.GetStatsArrayTarget();
            int[] expValues = ability.GetExperienceValues();
            float[] interval = ability.GetRangeOfAttack();

            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("Base: ", options);
            GUILayout.Label(" | ");
            //ability.SetBaseAttack(EditorGUILayout.IntField(ability.GetBaseAttack(), options));
            int baseAttack = EditorGUILayout.IntField(ability.GetBaseAttack(), options);
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("User stats in consideration", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Target stats in consideration", options);
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            
            EditorGUILayout.BeginVertical("Box");
            for (int i = 0; i < user.Length; i++)
            { 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(" | ");
                switch (i)
                {
                    case 0:
                        GUILayout.Label("MBP: ", options);
                        break;
                    case 1:
                        GUILayout.Label("MKP: ", options);
                        break;
                    case 2:
                        GUILayout.Label("ATK: ", options);
                        break;
                    case 3:
                        GUILayout.Label("DEF: ", options);
                        break;
                    case 4:
                        GUILayout.Label("SPI: ", options);
                        break;
                    case 5:
                        GUILayout.Label("MEN: ", options);
                        break;
                    case 6:
                        GUILayout.Label("AGI: ", options);
                        break;
                    case 7:
                        GUILayout.Label("ABP: ", options);
                        break;
                    case 8:
                        GUILayout.Label("AKP: ", options);
                        break;
                    case 9:
                        GUILayout.Label("KG : ", options);
                        break;
                    case 10:
                        GUILayout.Label("CHA: ", options);
                        break;
                    case 11:
                        GUILayout.Label("NXT: ", options);
                        break;
                }
                user[i] = EditorGUILayout.IntField(user[i], options);
                GUILayout.Label(" | ");
                target[i] = EditorGUILayout.IntField(target[i], options);
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("Exp. Values:", options);
            GUILayout.Label(" | ");
            for (int i = 0; i < expValues.Length; i++)
            { 
                expValues[i] = EditorGUILayout.IntField(expValues[i], options);
                GUILayout.Label(" | ");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("Range of attack Damage: ");
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label(" | ");
            GUILayout.Label("Low:", options);
            interval[0] = EditorGUILayout.FloatField(interval[0], options);
            GUILayout.Label(" | ");
            GUILayout.Label("Up:", options);
            interval[1] = EditorGUILayout.FloatField(interval[1], options);
            GUILayout.Label(" | ");
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            
            ability.SetAttacksStats(baseAttack, user, target, interval[0], interval[1]);
            ability.SetExperienceCurveParameters(expValues);
            
            EditorGUILayout.EndVertical();
        }*/
        
        //=============================================================================

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
        
        //=============================================================================

        /*public static bool Validator(Ability ability)
        {
            int[] user = ability.GetStatsArray();
            int[] target = ability.GetStatsArrayTarget();
            int[] expVal = ability.GetExperienceValues();
            
            bool res = ability.GetBaseAttack()>1000000 || ability.GetBaseAttack()<-1000000;
            string message = "";

            if (res) message += "Base attack most be between 1000000 and -1000000\n";
            res = false;

            for (int i = 0; i < user.Length; i++)
            {
                res = res || user[i] < -100 || user[i] > 100 || target[i] < -100 || target[i] > 100;
            }

            if (res)
                message += "One or more parameter is less than -100 or more than 100.\nThe values most be between 100 and -100";
            res = false;
            
            foreach (int t in expVal)
            {
                res = res || t < 0;
            }
            
            if(res) message += "One or more exp parameter is less of 0, check it and change it by a number upper of 0.\n";

            res = ability.GetRangeOfAttack()[0] > ability.GetRangeOfAttack()[1];

            if(res) message += "Low range value must be lower than up value.\n";

            res = ability.GetRangeOfAttack()[0] <= 0 || ability.GetRangeOfAttack()[1] <= 0;
            
            if(res) message += "Range values must be upper than 0.\n";
            
            if (!message.Equals("")) res = true;
            
            GUILayout.Label(message);

            return res;
        }*/
        
        //=============================================================================
        
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
                //dummy.SetCharacterLevel(i, character.GetMaxLevel());
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
        
        /*public static void AbilityStatsDisplay(Ability ability, GUILayoutOption[] options = null)
        {
            
            options ??= Options;

            int[] user = ability.GetStatsArray();
            int[] target = ability.GetStatsArrayTarget();
            int[] expValues = ability.GetExperienceValues();
            float[] interval = ability.GetRangeOfAttack();

            EditorGUILayout.BeginVertical("Box");
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("Base: ", options);
            GUILayout.Label(" | ");
            GUILayout.Label(ability.GetBaseAttack().ToString(), options);
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("User stats in consideration", options);
            GUILayout.Label(" | ");
            GUILayout.Label("Target stats in consideration", options);
            GUILayout.Label(" | ");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            
            EditorGUILayout.BeginVertical("Box");
            for (int i = 0; i < user.Length; i++)
            { 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(" | ");
                switch (i)
                {
                    case 0:
                        GUILayout.Label("MBP: ", options);
                        break;
                    case 1:
                        GUILayout.Label("MKP: ", options);
                        break;
                    case 2:
                        GUILayout.Label("ATK: ", options);
                        break;
                    case 3:
                        GUILayout.Label("DEF: ", options);
                        break;
                    case 4:
                        GUILayout.Label("SPI: ", options);
                        break;
                    case 5:
                        GUILayout.Label("MEN: ", options);
                        break;
                    case 6:
                        GUILayout.Label("AGI: ", options);
                        break;
                    case 7:
                        GUILayout.Label("ABP: ", options);
                        break;
                    case 8:
                        GUILayout.Label("AKP: ", options);
                        break;
                    case 9:
                        GUILayout.Label("KG : ", options);
                        break;
                    case 10:
                        GUILayout.Label("CHA: ", options);
                        break;
                    case 11:
                        GUILayout.Label("NXT: ", options);
                        break;
                }
                GUILayout.Label(user[i].ToString(), options);
                GUILayout.Label(" | ");
                GUILayout.Label(target[i].ToString(), options);
                GUILayout.Label(" | ");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(" | ");
            GUILayout.Label("Exp. Values:", options);
            GUILayout.Label(" | ");
            foreach (int t in expValues)
            {
                GUILayout.Label(t.ToString(), options);
                GUILayout.Label(" | ");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("Range of attack Damage: ");
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Label(" | ");
            GUILayout.Label("Low:", options);
            GUILayout.Label(interval[0].ToString(CultureInfo.InvariantCulture), options);
            GUILayout.Label(" | ");
            GUILayout.Label("Up:", options);
            GUILayout.Label(interval[1].ToString(CultureInfo.InvariantCulture), options);
            GUILayout.Label(" | ");
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndVertical();
        }*/
    }
}