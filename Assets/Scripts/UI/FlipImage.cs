using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipImage : Image
{
    public bool flipX = false;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        base.OnPopulateMesh(vh);

        if (!flipX)
            return;

        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);

        for (int i = 0; i < verts.Count; i++)
        {
            UIVertex v = verts[i];
            var uv = v.uv0;

            if (flipX)
                uv.x = 1f - uv.x;

            v.uv0 = uv;
            verts[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }
}
