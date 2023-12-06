using UnityEngine;

public static class SaveManager
{
    public static void Save(Player player)
    {
        int posX = Mathf.RoundToInt(player.transform.position.x);
        int posY = Mathf.RoundToInt(player.transform.position.y);
        PlayerPrefs.SetInt("PosX", posX);
        PlayerPrefs.SetInt("PosY", posY);
        PlayerPrefs.Save();
    }
}