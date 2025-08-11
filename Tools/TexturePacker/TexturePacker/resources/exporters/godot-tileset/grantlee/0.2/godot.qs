
var exportSprites = function(sprites, writePivotPoints, trimSpriteNames)
{
    var result = [];
    for (var i = 0; i < sprites.length; i++)
    {
        var s = sprites[i];
        var exportNode = {
            filename: trimSpriteNames ? s.trimmedName : s.fullName,
            region: {
                x:s.frameRect.x,
                y:s.frameRect.y,
                w:s.frameRect.width,
                h:s.frameRect.height
            },
            margin: {
                x:s.sourceRect.x,
                y:s.sourceRect.y,
                w:s.untrimmedSize.width-s.frameRect.width,
                h:s.untrimmedSize.height-s.frameRect.height
            },
        };
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
            size: {
                w: t.size.width,
                h: t.size.height
            },
            sprites: exportSprites(t.allSprites, root.settings.writePivotPoints, root.settings.trimSpriteNames)
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
            version: "1.0",
            smartupdate: root.smartUpdateKey
        },
    };
    return JSON.stringify(doc, null, "\t");
}
exportData.filterName = "exportData";
Library.addFilter("exportData");

