@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

:: 设置TexturePacker可执行文件路径
set TEXPACKER="C:\Program Files\CodeAndWeb\TexturePacker\bin\TexturePacker.exe"

:: 检查TexturePacker是否存在
if not exist %TEXPACKER% (
    echo 错误: 未找到TexturePacker，请确认路径是否正确。
    exit /b 1
)

:: 设置输入输出目录
set INPUT_DIR=C:\Work\client\Master\Card\Assets\Bundles\UI\Images\Global\HeroFrame
set OUTPUT_DIR=C:\Work\client\Master\Card\Assets\Bundles\UI\Images\Global\HeroFrame

:: 确保输出目录存在
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

:: 初始化计数器
set count=0

:: 遍历输入目录中的所有PNG文件
for %%i in ("%INPUT_DIR%\*.png") do (
    set /a count+=1
    
    :: 获取不带扩展名的文件名
    set FILENAME=%%~ni
    
    :: 设置输出文件路径
    set OUTPUT_IMAGE="%OUTPUT_DIR%\!FILENAME!.png"
    set TPSHEET_FILE="%OUTPUT_DIR%\!FILENAME!.json"
    
    echo 正在处理图片 !count!: "%%i"
    
    :: 执行TexturePacker命令
    %TEXPACKER% ^
        --texture-format png ^
        --format json-array ^
        --data !TPSHEET_FILE! ^
        --sheet !OUTPUT_IMAGE! ^
        --algorithm Polygon ^
        --size-constraints AnySize ^
        --max-width 4096 ^
        --max-height 4096 ^
        --trim-mode Polygon ^
        --no-trim ^
        --shape-padding 0 ^
        --border-padding 0 ^
		--extrude 0 ^
        --disable-rotation ^
        "%%i"
    
    if errorlevel 1 (
        echo 处理图片 "%%i" 时出错！
    ) else (
        echo 已导出: !OUTPUT_IMAGE!
        echo 已导出: !TPSHEET_FILE!
    )
    echo.
)

echo 批量处理完成！共处理了 %count% 张图片。

endlocal
pause