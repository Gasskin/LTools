var FixSpriteName = function(input)
{
  var input = input.rawString();
  input = input.replace(/\./g,"_");
  input = input.replace(/\s+/g,"_"); // whitespace
  input = input.replace(/\\+/g,"_");
  input = input.replace(/\/+/g,"_");
  return input;
};
FixSpriteName.filterName = "FixSpriteName";
FixSpriteName.isSafe = false;
Library.addFilter("FixSpriteName");
