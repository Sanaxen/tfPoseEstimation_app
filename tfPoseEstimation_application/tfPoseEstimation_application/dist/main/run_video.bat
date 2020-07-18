call envset.bat

cd tfpose
python run_video.py  --video=%1  --model=%2 --resolution=432x368
