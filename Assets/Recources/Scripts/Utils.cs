using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public delegate void SimpleCallback();
public static class Utils {
	public static void Shuffle<T>(ref T[] objects) {
		for (int i = objects.Length - 1; i > 0; i--) {
			int r = Random.Range(0, i + 1);
			T t = objects[r];
			objects[r] = objects[i];
			objects[i] = t;
		}
	}

    public static void Shuffle<T>(List<T> objects) {
        for (int i = objects.Count - 1; i > 0; i--) {
            int r = Random.Range(0, i + 1);
            T t = objects[r];
            objects[r] = objects[i];
            objects[i] = t;
        }
    }
}
