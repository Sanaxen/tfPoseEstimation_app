call envset.bat

cd tfpose
python run_webcam.py  --camera=%1  --resize=432x368 --model=%2
