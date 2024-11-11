using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileDataWritter 
{
    public string saveDataDirectoryPath = "";
    public string saveDataFilePath = "";

    public bool CheckToSeeIfFileExists(){
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveDataFilePath)))
            return true;
        else
            return false;
    }

    public void DeleteSaveFile(){
        File.Delete(Path.Combine(saveDataDirectoryPath, saveDataFilePath));
    }

    // 创建一个保存文件
    public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData){
        
        string savePath = Path.Combine(saveDataDirectoryPath, saveDataFilePath);

        try{
            // 如果不存在的话，创建将要写入文件数据的上级文件夹
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            // 将c#数据转化为json数据
            string dataToStore = JsonUtility.ToJson(characterSaveData);

            // 将文件写入计算机
            using(FileStream stream = new FileStream(savePath, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception ex){
            Debug.Log("发生了错误，游戏未被保存，路径： " + savePath +"/n" + ex);
        }
    }


    // 加载一个保存数据的文件
    public CharacterSaveData LoadSaveFile(){
        CharacterSaveData characterSaveData = null;
        string loadPath = Path.Combine(saveDataDirectoryPath, saveDataFilePath);
        try{
            if(File.Exists(loadPath)){
            string dataToLoad;
            using(FileStream stream = new FileStream(loadPath, FileMode.Open)){
                using(StreamReader reader = new StreamReader(stream)){
                    dataToLoad = reader.ReadToEnd();
                }
            }

            characterSaveData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
        }
        catch(Exception ex){
            Debug.LogError(ex);
        }

        return characterSaveData;
    }
}
