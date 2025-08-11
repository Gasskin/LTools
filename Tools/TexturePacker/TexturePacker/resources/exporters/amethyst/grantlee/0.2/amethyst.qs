
var ExportData = function(result)
{
    var output = "";
    var texture = result.texture;

    output += "#![enable(implicit_some)]\n";
    output += "List((\n";
    output += "    texture_width: " + texture.size.width + ",\n";
    output += "    texture_height: " + texture.size.height + ",\n";
    output += "    sprites: [\n";

    for (var j = 0; j < result.allSprites.length; j++)
    {
        var sprite = result.allSprites[j];
        output += "        (\n";
        output += "            // " + j + ": " + sprite.fullName + "\n";
        output += "            x: " + sprite.frameRect.x + ",\n";
        output += "            y: " + sprite.frameRect.y + ",\n";
        output += "            width: " + sprite.frameRect.width + ",\n";
        output += "            height: " + sprite.frameRect.height + ",\n";
        output += "            offsets: (" + -sprite.centerOffset.x + ", " + -sprite.centerOffset.y + "),\n";
        output += "        ),\n";
    }
    output = output.slice(0, -2) + "\n"; // remove last ","
    output += "    ]\n";
    output += "))\n";

    return output;
}
ExportData.filterName = "ExportData";
Library.addFilter("ExportData");



var ExportSpriteIds = function(result)
{
    var output = "";
    output += '#![allow(dead_code)]\n';
    output += "\n";

    for (var j = 0; j < result.allSprites.length; j++)
    {
        var sprite = result.allSprites[j];
        output += "pub const " + MakeIdentifier(sprite.trimmedName) + ": usize = " + j +";\n";
    }
    return output;
}
ExportSpriteIds.filterName = "ExportSpriteIds";
Library.addFilter("ExportSpriteIds");


var MakeIdentifier = function(name)
{
    return name.replace(/[^a-zA-Z0-9_]/g, "_")
               .replace(/_+/g, "_")
               .replace(/^([0-9])/g, "_$1")
               .toUpperCase();
};


