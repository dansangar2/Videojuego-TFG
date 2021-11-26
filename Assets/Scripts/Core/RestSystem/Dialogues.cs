using System.Linq;
using Core.Messages;
using Core.Saves;

namespace Core.RestSystem
{
    /**<summary>Make a dialogue depends of level.</summary>*/
    public class Dialogues
    {
        /**<summary>Check if there are some new message.</summary>*/
        public static bool Added;
        
        /**<summary>Make a dialogue depends of level. It doesn't always send a message.</summary>*/
        public static TextData[] ExecuteDialogue()
        {
            TextData[] textToDo = new TextData[0];
            switch (SavesFiles.GetSave().Level)
            {
                case 1:
                    textToDo = new TextData[7];
                    textToDo[0] = new TextData("Now we're in the rest zone. There are 6 options.");
                    textToDo[0].characterId = -1;
                    textToDo[1] = new TextData("\"Rest\". finish the action with character selected and prepare" +
                                               "for the next battle. Recovery BP by the points remaining.");
                    textToDo[1].characterId = -1;
                    textToDo[2] = new TextData("\"Hospital\". finish the action. Recovery ALL, but you (the character) " +
                                               "cannot go to the next battle. If someone is defeated this action will" +
                                               " be selected automatically for it.");
                    textToDo[2].characterId = -1;
                    textToDo[3] = new TextData("\"Training\". Using the 5 points you'll level up. You'll gain experience" +
                                               " using points.\nThis time we cannot to train because we haven't enough time now.");
                    textToDo[3].characterId = -1;
                    textToDo[4] = new TextData("\"Nursing\". Using the 5 points you'll completely recover the BP and KP. " +
                                               "You'll recover BP and KP using points.");
                    textToDo[4].characterId = -1;
                    textToDo[5] = new TextData("\"Party\". You can change the order of the party. The first 3, in magenta," +
                                               "they'll fight in the next battle.");
                    textToDo[5].characterId = -1;
                    textToDo[6] = new TextData("\"Save\". Save the current data in a file.");
                    textToDo[6].characterId = -1;
                    textToDo = textToDo.Select(t => { t.speed = 0.005f; return t; }).ToArray();
                    break;
                case 3:
                    textToDo = new TextData[3];
                    textToDo[0] = new TextData("Hello, I heard that you're very strong.");
                    textToDo[0].characterId = -6;
                    textToDo[0].characterId = -6;
                    textToDo[1] = new TextData("Could we join to your party?");
                    textToDo[1].characterId = -6;
                    textToDo[2] = new TextData("Yes, of course!");
                    textToDo[2].characterId = -1;
                    SavesFiles.GetSave().AddCharacter(5);
                    break;
                case 6:
                    textToDo = new TextData[5];
                    textToDo[0] = new TextData("Hey, are you \"player 1\"?");
                    textToDo[0].characterId = -7;
                    textToDo[1] = new TextData("Yes?");
                    textToDo[1].characterId = -1;
                    textToDo[2] = new TextData("We're \"Player 7\" and \"Player 8\".");
                    textToDo[2].characterId = -7;
                    textToDo[3] = new TextData("Could we join to your group?");
                    textToDo[3].characterId = -8;
                    textToDo[4] = new TextData("Yes!");
                    textToDo[4].characterId = -1;
                    SavesFiles.GetSave().AddCharacter(6,7);
                    break;
                case 12:
                    textToDo = new TextData[2];
                    textToDo[0] = new TextData("Hello, I'm \"Player 9\". Could I join to you?");
                    textToDo[0].characterId = -9;
                    textToDo[1] = new TextData("Yes!");
                    textToDo[1].characterId = -1;
                    SavesFiles.GetSave().AddCharacter(8);
                    break;
                case 20:
                    textToDo = new TextData[3];
                    textToDo[0] = new TextData("Hello, I and my brother would like to join to your party!");
                    textToDo[0].characterId = -10;
                    textToDo[1] = new TextData("Can we join?");
                    textToDo[1].characterId = -11;
                    textToDo[2] = new TextData("Yes.");
                    textToDo[2].characterId = -1;
                    SavesFiles.GetSave().AddCharacter(9,10);
                    break;
                case 30:
                    textToDo = new TextData[3];
                    textToDo[0] = new TextData("It's too nice that you are in the level 30.");
                    textToDo[0].characterId = 0;
                    textToDo[1] = new TextData("CONGRATULATION!");
                    textToDo[1].characterId = 0;
                    textToDo[2] = new TextData("Thanks!.");
                    textToDo[2].characterId = -1;
                    break;
            }

            Added = textToDo.Length>0;
            return textToDo;
        }
    }
}