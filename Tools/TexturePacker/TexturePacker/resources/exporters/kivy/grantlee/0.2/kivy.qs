
var exportSprites = function(sprites, trimSpriteNames)
{
    var result = {};
    for (var i = 0; i < sprites.length; i++)
    {
        var s = sprites[i];
        var spriteName = s.trimmedName;
        spriteName = spriteName.replace(/[\\\/]/g, "_");
        var frame = [ s.frameRect.x, s.frameRect.y, s.frameRect.width, s.frameRect.height ];

        result[spriteName] = frame;
    }
    return result;
}


var exportTextures = function(root)
{
    var textures = root.allResults[root.variantIndex].textures;
    var result = {};

    for (var i = 0; i < textures.length; i++)
    {
        var textureName = textures[i].fullName;
        var sprites = exportSprites(textures[i].allSprites)

        result[textureName] = sprites;
    }
    return result;
}


var exportData = function(root)
{
    var doc = exportTextures(root);
    return JSON.stringify(doc, null, "\t");
}
exportData.filterName = "exportData";
Library.addFilter("exportData");

