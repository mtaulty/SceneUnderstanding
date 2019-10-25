using System.Linq;
using UnityEngine;

public class LocalPositionMemoryBehaviour : MonoBehaviour
{
    void Start()
    {        
        if (PlayerPrefs.HasKey(this.gameObject.name))
        {
            var value = PlayerPrefs.GetString(this.gameObject.name);
            Debug.Log($"MT: Read SRT to string of {value}");
            this.StringToLocalSRT(value);
        }
    }
    public void OnManipulationEnded()
    {
        // Store away our local position, rotation and scale in settings type storage.
        var srtToString = this.LocalSRTToString();

        Debug.Log($"MT: Written out SRT to string of {srtToString}");

        PlayerPrefs.SetString(this.gameObject.name, srtToString);
        PlayerPrefs.Save();
    }
    string LocalSRTToString()
    {
        var t = this.gameObject.transform.localPosition;
        var s = this.gameObject.transform.localScale;
        var r = this.gameObject.transform.localRotation;

        return ($"{Vector3ToString(s)} {QuaternionToString(r)} {Vector3ToString(t)}");
    }
    void StringToLocalSRT(string value)
    {
        var pieces = value.Split(' ').Select(s => float.Parse(s)).ToArray();
        this.gameObject.transform.localScale = Vector3FromStrings(pieces, 0);
        this.gameObject.transform.localRotation = QuaternionFromStrings(pieces, 3);
        this.gameObject.transform.localPosition = Vector3FromStrings(pieces, 7);
    }
    static Quaternion QuaternionFromStrings(float[] pieces, int v) => new Quaternion(pieces[v], pieces[v+1], pieces[v+2], pieces[v+3]);
    static Vector3 Vector3FromStrings(float[] pieces, int v) => new Vector3(pieces[v], pieces[v+1], pieces[v+2]);
    static string Vector3ToString(Vector3 v) => $"{v.x} {v.y} {v.z}";
    static string QuaternionToString(Quaternion q) => $"{q.x} {q.y} {q.z} {q.w}";
}
