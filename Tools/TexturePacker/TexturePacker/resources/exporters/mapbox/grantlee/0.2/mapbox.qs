
var exportSprites = function(root)
{
    scaleFactor = root.scaleFactor;
    var result = {};

    var ratio = root.texture.fullName.replace(/.*@(\d+)x.*/, '$1');
    ratio = (ratio == root.texture.fullName) ? "1" : ratio;

    for (var i = 0; i < root.allSprites.length; i++)
    {
        var s = root.allSprites[i];

        result[s.trimmedName] = {
            x:s.frameRect.x,
            y:s.frameRect.y,
            width:s.frameRect.width,
            height:s.frameRect.height,
            pixelRatio: ratio
        }
    }
    return result;
}

var exportData = function(root)
{
    var doc = exportSprites(root);
    return JSON.stringify(doc, null, "\t");
}
exportData.filterName = "exportData";
Library.addFilter("exportData");

