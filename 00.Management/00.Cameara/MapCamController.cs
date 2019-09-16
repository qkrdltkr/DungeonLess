using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamController : IUpdateableObject {
    public string[] _Layers;
    protected Camera _camera;

    protected override void Initialize() {          
       _camera = this.GetComponent<Camera>();
        _camera.cullingMask = LayerMask.GetMask(_Layers);
    }
}