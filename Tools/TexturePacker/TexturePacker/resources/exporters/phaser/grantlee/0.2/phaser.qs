
var exportSprites = function(sprites, writePivotPoints, trimSpriteNames)
{
    var result = [];
    for (var i = 0; i < sprites.length; i++)
    {
        var s = sprites[i];
        var exportNode = {
            filename: trimSpriteNames ? s.trimmedName : s.fullName,
            rotated: s.rotated,
            trimmed: s.trimmed,
            sourceSize: { w:s.untrimmedSize.width, h:s.untrimmedSize.height },
            spriteSourceSize: { x:s.sourceRect.x, y:s.sourceRect.y, w:s.sourceRect.width, h:s.sourceRect.height }
        };
        if(s.rotated)
        {
            exportNode.frame = { x:s.frameRect.x, y:s.frameRect.y, w:s.frameRect.height, h:s.frameRect.width};
        }
        else
        {
            exportNode.frame = { x:s.frameRect.x, y:s.frameRect.y, w:s.frameRect.width, h:s.frameRect.height};
        }
        if (s.pivotPointNorm && writePivotPoints)
        {
            exportNode.anchor = { x:s.pivotPointNorm.x, y:s.pivotPointNorm.y };
        }
        result.push(exportNode);
    }
    return result;
}


var exportTexture = function(root)
{
    var textures = root.allResults[root.variantIndex].textures;
    var result = [];

    for (var i = 0; i < textures.length; i++)
    {
        var t = textures[i];
        var exportNode = {
            image: t.fullName,
            normalMap: (t.normalMapFileName === "") ? undefined : t.normalMapFileName,
            format: root.settings.outputFormat,
            size: {
                w: t.size.width,
                h: t.size.height
            },
            scale: root.variantParams.scale,
            frames: exportSprites(t.allSprites, root.settings.writePivotPoints, root.settings.trimSpriteNames)
        };

        result.push(exportNode);
    }
    return result;
}


var exportData = function(root)
{
    var doc = {
        textures: exportTexture(root),
        meta: {
            app: "https://www.codeandweb.com/texturepacker",
            version: "3.0",
            smartupdate: root.smartUpdateKey
        },
    };
    return JSON.stringify(doc, null, "\t");
}
exportData.filterName = "exportData";
Library.addFilter("exportData");

