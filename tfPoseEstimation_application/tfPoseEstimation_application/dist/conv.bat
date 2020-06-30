copy %1 %3 /v /y
magick.exe  -resize %2 %3 input.png

:echo magick.exe  -resize %2 %3 input.png
:pause