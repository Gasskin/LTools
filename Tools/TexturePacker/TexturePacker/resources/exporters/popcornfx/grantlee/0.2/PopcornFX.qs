//-----------------------------------------------------------------------------
// some global variables to store overall image width and height
width=1;
height=1;

// set the global width variable {{value|SetGlobalWidth}}
var SetGlobalWidth = function(input)
{
    width=input;
    return "";
};
SetGlobalWidth.filterName = "SetGlobalWidth";
SetGlobalWidth.isSafe = false;
Library.addFilter("SetGlobalWidth");

// set the global height variable {{value|SetGlobalHeight}}
var SetGlobalHeight = function(input)
{
    height=input;
    return "";
};
SetGlobalHeight.filterName = "SetGlobalHeight";
SetGlobalHeight.isSafe = false;
Library.addFilter("SetGlobalHeight");

//-----------------------------------------------------------------------------
// some global variables to store frame start X & Y
startX=1;
startY=1;

// set the global width variable {{value|SetStartX}}
var SetStartX = function(input)
{
    startX=input;
    return "";
};
SetStartX.filterName = "SetStartX";
SetStartX.isSafe = false;
Library.addFilter("SetStartX");

// set the global height variable {{value|SetStartY}}
var SetStartY = function(input)
{
    startY=input;
    return "";
};
SetStartY.filterName = "SetStartY";
SetStartY.isSafe = false;
Library.addFilter("SetStartY");

//-----------------------------------------------------------------------------
// Functions to compute relative X & Y coordinates

// calculate relative x value {{value|BuildRelStartX}}
var BuildRelStartX = function(input)
{
    return String(input/width);
};
BuildRelStartX.filterName = "BuildRelStartX";
BuildRelStartX.isSafe = false;
Library.addFilter("BuildRelStartX");

// calculate relative y value {{value|BuildRelStartY}}
var BuildRelStartY = function(input)
{
    return String(input/height);
};
BuildRelStartY.filterName = "BuildRelStartY";
BuildRelStartY.isSafe = false;
Library.addFilter("BuildRelStartY");

//-----------------------------------------------------------------------------
// Functions to compute relative X & Y end coordinates from width and height

// calculate relative x END value {{value|BuildRelEndX}}
var BuildRelEndX = function(input)
{
    return String((startX + input)/width);
};
BuildRelEndX.filterName = "BuildRelEndX";
BuildRelEndX.isSafe = false;
Library.addFilter("BuildRelEndX");

// calculate relative y START value {{value|BuildRelEndY}}
var BuildRelEndY = function(input)
{
    return String((startY + input)/height);
};
BuildRelEndY.filterName = "BuildRelEndY";
BuildRelEndY.isSafe = false;
Library.addFilter("BuildRelEndY");
