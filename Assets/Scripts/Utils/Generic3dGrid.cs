using System;
using System.Collections.Generic;
using UnityEngine;

public class Generic3dGrid<T> where T : class
{
    [SerializeField] Vector3Int size;
    
    private Dictionary<Vector3Int,T> _dict = new Dictionary<Vector3Int,T>();
    private Dictionary<T,Vector3Int> _reversedDict = new Dictionary<T, Vector3Int>();

    public Generic3dGrid(Vector3Int size){
        this.size = size;
    }

    public T this[Vector3Int vect]
    {
        get { return _dict[vect];}
        set {
            if(_dict.ContainsKey(vect)){
                throw new GridConflict(vect);
            }

            // remove ref to previous vect
            if(_reversedDict.ContainsKey(value)){
                Vector3Int previousVect = _reversedDict[value];
                _dict.Remove(previousVect);
            }

            _dict[vect] = value;
            _reversedDict[value] = vect;
        }
    }

    public T this[int x, int y, int z]
    {
        get => this[new Vector3Int(x, y, z)];
        set => this[new Vector3Int(x, y, z)] = value;
    }

    public T Get(Vector3Int vector){
        if(_dict.ContainsKey(vector)){
            return this[vector];
        }
        return default(T);
    }

    public T Get(int x, int y, int z) => Get(new Vector3Int(x, y, z));

    public Vector3Int GetPosition(T t){
        return _reversedDict[t];
    }

    public void Remove(T t){
        Vector3Int position = GetPosition(t);
        _reversedDict.Remove(t);
        _dict.Remove(position);
    }
    public bool ContainsKey(Vector3Int vect) => _dict.ContainsKey(vect);
}

public class GridConflict : System.Exception{
    public GridConflict(Vector3Int position) : 
        base(ModifyMessage(position))
    {
    }

    private static string ModifyMessage(Vector3Int position)
    {
        return "There is already an object at " + position.ToString();
    }
}