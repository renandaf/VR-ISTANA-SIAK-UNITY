using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        string fullPath = Path.Combine(dataDirPath,profileId,dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //load data dari sebuah file
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //ubah data dari json ke gamedata
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when load data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data, string profileId)
    {
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            //buat direktori jika belum ada
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //ubah gamedata menjadi json
            string dataToStore = JsonUtility.ToJson(data, true);

            //masukkan json kedalam file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId) 
    {
        string fullPath = Path.Combine(dataDirPath, profileId);
        Directory.Delete(fullPath,true);
        Debug.Log("Save Removed");
    }

    public Dictionary<string,GameData> LoadAllProfile()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //loop semua folder yang ada di folder save
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        IEnumerable<DirectoryInfo> dirInfosSort = dirInfos.OrderBy(f => f.LastWriteTime);
        foreach (DirectoryInfo dirInfo in dirInfosSort)
        {
            string profileId = dirInfo.Name;

            //cek apakah folder merupakan folder untuk save data
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                continue;
            }
            GameData profileData = Load(profileId);
            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
        }
        return profileDictionary;
    }
}
