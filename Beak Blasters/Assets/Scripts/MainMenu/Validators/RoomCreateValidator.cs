using UnityEngine;

namespace MainMenu.Validators
{
    public class RoomCreateValidator : MonoBehaviour
    {
        public static bool IsVoidName(string roomName)
        {
            if (roomName == "")
                return true;

            return false;
        }
    }
}

