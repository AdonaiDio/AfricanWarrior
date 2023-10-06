using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string RelativePath, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing file for the first time!");
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    //private void writeencrypteddata<t>(t data, filestream stream)
    //{
    //    using aes aesprovider = aes.create();
    //    aesprovider.key = convert.frombase64string(key);
    //    aesprovider.iv = convert.frombase64string(iv);
    //    using icryptotransform cryptotransform = aesprovider.createencryptor();
    //    using cryptostream cryptostream = new cryptostream(
    //        stream,
    //        cryptotransform,
    //        cryptostreammode.write
    //    );

    //    // you can uncomment the below to see a generated value for the iv & key.
    //    // you can also generate your own if you wish
    //    //debug.log($"initialization vector: {convert.tobase64string(aesprovider.iv)}");
    //    //debug.log($"key: {convert.tobase64string(aesprovider.key)}");
    //    cryptostream.write(encoding.ascii.getbytes(jsonconvert.serializeobject(data)));
    //}

    //-----

    public T LoadData<T>(string RelativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. File does not exist!");
            throw new FileNotFoundException($"{path} does not exist!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    //-----

    //private t readencrypteddata<t>(string path)
    //{
    //    byte[] filebytes = file.readallbytes(path);
    //    using aes aesprovider = aes.create();

    //    aesprovider.key = convert.frombase64string(key);
    //    aesprovider.iv = convert.frombase64string(iv);

    //    using icryptotransform cryptotransform = aesprovider.createdecryptor(
    //        aesprovider.key,
    //        aesprovider.iv
    //    );
    //    using memorystream decryptionstream = new memorystream(filebytes);
    //    using cryptostream cryptostream = new cryptostream(
    //        decryptionstream,
    //        cryptotransform,
    //        cryptostreammode.read
    //    );
    //    using streamreader reader = new streamreader(cryptostream);

    //    string result = reader.readtoend();

    //    debug.log($"decrypted result (if the following is not legible, probably wrong key or iv): {result}");
    //    return jsonconvert.deserializeobject<t>(result);
    //}
    public bool DeleteData(string RelativePath)
    {
        string path = Application.persistentDataPath + RelativePath;
        if (File.Exists(path))
        {
            Debug.Log("Data exists. Deleting old file!");
            File.Delete(path);
            return true;
        }
        else
        {
            Debug.Log("Data dosen't exists.");
            return false;
        }
    }
}