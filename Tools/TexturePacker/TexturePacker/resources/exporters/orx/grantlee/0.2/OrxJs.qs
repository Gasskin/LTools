var buffer = '';

function append() {
    for (var i = 0; i < arguments.length; i++) {
        if (arguments[i] !== undefined) {
            buffer += arguments[i] + '\n';
        }
    }
    if (arguments.length == 0) {
        buffer += '\n';
    }
}

function friendlyName(name) {
    name = name.replace(/[^a-z0-9]/gi, ' ');      // Keep alpha-numeric
    name = name[0].toUpperCase() + name.slice(1); // Proper case
    return name;
}

function getId(root, name) {
    if (!root.exporterProperties.optimizeSectionNames) {
        // No fancy stuff. Just replace some special characters.
        name = name.replace(/[@\[\]]+/g, '_');
        return name;
    }
    
    // 'big  enemy/hard-boss/boss_01' -> ['big', 'enemy', 'hard', 'boss', 'boss', '01']
    var parts = name.split(/[\/_\- ]+/g);
    
    for (var i = 0; i < parts.length; i++) {
        parts[i] = friendlyName(parts[i]);
    }
    
    // Optimize any sequences of equivalent identifiers.
    // E.g. ['Boss', 'Boss', 'Boss1' -> 'Boss1'
    var clean = parts[0];
    for (var i = 1; i < parts.length; i++) {
        var current = parts[i];
        if (i == parts.length - 1) {
            // Skip trailing digits on actual filename!
            current = replaceTrailingDigits(current, '');
        }
        
        if (current !== parts[i - 1]) {
            clean += parts[i];
        }
    }
    
    // Now we have an identifier suitable for .ini files.
    // E.g. 'BigEnemyHardBoss01'
    
    return clean;
}

function section(id) {
    return '[' + id + ']';
}

function frameSection(root, sprite, id) {
    result = section(id);

    if (root.exporterProperties.includeComments) {
        result += ' ; Source: ' + sprite.fullName;
    }
    
    return result;
}

function tagIf(key, value, condition) {
    if (condition) {
        return tag(key, value);
    }
}

function tag(key, value) {
    var maxTagLength = 13; // I.e. TextureOrigin
    while (key.length < maxTagLength) {
        key += ' ';
    }
    return key + ' = ' + value;
}

function pointToString(point) {
    return '(' + point.x + ', ' + point.y + ')';
}

function sortByName(arr) {
    return arr.slice().sort(function(a, b) {
        return a.trimmedName.localeCompare(b.trimmedName);
    });
}

function pivotToString(root, sprite) {
    var pivot = normPointToString(root, sprite.pivotPointNorm);
    if (pivot === undefined) {
        pivot = pointToString(sprite.pivotPoint);
    }
    return pivot;
}

function getPointName(root, c, horizontal) {
    var snap = root.exporterProperties.pixelSnap ? '+truncate' : '';
    
    switch (c) {
        case 0.0: return horizontal ? 'left' : 'top';
        case 0.5: return 'center' + snap;
        case 1.0: return horizontal ? 'right' : 'bottom';
    }
}

function normPointToString(root, point) {
    if (point.x === 0.0 && point.y === 0.0) {
        // 'top left' is default.
        return;
    }
    
    var nx = getPointName(root, point.x, true);
    var ny = getPointName(root, point.y, false);
    
    if (nx !== undefined && ny !== undefined) {
        // No need to repeat identifier! E.g. 'center center'
        if (nx === ny) {
            return nx;
        }
        return nx + ' ' + ny;
    }
    
    // orx wants absolute coordinates!
}

function sizeToString(size) {
    return '(' + size.width + ', ' + size.height + ')';
}

function printAnimation(root, sprite, texture, frameData) {
    if (!frameData.animation) {
        return;
    }
    
    // Only generate animation set for the first frame!
    if (frameData.frameNo !== 1) {
        return;
    }
    
    var animationSetid = getContainerId(root, sprite.trimmedName, 'animation-set');
    var animId = getContainerId(root, sprite.trimmedName, 'anim');
    var textureId = getId(root, texture.trimmedName);
    
    append(section(animationSetid),
           tag('Texture', '@' + textureId),
           tagIf('TextureOrigin', pointToString(sprite.frameRect), frameData.skippedFrames),
           tagIf('FrameSize', sizeToString(sprite.size), frameData.skippedFrames),
           tagIf('Pivot', pivotToString(root, sprite), root.settings.writePivotPoints),
           tag('KeyDuration', root.exporterProperties.keyDuration),
           tag('StartAnim', animId),
           tagIf(animId, frameData.frameCount, frameData.skippedFrames)); // Required only if there are frames with no texture coordinates!
    
    append();
}

