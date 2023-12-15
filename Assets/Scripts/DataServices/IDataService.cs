using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    public void saveData<T>(string relativePath, T data, bool encrypt);

    public T loadData<T>(string relativePath, bool encrypt);
}
