using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class OneSpriteInfo
{
    [ProtoMember(1)]
    public string Name { get; set; }

    [ProtoMember(2)]
    public float RectX { get; set; }

    [ProtoMember(3)]
    public float RectY { get; set; }

    [ProtoMember(4)]
    public float RectH { get; set; }

    [ProtoMember(5)]
    public float RectW { get; set; }

    [ProtoMember(6)]
    public int Alignment { get; set; }

    [ProtoMember(7)]
    public float PivotX { get; set; }

    [ProtoMember(8)]
    public float PivotY { get; set; }

    [ProtoMember(9)]
    public float BorderX { get; set; }

    [ProtoMember(10)]
    public float BorderY { get; set; }

    [ProtoMember(11)]
    public float BorderZ { get; set; }

    [ProtoMember(12)]
    public float BorderW { get; set; }

    [ProtoMember(13)]
    public int TextureIndex { get; set; }
}

[ProtoContract]
public class TpSpriteAtlasProto
{
    [ProtoMember(1)]
    public List<OneSpriteInfo> SpriteInfos { get; set; }
}