function printFrame(root, sprite, texture, frameData) {
    if (frameData.skipped) {
        return;
    }
    
    var id = getFrameId(root, sprite, frameData);
    var textureId = getId(root, texture.trimmedName);
    
    append(frameSection(root, sprite, id),
           tagIf('Texture', '@' + textureId, !frameData.animation),
           tag('TextureOrigin', pointToString(sprite.frameRect)),
           tagIf('TextureSize', sizeToString(sprite.frameRect), !frameData.skippedFrames), // FrameSize already set?
           tagIf('Pivot', pivotToString(root, sprite), root.settings.writePivotPoints && !frameData.animation));
    
    append();
}

function getFrameId(root, sprite, frameData) {
    var name = sprite.trimmedName;
    
    if (frameData.animation) {
        var anim = 'anim' + frameData.frameNo;
        name = getContainerId(root, name, anim);
    }
    
    return getId(root, name);
}

function replaceTrailingDigits(s, replacement) {
    return s.replace(/(\D)\d+$/gi, '$1' + replacement);
}

function getContainerId(root, name, suffix) {
    name = replaceTrailingDigits(name, '-' + suffix);
    
    return getId(root, name);
}

function sizeEqual(a, b) {
    if (a === undefined || b === undefined) {
        return false;
    }
    
    return a.width == b.width && a.height == b.height;
}

function isNewFrameSet(sprite, prevSprite) {
    if (prevSprite === undefined) {
        return true;
    }
    
    var path = replaceTrailingDigits(sprite.trimmedName, '');
    var prevPath = replaceTrailingDigits(prevSprite.trimmedName, '');
    var size = sprite.untrimmedSize;
    var prevSize = prevSprite.untrimmedSize;
    
    // We have a new frame set if:
    // * Filenames differ, excluding trailing digits.
    // * Original sprite sizes differ.
    return (path !== prevPath || !sizeEqual(size, prevSize));
}

function spritesAligned(texture, sprite, prevSprite) {
    var x = prevSprite.frameRect.x + prevSprite.frameRect.width;
    var y = prevSprite.frameRect.y;
    
    if (x >= texture.size.width) {
        x = 0;
        y = y + prevSprite.frameRect.height;
    }
    
    return (sprite.frameRect.x === x && sprite.frameRect.y === y) && sizeEqual(prevSprite.frameRect, sprite.frameRect);
}

function getFrameData(root, texture, sprites) {
    var results = [];
    
    var start = 0, current = 0;
    var prevSprite;
    
    for (var i = 0; i < sprites.length; i++) {
        var sprite = sprites[i];
        var skipped;
        if (isNewFrameSet(sprite, prevSprite)) {
            skipped = false;
            start = i;
            current = 0;
        } else {
            skipped = spritesAligned(texture, sprite, prevSprite);
        }
        
        prevSprite = sprite;
        
        results.push({
            frameNo: ++current,
            skipped: skipped
        });
        
        // Animations have at least two frames.
        var animation = root.settings.autodetectAnimations && current >= 2;
        
        var skippedFrames = skipped;
        // Update entire frame set.
        for (var j = start; j <= i; j++) {
            skippedFrames = skippedFrames || results[j].skippedFrames;
            
            results[j].frameCount = current;
            results[j].animation = animation;
            results[j].skippedFrames = skippedFrames;
        }
        
        // No need to define the start frame if we skipped frames!
        results[start].skipped = skippedFrames;
    }
    
    return results;
}

function printFrames(root) {
    var textures = root.allResults[root.variantIndex].textures;
        
    for (var i = 0; i < textures.length; i++) {
        var texture = textures[i];
        var sprites = sortByName(texture.sprites);
        
        var frameData = getFrameData(root, texture, sprites);
        
        for (var j = 0; j < sprites.length; j++) {
            var sprite = sprites[j];
            
            printAnimation(root, sprite, texture, frameData[j]);
        }
        
        for (var j = 0; j < sprites.length; j++) {
            var sprite = sprites[j];
            printFrame(root, sprite, texture, frameData[j]);
        }
    }
}

function printTexture(root, texture) {
    var id = getId(root, texture.trimmedName);
    append(section(id),
           tag('Texture', texture.fullName),
           tag('TextureSize', sizeToString(texture.size)),
           tagIf('KeepInCache', true, root.exporterProperties.keepInCache));
    
    append();
}

function printTextures(root) {
    var textures = root.allResults[root.variantIndex].textures;
    
    for (var i = 0; i < textures.length; i++) {
        printTexture(root, textures[i]);
    }
}

var ExportData = function(root) {
    buffer = '';
    
    printTextures(root);
    printFrames(root);
    
    return buffer;
}

ExportData.filterName = 'exportData';
Library.addFilter('ExportData');
