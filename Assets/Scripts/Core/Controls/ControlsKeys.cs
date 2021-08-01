using UnityEngine;

namespace Core.Controls
{
    /**<summary>The controls of the game is there.</summary>*/
    public static class ControlsKeys
    {
        public static KeyCode MoveUp { get; set; } = KeyCode.UpArrow;
        public static KeyCode MoveLeft { get; set; } = KeyCode.LeftArrow;
        public static KeyCode MoveRight { get; set; } = KeyCode.RightArrow;
        public static KeyCode MoveDown { get; set; } = KeyCode.DownArrow;
        public static KeyCode Ok { get; set; } = KeyCode.A;
        public static KeyCode Back{ get; set; } = KeyCode.X;
        public static KeyCode ActionButton1 { get; set; } = KeyCode.S;
        public static KeyCode ActionButton2 { get; set; } = KeyCode.Z;
        public static KeyCode CameraLeft { get; set; } = KeyCode.Q;
        public static KeyCode CameraRight { get; set; } = KeyCode.W;
        public static KeyCode Pause { get; set; } = KeyCode.Return;

        public static bool DirectionalKeyIsDown()
        {
            return Input.GetKeyDown(MoveDown) 
                   || Input.GetKeyDown(MoveLeft) 
                   || Input.GetKeyDown(MoveRight) 
                   || Input.GetKeyDown(MoveUp);
        }

        public static KeyCode? GetKey(int index)
        {
            switch (index)
            {
                case 0:
                    return MoveUp;
                case 1:
                    return MoveLeft;
                case 2:
                    return MoveRight;
                case 3:
                    return MoveDown;
                case 4:
                    return Ok;
                case 5:
                    return Back;
                case 6:
                    return Pause;
                case 7:
                    return ActionButton1;
                case 8:
                    return ActionButton2;
                case 9:
                    return CameraLeft;
                case 10:
                    return CameraRight;
            }

            return null;
        }
        
        public static void SetKey(int index, KeyCode key)
        {
            int? used = InUseBy(key);
            if(used==index) return;
            KeyCode sKey = KeyCode.None;
            switch (index)
            {
                case 0:
                    sKey = MoveUp;
                    MoveUp = key;
                    break;
                case 1:
                    sKey = MoveLeft;
                    MoveLeft = key;
                    break;
                case 2:
                    sKey = MoveRight;
                    MoveRight = key;
                    break;
                case 3:
                    sKey = MoveDown;
                    MoveDown = key;
                    break;
                case 4:
                    sKey = Ok;
                    Ok = key;
                    break;
                case 5:
                    sKey = Back;
                    Back = key;
                    break;
                case 6:
                    sKey = Pause;
                    Pause = key;
                    break;
                case 7:
                    sKey = ActionButton1;
                    ActionButton1 = key;
                    break;
                case 8:
                    sKey = ActionButton2;
                    ActionButton2 = key;
                    break;
                case 9:
                    sKey = CameraLeft;
                    CameraLeft = key;
                    break;
                case 10:
                    sKey = CameraRight;
                    CameraRight = key;
                    break;
            }
            if(used!=null)  SetKey((int) used, sKey);
        }

        public static int? InUseBy(KeyCode key)
        {
            if(key.Equals(MoveUp)) return 0;
            if(key.Equals(MoveLeft)) return 1;
            if(key.Equals(MoveRight)) return 2;
            if(key.Equals(MoveDown)) return 3;
            if(key.Equals(Ok)) return 4;
            if(key.Equals(Back)) return 5;
            if (key.Equals(Pause)) return 6;
            if(key.Equals(ActionButton1)) return 7;
            if (key.Equals(ActionButton2)) return 8;
            if(key.Equals(CameraLeft)) return 7;
            if (key.Equals(CameraRight)) return 8;
            return null;
        }
        
        public static bool InUse(KeyCode key)
        {
            return key.Equals(MoveUp) 
                   || key.Equals(MoveLeft)
                   || key.Equals(MoveRight)
                   || key.Equals(MoveDown)
                   || key.Equals(Ok)
                   || key.Equals(Back)
                   || key.Equals(Pause)
                   || key.Equals(ActionButton1)
                   || key.Equals(ActionButton2)
                   || key.Equals(CameraLeft)
                   || key.Equals(CameraRight);
        }
        
        public static bool InUse(int index)
        {
            return GetKey(index) == null;
        }
    }
}